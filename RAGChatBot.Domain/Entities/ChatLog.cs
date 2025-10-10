using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGChatBot.Domain.Entities
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
