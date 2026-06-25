using System.Collections.Generic;

namespace CyberSecurityChatbot
{
    public class QuizManager
    {
        private List<QuizQuestion> _questions;
        private int _currentIndex = 0;
        private int _score = 0;

        public QuizManager()
        {
            _questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "A) Reply with your password", "B) Delete the email", "C) Report the email as phishing", "D) Ignore it" },
                    CorrectAnswer = "C",
                    Explanation = "Correct! Reporting phishing emails helps prevent scams.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Question = "True or False: Using the same password across multiple accounts is safe.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "False. Reusing passwords means one breach can compromise all your accounts.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Question = "Which of these is the strongest password?",
                    Options = new List<string> { "A) password123", "B) MyName2024", "C) Tr$7!qLp@9z", "D) 12345678" },
                    CorrectAnswer = "C",
                    Explanation = "Correct! Strong passwords mix uppercase, lowercase, numbers, and symbols unpredictably.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Question = "True or False: A padlock icon in the browser address bar guarantees a site is 100% safe.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "False. It only means the connection is encrypted (HTTPS) - the site itself could still be malicious.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Question = "What is 'social engineering' in cybersecurity?",
                    Options = new List<string> { "A) Building secure networks", "B) Manipulating people into revealing info", "C) A type of firewall", "D) Coding social media apps" },
                    CorrectAnswer = "B",
                    Explanation = "Correct! Social engineering tricks people psychologically rather than hacking systems directly.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Question = "True or False: Two-factor authentication (2FA) adds an extra layer of security beyond your password.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "True",
                    Explanation = "True! 2FA requires a second proof of identity, like a code sent to your phone.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Question = "What should you avoid doing on public Wi-Fi?",
                    Options = new List<string> { "A) Browsing news sites", "B) Online banking without a VPN", "C) Checking the weather", "D) Watching videos" },
                    CorrectAnswer = "B",
                    Explanation = "Correct! Public Wi-Fi can expose sensitive transactions to attackers without a VPN.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Question = "True or False: Ransomware encrypts your files and demands payment to unlock them.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "True",
                    Explanation = "True. Ransomware is malicious software that locks your data until a ransom is paid.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Question = "How often should you review your social media privacy settings?",
                    Options = new List<string> { "A) Never", "B) Only when signing up", "C) Regularly", "D) Once a year" },
                    CorrectAnswer = "C",
                    Explanation = "Correct! Privacy settings and app permissions should be reviewed regularly as platforms change.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Question = "What is the recommended backup strategy known as the '3-2-1 rule'?",
                    Options = new List<string> { "A) 3 passwords, 2 devices, 1 account", "B) 3 copies, 2 media types, 1 offsite", "C) 3 logins, 2 emails, 1 phone", "D) 3 firewalls, 2 VPNs, 1 antivirus" },
                    CorrectAnswer = "B",
                    Explanation = "Correct! Keep 3 copies of your data, on 2 different media types, with 1 stored offsite.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Question = "True or False: Disabling auto-connect to Wi-Fi networks improves your security.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "True",
                    Explanation = "True! Auto-connect can link you to fake or insecure networks without your knowledge.",
                    IsTrueFalse = true
                }
            };
        }

        public QuizQuestion GetCurrentQuestion()
        {
            if (IsFinished()) return null;
            return _questions[_currentIndex];
        }

        public bool SubmitAnswer(string answer)
        {
            var current = _questions[_currentIndex];
            bool isCorrect = answer.Trim().Equals(current.CorrectAnswer.Trim(), System.StringComparison.OrdinalIgnoreCase);

            if (isCorrect) _score++;
            _currentIndex++;

            return isCorrect;
        }

        public string GetExplanation()
        {
            int index = _currentIndex - 1;
            if (index < 0 || index >= _questions.Count) return "";
            return _questions[index].Explanation;
        }

        public bool IsFinished()
        {
            return _currentIndex >= _questions.Count;
        }

        public int GetScore() => _score;
        public int GetTotalQuestions() => _questions.Count;

        public string GetFinalMessage()
        {
            double percentage = (double)_score / _questions.Count * 100;
            if (percentage >= 80)
                return "Great job! You're a cybersecurity pro! 🛡️";
            else if (percentage >= 50)
                return "Good effort! Keep learning to stay safe online!";
            else
                return "Keep learning to stay safe online! Review the tips and try again.";
        }

        public void ResetQuiz()
        {
            _currentIndex = 0;
            _score = 0;
        }
    }
}