using Microsoft.EntityFrameworkCore;
using QueryDocs.Domain.Entities;

namespace QueryDocs.Infrastructure.DbContexts
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }
        public virtual DbSet<User> Users { get; set; }  
        public virtual DbSet<ExceptionLog> ExceptionLogs { get; set; }  
        public virtual DbSet<ChatLog> ChatLogs { get; set; }  
    }
}
