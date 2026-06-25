using System;

namespace CyberSecurityChatbot
{
    public class LogEntry
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string CreatedAt { get; set; } = DateTime.Now.ToString("[HH:mm]");
    }
}
