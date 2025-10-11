
namespace QueryDocs.Domain.Models
{
    public class OpenAISettings
    {
        public string OpenAIApiKey { get; set; } = string.Empty;
        public string EmbeddingModel { get; set; } = string.Empty;
        public string ChatModel { get; set; } = string.Empty;
    }
}
