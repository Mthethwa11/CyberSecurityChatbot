using System;

namespace CyberSecurityChatbot
{
    public static class ChatbotLogic
    {
        public static string GetResponse(string input, string userName)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "I didn't quite understand that. Could you rephrase?";
            }

            string lowerInput = input.ToLower();

            // Exit commands
            if (lowerInput == "exit" || lowerInput == "quit")
            {
                return "";
            }

            // Greeting responses
            if (lowerInput.Contains("how are you"))
            {
                return $"I'm functioning perfectly, {userName}! Thanks for asking. How can I help you learn about cybersecurity today?";
            }

            // Purpose responses
            if (lowerInput.Contains("purpose") || lowerInput.Contains("what can you do") || lowerInput.Contains("help me"))
            {
                return $"My purpose, {userName}, is to educate South African citizens about cybersecurity threats like phishing, password theft, and unsafe browsing. I can give you tips and advice to stay safe online!";
            }

            // What can I ask about
            if (lowerInput.Contains("what can i ask") || lowerInput.Contains("topics"))
            {
                return $"You can ask me about:{Environment.NewLine}• Password safety{Environment.NewLine}• Phishing scams{Environment.NewLine}• Safe browsing habits{Environment.NewLine}Just type a question like 'Tell me about passwords' or 'What is phishing?'";
            }

            // Password safety
            if (lowerInput.Contains("password") || lowerInput.Contains("passwords"))
            {
                return GetPasswordAdvice();
            }

            // Phishing
            if (lowerInput.Contains("phish") || lowerInput.Contains("scam") || lowerInput.Contains("fake email"))
            {
                return GetPhishingAdvice();
            }

            // Safe browsing
            if (lowerInput.Contains("brows") || lowerInput.Contains("website") || lowerInput.Contains("link") || lowerInput.Contains("click"))
            {
                return GetSafeBrowsingAdvice();
            }

            // Default response for unrecognized input
            return $"I'm still learning, {userName}. I didn't quite understand that. Could you rephrase? Try asking about 'passwords', 'phishing', or 'safe browsing'.";
        }

        private static string GetPasswordAdvice()
        {
            return "🔐 PASSWORD SAFETY TIPS:\n" +
                   "• Use a password that is at least 12 characters long\n" +
                   "• Include uppercase, lowercase, numbers, and symbols\n" +
                   "• Don't use the same password for multiple accounts\n" +
                   "• Use a password manager like Bitwarden or LastPass\n" +
                   "• Never share your password with anyone, not even 'tech support'";
        }

        private static string GetPhishingAdvice()
        {
            return "🎣 PHISHING AWARENESS:\n" +
                   "• Never click links in unexpected emails or SMS messages\n" +
                   "• Check the sender's email address carefully - scammers spoof real companies\n" +
                   "• Hover over links to see the real URL before clicking\n" +
                   "• Legitimate companies won't ask for passwords via email\n" +
                   "• If something seems urgent or too good to be true, it's probably a scam";
        }

        private static string GetSafeBrowsingAdvice()
        {
            return "🌐 SAFE BROWSING TIPS:\n" +
                   "• Look for 'https://' and the padlock icon in the address bar\n" +
                   "• Avoid downloading files from untrusted websites\n" +
                   "• Keep your browser and antivirus software updated\n" +
                   "• Use an ad-blocker to avoid malicious ads\n" +
                   "• Be careful what you download - verify file extensions before opening";
        }
    }
}