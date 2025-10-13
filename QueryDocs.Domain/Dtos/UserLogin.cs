using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace QueryDocs.Domain.Dtos
{
    public class UserLogin
    {
        [EmailAddress, Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
