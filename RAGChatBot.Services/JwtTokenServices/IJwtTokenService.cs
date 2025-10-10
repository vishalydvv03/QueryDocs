using RAGChatBot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGChatBot.Services.JwtTokenServices
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
