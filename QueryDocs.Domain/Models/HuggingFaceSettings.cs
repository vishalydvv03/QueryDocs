using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryDocs.Domain.Models
{
    public class HuggingFaceSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public string ModelEndpoint { get; set; } = string.Empty;
    }
}
