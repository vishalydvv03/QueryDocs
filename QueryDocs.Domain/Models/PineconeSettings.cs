
namespace QueryDocs.Domain.Models
{
    public class PineconeSettings
    {
        public string VectorApiKey { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public string Index { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
    }
}
