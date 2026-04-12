using System;
using System.Threading;

namespace CyberSecurityChatbot
{
    public static class ChatbotUI
    {
        public static void DisplayAsciiArt()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            string asciiArt = @"
    ╔══════════════════════════════════════════════════════════════════╗
    ║                                                                  ║
    ║         ██████╗██╗   ██╗██████╗ ███████╗██████╗                  ║
    ║        ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗                 ║
    ║        ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝                 ║
    ║        ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗                 ║
    ║        ╚██████╗   ██║   ██████╔╝███████╗██║  ██║                 ║
    ║         ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝                 ║
    ║                                                                  ║
    ║           █████╗ ██╗    ██╗ █████╗ ██████╗ ███████╗              ║
    ║          ██╔══██╗██║    ██║██╔══██╗██╔══██╗██╔════╝              ║
    ║          ███████║██║ █╗ ██║███████║██████╔╝█████╗                ║
    ║          ██╔══██║██║███╗██║██╔══██║██╔══██╗██╔══╝                ║
    ║          ██║  ██║╚███╔███╔╝██║  ██║██║  ██║███████╗              ║
    ║          ╚═╝  ╚═╝ ╚══╝╚══╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝              ║
    ║                                                                  ║
    ║              C Y B E R S E C U R I T Y   B O T                   ║
    ║                                                                  ║
    ╚══════════════════════════════════════════════════════════════════╝";
            Console.WriteLine(asciiArt);
            Console.ResetColor();
            Thread.Sleep(1500);
        }

        public static string GetUserName()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\n [Bot] May I have your name? ");
            Console.ResetColor();
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                name = "Friend";
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($" [Bot] I'll call you {name} then!");
                Console.ResetColor();
            }
            return name;
        }

        public static void ShowWelcomeMessage(string name)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n ╔══════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($" ║                    WELCOME, {name.ToUpper()}!                              ║");
            Console.WriteLine(" ║                                                              ║");
            Console.WriteLine(" ║   I'm your Cybersecurity Awareness Assistant!                ║");
            Console.WriteLine(" ║   I can help you learn about:                                ║");
            Console.WriteLine(" ║                                                              ║");
            Console.WriteLine(" ║   • Password Safety                                         ║");
            Console.WriteLine(" ║   • Phishing Scams                                          ║");
            Console.WriteLine(" ║   • Safe Browsing                                           ║");
            Console.WriteLine(" ║                                                              ║");
            Console.WriteLine(" ║   Type 'exit' or 'quit' to end the conversation.            ║");
            Console.WriteLine(" ╚══════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Thread.Sleep(1000);
        }

        public static void TypeWriterEffect(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n [Bot] ");
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(15); // Typing effect delay
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        public static void ShowGoodbyeMessage(string name)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n ╔══════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($" ║                    Goodbye, {name}! Stay safe online!                 ║");
            Console.WriteLine(" ║                    Remember: Think before you click!              ║");
            Console.WriteLine(" ╚══════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }
    }
}