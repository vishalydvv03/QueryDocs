using Microsoft.EntityFrameworkCore;
using RAGChatBot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGChatBot.Infrastructure.DbContexts
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }
        public virtual DbSet<User> Users { get; set; }  
        public virtual DbSet<ExceptionLog> ExceptionLogs { get; set; }  
        public virtual DbSet<ChatLog> ChatLogs { get; set; }  
    }
}
