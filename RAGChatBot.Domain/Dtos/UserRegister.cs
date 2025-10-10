using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGChatBot.Domain.Dtos
{
    public class UserRegister
    {
        [Required, MaxLength(100)]
        public string UserName { get; set; }

        [EmailAddress, Required]
        public required string Email { get; set; }
        public required string Password { get; set; }

        [Required, StringLength(10, MinimumLength = 10)]
        public required string ContactNo { get; set; }

    }
}
