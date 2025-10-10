using RAGChatBot.Domain.Dtos;
using RAGChatBot.Infrastructure.ResponseHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGChatBot.Services.AuthenticationServices
{
    public interface IAuthService
    {
        Task<ServiceResult> RegisterUserService(UserRegister registerModel);
        Task<ServiceResult> LoginUserService(UserLogin loginModel);
    }
}
