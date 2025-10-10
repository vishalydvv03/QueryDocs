using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGChatBot.Domain.Entities
{
    public class ExceptionLog
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public string? Type { get; set; }
        public string? StackTrace { get; set; }
        public string? URL { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
