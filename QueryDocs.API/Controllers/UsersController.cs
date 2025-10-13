using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QueryDocs.Domain.Dtos;
using QueryDocs.Infrastructure.ResponseHelpers;
using QueryDocs.Services.AuthenticationServices;
using QueryDocs.Services.UserServices;
using System.Security.Claims;

namespace QueryDocs.API.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UsersController : BaseAPIController
    {
        private readonly IUserService userService;
        public UsersController(IUserService userService, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            this.userService = userService;
        }

        [HttpPut("{userId:int}")]
        public async Task<ServiceResult> UpdateUser(int userId, UserUpdate updateModel)
        {
            var result = new ServiceResult();

            if (userId == LoggedInUserId || await userService.IsUserAdmin(LoggedInUserId))
            {
                result = await userService.UpdateUser(userId, updateModel);
            }
            else
            {
                result.SetUnAuthorized();
            }
            return result;
        }

        [HttpDelete("{userId:int}")]
        public async Task<ServiceResult> DeleteUser(int userId)
        {
            var result = new ServiceResult();

            if (userId == LoggedInUserId || await userService.IsUserAdmin(LoggedInUserId))
            {
                result = await userService.DeleteUser(userId);
            }
            else
            {
                result.SetUnAuthorized();
            }
            return result;
        }
    }
}
