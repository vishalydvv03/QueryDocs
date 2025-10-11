using OpenAI.Chat;

namespace QueryDocs.Services.OpenAIServices
{
    public interface IOpenAIService
    {
        Task<float[]> CreateEmbeddings(string text);
        ChatClient GetChatClient(string? chatModel = null, string? apiKey = null);
    }
}
