using QueryDocs.Domain.Dtos;
using QueryDocs.Infrastructure.ResponseHelpers;

namespace QueryDocs.Services.AuthenticationServices
{
    public interface IAuthService
    {
        Task<ServiceResult> RegisterUserService(UserRegister registerModel);
        Task<ServiceResult> LoginUserService(UserLogin loginModel);
    }
}
