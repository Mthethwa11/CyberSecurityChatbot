using CyberSecurityChatbot;
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
                    
                    using (SoundPlayer player = new SoundPlayer("greeting.wav"))
                    {
                        player.PlaySync();
                    }
                    Console.WriteLine("[Audio] Greeting played successfully!");
                }
                else
                {
                    Console.WriteLine("[Audio] File not found. Continuing with text greeting...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Audio] Error: {ex.Message}");
            }
        }
    }
}