
namespace RAGChatBot.Domain.Models
{
    public class QueryRequest
    {
        public string Query { get; set; } = string.Empty;
        public int TopK { get; set; } = 5; 
    }
}
