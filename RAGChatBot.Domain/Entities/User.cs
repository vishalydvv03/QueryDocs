using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGChatBot.Domain.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public required string UserName { get; set; }

        [EmailAddress, Required]
        public required string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required, StringLength(10, MinimumLength = 10)]
        public required string ContactNo { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
