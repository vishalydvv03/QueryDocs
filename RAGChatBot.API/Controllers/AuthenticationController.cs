using Microsoft.AspNetCore.Mvc;
using RAGChatBot.Domain.Dtos;
using RAGChatBot.Infrastructure.ResponseHelpers;
using RAGChatBot.Services.AuthenticationServices;

namespace RAGChatBot.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService authService;
        public AuthenticationController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register")]
        public async Task<ServiceResult> RegisterUser(UserRegister registerModel)
        {
            var result = new ServiceResult();
            if (!ModelState.IsValid)
            {
                result.SetBadRequest("Model Validation Failed");
            }
            else
            {
                result = await authService.RegisterUserService(registerModel);
            }
            return result;
        }

        [HttpPost("login")]
        public async Task<ServiceResult> LoginUser(UserLogin loginModel)
        {
            var result = new ServiceResult();
            if (!ModelState.IsValid)
            {
                result.SetBadRequest("Model Validation Failed");
            }
            else
            {
                result = await authService.LoginUserService(loginModel);
            }
            return result;
        }
    }
}
