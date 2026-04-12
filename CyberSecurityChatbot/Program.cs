using System;
using System.Media;
using System.Threading;

namespace CyberSecurityChatbot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Cybersecurity Awareness Bot";
            Console.WindowWidth = 100;
            Console.WindowHeight = 40;

            // Step 1: Play voice greeting
            AudioService.PlayGreeting();

            // Step 2: Display ASCII art logo
            ChatbotUI.DisplayAsciiArt();

            // Step 3: Personalized greeting
            string userName = ChatbotUI.GetUserName();
            ChatbotUI.ShowWelcomeMessage(userName);

            // Step 4: Main conversation loop
            string userInput;
            do
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("\n You: ");
                Console.ResetColor();
                userInput = Console.ReadLine();

                string response = ChatbotLogic.GetResponse(userInput, userName);

                ChatbotUI.TypeWriterEffect(response);

            } while (!userInput.ToLower().Equals("exit") && !userInput.ToLower().Equals("quit"));

            ChatbotUI.ShowGoodbyeMessage(userName);
        }
    }
}
