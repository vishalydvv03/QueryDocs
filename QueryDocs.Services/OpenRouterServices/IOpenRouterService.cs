using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryDocs.Services.OpenRouterServices
{
    public interface IOpenRouterService
    {
        Task<string?> CompleteChat(string message);
    }
}
