using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityChatbot
{
    public class ActivityLogger
    {
        private readonly ApplicationDbContext db;

        public ActivityLogger()
        {
            db = new ApplicationDbContext();
            db.Database.EnsureCreated();
        }

        public void Log(string action)
        {
            var entry = new LogEntry
            {
                Description = action,
                CreatedAt = System.DateTime.Now.ToString("[HH:mm] ")
            };

            db.Logs.Add(entry);
            db.SaveChanges();
        }

        public string GetRecentLog(int count = 10)
        {
            var entries = db.Logs.OrderByDescending(l => l.Id).Take(count).ToList();
            entries.Reverse();

            if (entries.Count == 0)
                return "No activity has been logged yet.";

            var lines = new List<string>();
            for (int i = 0; i < entries.Count; i++)
            {
                lines.Add($"{i + 1}. {entries[i].CreatedAt}{entries[i].Description}");
            }

            return "Here's a summary of recent actions:\n" + string.Join("\n", lines);
        }

        public string GetFullLog()
        {
            var entries = db.Logs.OrderBy(l => l.Id).ToList();

            if (entries.Count == 0)
                return "No activity has been logged yet.";

            var lines = new List<string>();
            for (int i = 0; i < entries.Count; i++)
            {
                lines.Add($"{i + 1}. {entries[i].CreatedAt}{entries[i].Description}");
            }

            return "Full activity history:\n" + string.Join("\n", lines);
        }

        public int GetCount()
        {
            return db.Logs.Count();
        }
    }
}