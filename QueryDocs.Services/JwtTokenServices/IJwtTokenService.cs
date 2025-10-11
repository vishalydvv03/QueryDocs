using QueryDocs.Domain.Entities;

namespace QueryDocs.Services.JwtTokenServices
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
