using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RAGChatBot.Domain.Models;
using RAGChatBot.Infrastructure.ResponseHelpers;
using RAGChatBot.Services.PineconeServices;
using System.Security.Claims;

namespace RAGChatBot.API.Controllers
{
    [Authorize]
    [Route("api/query")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly IPineconeService pineconeService;
        public QueryController(IPineconeService pineconeService)
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
                result = await pineconeService.GenerateAnswer(query);
            }
            return result;
        }
    }
}
