using RAGChatBot.Domain.Entities;

namespace RAGChatBot.Services.JwtTokenServices
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
