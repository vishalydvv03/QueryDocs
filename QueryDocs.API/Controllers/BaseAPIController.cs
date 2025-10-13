using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QueryDocs.Services.UserServices;
using System.Security.Claims;

namespace QueryDocs.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseAPIController : ControllerBase
    {
        public int LoggedInUserId { get; private set; }
        private readonly IHttpContextAccessor httpContextAccessor;

        public BaseAPIController(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            LoggedInUserId = int.TryParse(userIdClaim, out int parsedUserId) ? parsedUserId : 0;
        }

    }
}
