using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGChatBot.Domain.Models
{
    public class QueryRequest
    {
        public string Query { get; set; } = string.Empty;
        public int TopK { get; set; } = 5; 
    }
}
