using System.Collections.Generic;

namespace CyberSecurityChatbot
{
    public class TaskManager
    {
        private readonly TaskStorageHelper _storage;
        private readonly ActivityLogger _logger;

        public TaskManager(ActivityLogger logger)
        {
            _storage = new TaskStorageHelper();
            _logger = logger;
        }

        public CyberTask AddTask(string title, string description, string reminder)
        {
            var task = _storage.AddTask(title, description, reminder);

            string logMessage = string.IsNullOrEmpty(reminder)
                ? $"Task added: '{title}' (no reminder set)"
                : $"Task added: '{title}' (Reminder: {reminder})";

            _logger.Log(logMessage);

            return task;
        }

        public List<CyberTask> GetAllTasks()
        {
            return _storage.LoadTasks();
        }

        public void MarkAsComplete(int id, string title)
        {
            _storage.MarkAsComplete(id);
            _logger.Log($"Task marked complete: '{title}'");
        }

        public void DeleteTask(int id, string title)
        {
            _storage.DeleteTask(id);
            _logger.Log($"Task deleted: '{title}'");
        }
    }
}