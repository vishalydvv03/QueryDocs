
namespace QueryDocs.Domain.Entities
{
    public class ChatLog
    {
        public int Id { get; set; }
        public string Query { get; set; } = string.Empty;
        public string Response { get; set; }= string.Empty;
        public string ContextChunk { get; set; } = string.Empty;
        public int UserId { get; set; }
        
    }
}
