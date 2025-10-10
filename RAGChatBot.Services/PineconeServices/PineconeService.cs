using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;
using Pinecone;
using RAGChatBot.Domain.Models;
using RAGChatBot.Infrastructure.ResponseHelpers;
using RAGChatBot.Services.OpenAIServices;
using System.ClientModel;
using System.Security.Claims;


namespace RAGChatBot.Services.PineconeServices
{
    public class PineconeService : IPineconeService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly PineconeClient pineconeClient;
        private readonly IOpenAIService openAiService;
        private readonly PineconeSettings pineconeSettings;

        public PineconeService(IOptions<PineconeSettings> pineconeSettings, IHttpContextAccessor httpContextAccessor, OpenAIClient openAiClient, IOpenAIService openAiService, IOptions<OpenAISettings> openAiSettings, PineconeClient pineconeClient)
        {
            this.pineconeClient = pineconeClient;
            this.pineconeSettings = pineconeSettings.Value;
            this.httpContextAccessor = httpContextAccessor;
            this.openAiService = openAiService;
        }

        public async Task UpsertEmbeddingsAsync(List<EmbeddingChunk> embeddingChunks, string fileName)
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";

            var index = await pineconeClient.GetIndex(pineconeSettings.Index);

            var filter = new MetadataMap
            {
                ["user"] = userId
            };
            await index.Delete(filter);

            var vectors = embeddingChunks.Select((chunk, i) => new Vector
            {
                Id = $"{userId}-{fileName}-{i}-{Guid.NewGuid()}",
                Values = chunk.Vector,
                Metadata = new MetadataMap
                {
                    ["user"] = userId,
                    ["file_name"] = fileName,
                    ["original_text"] = chunk.ChunkText,
                    ["uploaded_at"] = DateTime.UtcNow.ToString("o")
                }
            }).ToList();

            await index.Upsert(vectors);
        }

        public async Task<ServiceResult> GenerateAnswer(QueryRequest query)
        {
            var result = new ServiceResult();

            var index = await pineconeClient.GetIndex(pineconeSettings.Index);
            var queryVector = await openAiService.CreateEmbeddings(query.Query);

            var searchResult = await index.Query(queryVector, topK: Convert.ToUInt32(query.TopK), includeMetadata: true);

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

                var systemMessage = ChatMessage.CreateSystemMessage(
                                @"
                                1.You are a helpful, friendly, and professional AI assistant.
                                2.Always provide clear, step-by-step explanations for technical questions.
                                3.Include examples, code snippets, or best practices when relevant.
                                4.Respond to greetings with a friendly reply.
                                5.If a question is ambiguous, ask politely for clarification.
                                6.When explaining concepts, start simple, then add technical details and keep it summarized.
                                7.Answer only on the basis of context provided in the user message and don't hallucinate.
                                "
                );

                var userMessage = ChatMessage.CreateUserMessage($@"
                            Context: {context}
                            Question: {query.Query}
                            Answer:"
                );

                var messages = new List<ChatMessage>
                {
                    systemMessage,
                    userMessage
                };

                var chatClient = openAiService.GetChatClient();

                ChatCompletion completion = await chatClient.CompleteChatAsync(messages);
                string answer = completion.Content[0].Text;
                result.SetSuccess(answer);
            }
            return result;
        }

    }
}
