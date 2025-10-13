using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace QueryDocs.Domain.Dtos
{
    public class UserRegister
    {
        [Required, MaxLength(100)]
        public required string UserName { get; set; }

        [EmailAddress, Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required, StringLength(10, MinimumLength = 10)]
        public required string ContactNo { get; set; }

    }
}
