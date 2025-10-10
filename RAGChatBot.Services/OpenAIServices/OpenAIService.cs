using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;
using RAGChatBot.Domain.Models;
using System.ClientModel;


namespace RAGChatBot.Services.OpenAIServices
{
    public class OpenAIService : IOpenAIService
    {
        private readonly OpenAIClient client;
        private readonly OpenAISettings openAiSettings;
        public OpenAIService(OpenAIClient client, IOptions<OpenAISettings> openAiSettings)
        {
            this.client = client;
            this.openAiSettings = openAiSettings.Value;
        }
        public async Task<float[]> CreateEmbeddings(string text)
        {
            var embeddingClient = client.GetEmbeddingClient(openAiSettings.EmbeddingModel);
            var response = await embeddingClient.GenerateEmbeddingAsync(input: text);
            return response.Value.ToFloats().ToArray();
        }

        public ChatClient GetChatClient(string? chatModel = null, string? apiKey = null)
        {
            chatModel ??= openAiSettings.ChatModel;
            apiKey ??= openAiSettings.OpenAIApiKey;
            return new ChatClient(
                model: chatModel,
                credential: new ApiKeyCredential(apiKey)
            );
        }


    }
}
