using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueryDocs.Infrastructure.ResponseHelpers;
using QueryDocs.Services.DocumentServices;

namespace QueryDocs.API.Controllers
{
    [Authorize]
    [Route("api/docs")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService documentService;
        public DocumentsController(IDocumentService documentService)
        {
            this.documentService = documentService;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<ServiceResult> UploadFile(IFormFile file)
        {
            var result = new ServiceResult();
            if (file == null || file.Length == 0)
            {
                result.SetBadRequest("No file provided.");
            }
            else
            {
                result = await documentService.ProcessDocument(file);
            }

            return result;
        }
    }
}
