using RAGChatBot.Domain.Dtos;
using RAGChatBot.Infrastructure.ResponseHelpers;

namespace RAGChatBot.Services.AuthenticationServices
{
    public interface IAuthService
    {
        Task<ServiceResult> RegisterUserService(UserRegister registerModel);
        Task<ServiceResult> LoginUserService(UserLogin loginModel);
    }
}
