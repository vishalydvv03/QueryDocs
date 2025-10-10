using RAGChatBot.Infrastructure.ResponseHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RAGChatBot.Services.DocumentServices
{
    public interface IDocumentService
    {
        Task<ServiceResult> ProcessDocument(IFormFile file);
    }
}
