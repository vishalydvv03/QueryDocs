using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;
using Pinecone;
using QueryDocs.Domain.Entities;
using QueryDocs.Domain.Models;
using QueryDocs.Infrastructure.DbContexts;
using QueryDocs.Infrastructure.ResponseHelpers;
using QueryDocs.Services.HuggingFaceServices;
using QueryDocs.Services.OpenAIServices;
using QueryDocs.Services.OpenRouterServices;



namespace QueryDocs.Services.PineconeServices
{
    public class PineconeService : IPineconeService
    {
        private readonly ChatDbContext dbContext;
        private readonly PineconeClient pineconeClient;
        private readonly IHuggingFaceService hfService;
        private readonly IOpenAIService openAiService;
        private readonly IOpenRouterService openRouterService;
        private readonly PineconeSettings pineconeSettings;

        public PineconeService(IOptions<PineconeSettings> pineconeSettings, OpenAIClient openAiClient, IHuggingFaceService hfService, IOpenAIService openAiService, IOpenRouterService openRouterService, IOptions<OpenAISettings> openAiSettings, PineconeClient pineconeClient, ChatDbContext dbContext)
        {
            this.pineconeClient = pineconeClient;
            this.pineconeSettings = pineconeSettings.Value;
            this.hfService = hfService;
            this.openAiService = openAiService;
            this.openRouterService = openRouterService;
            this.dbContext = dbContext;
        }

        public async Task UpsertEmbeddingsAsync(List<EmbeddingChunk> embeddingChunks, string fileName, int userId)
        {
            var index = await pineconeClient.GetIndex(pineconeSettings.Index);

            var vectorId = $"{userId}-{fileName}-0";

            var queryResult = await index.Query(vectorId, topK: 1);

            if (queryResult.Any())
            {
                var searchFilter = new MetadataMap
                {
                    ["user"] = new MetadataMap
                    {
                        ["$eq"] = userId.ToString()
                    }
                };
                await index.Delete(filter: searchFilter);
            }

            var vectors = embeddingChunks.Select((chunk, i) => new Vector
            {
                Id = vectorId,
                Values = chunk.Vector,
                Metadata = new MetadataMap
                {
                    ["user"] = userId.ToString(),
                    ["file_name"] = fileName,
                    ["original_text"] = chunk.ChunkText,
                    ["uploaded_at"] = DateTime.UtcNow.ToString("o")
                }
            }).ToList();

            await index.Upsert(vectors);
        }

        public async Task<ServiceResult> GenerateAnswer(QueryRequest query,int userId)
        {
            var result = new ServiceResult();

            var index = await pineconeClient.GetIndex(pineconeSettings.Index);
            var queryVector = await hfService.CreateEmbeddingsFromHuggingFace(query.Query);

            var searchFilter = new MetadataMap
            {
                ["user"] = new MetadataMap
                {
                    ["$eq"] = userId.ToString()
                }
            };



            var searchResult = await index.Query(queryVector, topK: Convert.ToUInt32(query.TopK), includeMetadata: true, filter: searchFilter);

            if (searchResult.Length == 0)
            {
                result.SetSuccess("Sorry, I could not find relevant answers");
            }
            else
            {
                var chunks = searchResult.Where(r => r.Metadata != null && r.Metadata.ContainsKey("original_text"))
                   .Select(r => r.Metadata!["original_text"].ToString()!)
                   .ToList();

                var context = string.Join("\n", chunks);

                var previousChats = await dbContext.ChatLogs
                    .Where(c => c.UserId == userId)
                    .OrderByDescending(c => c.Id)
                    .Take(5)
                    .ToListAsync();

                var history = string.Join("\n", previousChats.Select(c => $"UserQuery: {c.Query}\nBotReponse: {c.Response}\n"));

                var systemMessage = @"
                    You are a helpful, friendly, and professional AI assistant. 
                    Your goal is to provide accurate, concise, and context-aware responses in a human friendly way.

                    Guidelines:
                    - Provide clear, step-by-step explanations for technical questions.
                    - Respond to greetings politely and naturally.
                    - If a question is ambiguous, ask the user for clarification.
                    - Start with a simple explanation before adding technical depth and keep it short and concise.
                    - Use only the information from the provided context or previous conversation.
                    - Never fabricate or assume facts not found in the given data.
                    - When the question refers to past conversation, incorporate the relevant history into your answer.";



                var userMessage = $@"
                    Previous conversation: {history}
                    Context from documents: {context}
                    New question: {query.Query}
                    Answer:";
                

                //var messages = new List<ChatMessage>
                //{
                //    systemMessage,
                //    userMessage
                //};

                //var chatClient = openAiService.GetChatClient();

                //ChatCompletion completion = await chatClient.CompleteChatAsync(messages);
                //string answer = completion.Content[0].Text;

                var answer = await openRouterService.CompleteChat($"{systemMessage}\n{userMessage}") ?? string.Empty;
                var log = new ChatLog
                {
                    Query = query.Query,
                    Response = answer,
                    ContextChunk = context,
                    UserId = userId
                };

                dbContext.ChatLogs.Add(log);
                await dbContext.SaveChangesAsync();
                result.SetSuccess(answer);
            }
            return result;
        }

    }
}
