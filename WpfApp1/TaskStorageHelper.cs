using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityChatbot
{
    public class TaskStorageHelper
    {
        private readonly ApplicationDbContext db;

        public TaskStorageHelper()
        {
            db = new ApplicationDbContext();
            db.Database.EnsureCreated();
        }

        public List<CyberTask> LoadTasks()
        {
            return db.Tasks.ToList();
        }

        public CyberTask AddTask(string title, string description, string reminder)
        {
            var task = new CyberTask
            {
                Title = title,
                Description = description,
                Reminder = reminder,
                IsComplete = false
            };

            db.Tasks.Add(task);
            db.SaveChanges();
            return task;
        }

        public void MarkAsComplete(int id)
        {
            var task = db.Tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.IsComplete = true;
                db.Tasks.Update(task);
                db.SaveChanges();
            }
        }

        public void DeleteTask(int id)
        {
            var task = db.Tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                db.Tasks.Remove(task);
                db.SaveChanges();
            }
        }
    }
}