using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGChatBot.Services.OpenAIServices
{
    public interface IOpenAIService
    {
        Task<float[]> CreateEmbeddings(string text);
        ChatClient GetChatClient(string? chatModel = null, string? apiKey = null);
    }
}
