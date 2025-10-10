using OpenAI.Chat;

namespace RAGChatBot.Services.OpenAIServices
{
    public interface IOpenAIService
    {
        Task<float[]> CreateEmbeddings(string text);
        ChatClient GetChatClient(string? chatModel = null, string? apiKey = null);
    }
}
