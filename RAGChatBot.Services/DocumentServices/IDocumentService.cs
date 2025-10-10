using RAGChatBot.Infrastructure.ResponseHelpers;
using Microsoft.AspNetCore.Http;

namespace RAGChatBot.Services.DocumentServices
{
    public interface IDocumentService
    {
        Task<ServiceResult> ProcessDocument(IFormFile file);
    }
}
