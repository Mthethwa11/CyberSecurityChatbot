using System;
using System.IO;
using System.Media;

namespace CyberSecurityChatbot
{
    /// <summary>
    /// Handles audio playback of the voice greeting WAV file.
    /// Gracefully falls back to text if audio is unavailable.
    /// </summary>
    public static class AudioService
    {
        /// <summary>
        /// Attempts to play the greeting.wav audio file.
        /// Returns a status message indicating success or fallback reason.
        /// </summary>
        public static string PlayGreeting()
        {
            try
            {
                string audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");

                if (File.Exists(audioPath))
                {
                    using SoundPlayer player = new SoundPlayer(audioPath);
                    player.PlaySync();
                    return "[Audio] Greeting played successfully!";
                }
                else
                {
                    return "[Audio] File not found. Continuing with text greeting...";
                }
            }
            catch (Exception ex)
            {
                return $"[Audio] Error: {ex.Message}";
            }
        }
    }
}
