using System;
using System.Media;
using System.IO;

namespace CyberSecurityChatbot
{
    public static class AudioService
    {
        public static void PlayGreeting()
        {
            try
            {
                string audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");
                if (File.Exists(audioPath))
                {
                    using (SoundPlayer player = new SoundPlayer(audioPath))
                    {
                        player.PlaySync();
                    }
                }
                else
                {
                    Console.WriteLine("Audio file not found. Continuing with text greeting...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not play audio: {ex.Message}");
            }
        }
    }
}