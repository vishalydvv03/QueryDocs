using QueryDocs.Infrastructure.ResponseHelpers;
using Microsoft.AspNetCore.Http;

namespace QueryDocs.Services.DocumentServices
{
    public interface IDocumentService
    {
        Task<ServiceResult> ProcessDocument(IFormFile file);
    }
}
