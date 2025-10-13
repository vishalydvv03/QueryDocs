using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueryDocs.Domain.Models;
using QueryDocs.Infrastructure.ResponseHelpers;
using QueryDocs.Services.PineconeServices;

namespace QueryDocs.API.Controllers
{
    [Authorize]
    [Route("api/query")]
    [ApiController]
    public class QueryController : BaseAPIController
    {
        private readonly IPineconeService pineconeService;
        public QueryController(IPineconeService pineconeService, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            this.pineconeService = pineconeService;
        }

        [HttpPost]
        public async Task<ServiceResult> AskQuery([FromBody] QueryRequest query)
        {
            var result = new ServiceResult();
            if (query == null || string.IsNullOrWhiteSpace(query.Query))
            {
                result.SetBadRequest("Please ask your query");
            }
            else
            {
                result = await pineconeService.GenerateAnswer(query, LoggedInUserId);
            }
            return result;
        }
    }
}
