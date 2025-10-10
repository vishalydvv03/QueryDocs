using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RAGChatBot.Domain.Dtos;
using RAGChatBot.Domain.Entities;
using RAGChatBot.Infrastructure.DbContexts;
using RAGChatBot.Infrastructure.ResponseHelpers;
using RAGChatBot.Services.JwtTokenServices;

namespace RAGChatBot.Services.AuthenticationServices
{
    public class AuthService : IAuthService
    {
        private readonly ChatDbContext context;
        private readonly PasswordHasher<User> hasher;
        private readonly IJwtTokenService jwtTokenService;
        public AuthService(ChatDbContext context, PasswordHasher<User> hasher, IJwtTokenService jwtTokenService)
        {
            this.context = context;
            this.hasher = hasher;
            this.jwtTokenService = jwtTokenService;
        }
        public async Task<ServiceResult> LoginUserService(UserLogin loginModel)
        {
            var result = new ServiceResult();
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == loginModel.Email && u.IsActive);
            if (user != null)
            {
                var verifyUser = hasher.VerifyHashedPassword(user, user.PasswordHash, loginModel.Password);
                if (verifyUser == PasswordVerificationResult.Success)
                {
                    var token = jwtTokenService.GenerateToken(user);
                    result.SetSuccess(token);
                }
                else
                {
                    result.SetUnAuthorized();
                }
            }
            else
            {
                result.SetBadRequest("Email Not Registered");
            }
            return result;
        }

        public async Task<ServiceResult> RegisterUserService(UserRegister registerModel)
        {
            var result = new ServiceResult();
            var userExists = await context.Users.AnyAsync(u => (u.Email == registerModel.Email || u.ContactNo == registerModel.ContactNo) && u.IsActive);
            if (userExists)
            {
                result.SetConflict();
            }
            else
            {
                var user = new User
                {
                    UserName = registerModel.UserName,
                    Email = registerModel.Email,
                    ContactNo = registerModel.ContactNo,
                };

                user.PasswordHash = hasher.HashPassword(user, registerModel.Password);

                context.Users.Add(user);
                await context.SaveChangesAsync();
                result.SetSuccess();
            }

            return result;
        }
    }
}
