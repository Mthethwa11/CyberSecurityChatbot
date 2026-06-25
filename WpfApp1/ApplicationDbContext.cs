using Microsoft.EntityFrameworkCore;

namespace CyberSecurityChatbot
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<CyberTask> Tasks { get; set; }
        public DbSet<LogEntry> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}