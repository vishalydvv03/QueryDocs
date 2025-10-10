using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGChatBot.Domain.Models
{
    public class PineconeSettings
    {
        public string VectorApiKey { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public string Index { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
    }
}
