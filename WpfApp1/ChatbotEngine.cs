using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityChatbot
{
    /// <summary>
    /// Core chatbot engine handling keyword recognition, random responses,
    /// memory/recall, sentiment detection, conversation flow, and NLP intent routing.
    /// </summary>
    public class ChatbotEngine
    {
        // ─── Fields ────────────────────────────────────────────────────────────
        private readonly Random _random = new Random();
        private string _lastTopic = string.Empty;
        private readonly Dictionary<string, string> _userMemory = new();

        // ─── NLP / Task / Quiz / Log Integration ───────────────────────────────
        private readonly TaskManager _taskManager;
        private readonly QuizManager _quizManager;
        private readonly ActivityLogger _activityLogger;
        private string _pendingTaskTitle = string.Empty;
        private bool _awaitingReminderResponse = false;

        /// <summary>Raised when the chatbot detects a "start quiz" intent, so the GUI can switch tabs.</summary>
        public event Action QuizRequested;

        /// <summary>Raised when a task is added via chat, so the GUI task list can refresh.</summary>
        public event Action TasksChanged;

        public ChatbotEngine(TaskManager taskManager = null, QuizManager quizManager = null, ActivityLogger activityLogger = null)
        {
            _taskManager = taskManager;
            _quizManager = quizManager;
            _activityLogger = activityLogger;
        }

        // ─── Keyword → Multiple Responses (Random Responses feature) ──────────
        private readonly Dictionary<string, List<string>> _responses = new()
        {
            ["password"] = new List<string>
            {
                "🔒 PASSWORD TIP: Use at least 12 characters combining uppercase, lowercase, numbers, and symbols.",
                "🔒 PASSWORD TIP: Never reuse passwords across accounts. A password manager like Bitwarden or LastPass helps!",
                "🔒 PASSWORD TIP: Enable two-factor authentication (2FA) wherever possible — it's a game changer.",
                "🔒 PASSWORD TIP: Avoid using personal details like your name, birthday, or pet's name in passwords.",
                "🔒 PASSWORD TIP: Change your passwords immediately if you suspect any account has been breached."
            },
            ["phish"] = new List<string>
            {
                "🎣 PHISHING TIP: Never click links in unexpected emails or SMS messages — verify first!",
                "🎣 PHISHING TIP: Check the sender's email address carefully — scammers spoof real companies.",
                "🎣 PHISHING TIP: Hover over links to see the real URL before you click.",
                "🎣 PHISHING TIP: Legitimate companies will NEVER ask for your password via email.",
                "🎣 PHISHING TIP: If something feels urgent or too good to be true, it's probably a phishing attempt."
            },
            ["scam"] = new List<string>
            {
                "⚠️ SCAM ALERT: Scammers disguise themselves as trusted organisations. Always verify.",
                "⚠️ SCAM ALERT: If you receive a prize you never entered, it's a scam — delete it!",
                "⚠️ SCAM ALERT: Never send money, gift cards, or personal details to unverified contacts.",
                "⚠️ SCAM ALERT: If a caller claims to be your bank, hang up and call the number on your card."
            },
            ["privacy"] = new List<string>
            {
                "🔐 PRIVACY TIP: Review your social media privacy settings regularly.",
                "🔐 PRIVACY TIP: Limit personal data you share online — less exposure means less risk.",
                "🔐 PRIVACY TIP: Check app permissions before installing — does a flashlight need your contacts?",
                "🔐 PRIVACY TIP: Use a VPN on public Wi-Fi to protect your browsing data.",
                "🔐 PRIVACY TIP: Clear cookies and browsing history regularly to reduce tracking."
            },
            ["malware"] = new List<string>
            {
                "🦠 MALWARE TIP: Keep your antivirus software updated and run regular scans.",
                "🦠 MALWARE TIP: Never download software from untrusted or unofficial websites.",
                "🦠 MALWARE TIP: Be wary of USB drives from unknown sources — they can carry malware.",
                "🦠 MALWARE TIP: Malware often disguises itself as legitimate software — only use official sources."
            },
            ["ransomware"] = new List<string>
            {
                "💀 RANSOMWARE TIP: Always back up your data using the 3-2-1 rule (3 copies, 2 media, 1 offsite).",
                "💀 RANSOMWARE TIP: Never open email attachments from unknown senders — ransomware spreads this way.",
                "💀 RANSOMWARE TIP: Keep your OS and software updated to patch vulnerabilities ransomware exploits.",
                "💀 RANSOMWARE TIP: If infected, disconnect from the internet immediately and contact IT support."
            },
            ["vpn"] = new List<string>
            {
                "🌐 VPN TIP: A VPN encrypts your internet connection, protecting your data on public Wi-Fi.",
                "🌐 VPN TIP: Use a reputable VPN provider — free VPNs often sell your data to third parties.",
                "🌐 VPN TIP: A VPN hides your IP address, making it harder for websites to track you.",
                "🌐 VPN TIP: VPNs are especially important at cafes, airports, or any public Wi-Fi hotspot."
            },
            ["firewall"] = new List<string>
            {
                "🛡️ FIREWALL TIP: A firewall monitors and controls incoming and outgoing network traffic.",
                "🛡️ FIREWALL TIP: Always keep your firewall enabled — it's your first line of digital defence.",
                "🛡️ FIREWALL TIP: Both hardware and software firewalls work together to protect your network.",
                "🛡️ FIREWALL TIP: Firewalls can block malicious traffic before it ever reaches your device."
            },
            ["brows"] = new List<string>
            {
                "🌍 SAFE BROWSING TIP: Always look for 'https://' and the padlock icon in the address bar.",
                "🌍 SAFE BROWSING TIP: Avoid downloading files from untrusted or unfamiliar websites.",
                "🌍 SAFE BROWSING TIP: Keep your browser and extensions updated to patch security holes.",
                "🌍 SAFE BROWSING TIP: Use an ad-blocker to avoid malicious advertisements.",
                "🌍 SAFE BROWSING TIP: Verify file extensions before opening downloaded files."
            },
            ["2fa"] = new List<string>
            {
                "📱 2FA TIP: Two-factor authentication adds a critical extra layer of security beyond passwords.",
                "📱 2FA TIP: Use an authenticator app like Google Authenticator rather than SMS codes where possible.",
                "📱 2FA TIP: Even if your password is stolen, 2FA can prevent unauthorised account access."
            },
            ["backup"] = new List<string>
            {
                "💾 BACKUP TIP: Follow the 3-2-1 rule: 3 copies, 2 different media types, 1 stored offsite.",
                "💾 BACKUP TIP: Back up your data regularly — ransomware victims who back up can recover without paying.",
                "💾 BACKUP TIP: Test your backups periodically to confirm they can actually be restored when needed."
            },
            ["wifi"] = new List<string>
            {
                "📶 WI-FI TIP: Avoid using public Wi-Fi for banking or sensitive transactions.",
                "📶 WI-FI TIP: Use a VPN on public Wi-Fi to encrypt your traffic.",
                "📶 WI-FI TIP: Disable auto-connect to Wi-Fi networks on your devices."
            }
        };

        // ─── Sentiment Detection ───────────────────────────────────────────────
        private readonly Dictionary<string, string[]> _sentimentKeywords = new()
        {
            ["worried"] = new[] { "worried", "scared", "afraid", "nervous", "anxious", "concerned", "fear", "unsafe", "danger" },
            ["curious"] = new[] { "curious", "interested", "wondering", "want to know", "how does", "what is", "learn", "tell me about", "explain" },
            ["frustrated"] = new[] { "frustrated", "annoyed", "confused", "don't understand", "complicated", "difficult", "hard", "lost", "can't figure" },
            ["happy"] = new[] { "happy", "great", "awesome", "good", "excellent", "thanks", "thank you", "helpful", "love it" }
        };

        // ─── Follow-Up Keywords (Conversation Flow) ────────────────────────────
        private readonly string[] _followUpKeywords =
        {
            "more", "another", "again", "explain", "elaborate",
            "continue", "go on", "tell me more", "give me another", "another tip"
        };

        // ─── NLP Intent Keyword Groups ──────────────────────────────────────────
        private readonly string[] _addTaskKeywords =
        {
            "add task", "add a task", "create task", "i need to", "enable", "set up a task"
        };

        private readonly string[] _reminderKeywords =
        {
            "remind me", "reminder", "set a reminder", "remind me to", "don't forget"
        };

        private readonly string[] _quizKeywords =
        {
            "start quiz", "take quiz", "test my knowledge", "quiz me", "play the game"
        };

        private readonly string[] _logKeywords =
        {
            "show activity log", "what have you done", "what did you do", "show log", "recent actions"
        };

        // ─── Properties ────────────────────────────────────────────────────────

        /// <summary>Gets or sets the user's name stored in memory.</summary>
        public string UserName
        {
            get => _userMemory.TryGetValue("name", out var n) ? n : "Friend";
            set => _userMemory["name"] = value;
        }

        /// <summary>Gets the user's detected sentiment for UI display.</summary>
        public string CurrentSentiment { get; private set; } = "neutral";

        // ─── Main Response Method ──────────────────────────────────────────────

        /// <summary>
        /// Processes user input and returns a contextual, personalised response.
        /// Step 0 checks NLP intents (task/reminder/quiz/log) before falling through
        /// to the existing Part 2 flow (sentiment, keyword, memory, follow-up, fallback).
        /// </summary>
        public string GetResponse(string input)
        {
            // ── Input Validation (Error Handling) ──────────────────────────────
            if (string.IsNullOrWhiteSpace(input))
                return "💬 It looks like you didn't type anything. Please enter a question or topic!";

            string lower = input.ToLower().Trim();

            // ── Exit Command ───────────────────────────────────────────────────
            if (lower == "exit" || lower == "quit")
                return "__EXIT__";

            // ── Step 0: NLP Intent Detection (Task / Reminder / Quiz / Log) ─────
            string intentResponse = ProcessIntent(lower, input);
            if (intentResponse != null)
                return intentResponse;

            // ── Detect Sentiment ───────────────────────────────────────────────
            CurrentSentiment = DetectSentiment(lower);
            string sentimentPrefix = BuildSentimentPrefix(CurrentSentiment);

            // ── Memory: Store favourite topic if user mentions interest ─────────
            if (lower.Contains("interested in") || lower.Contains("i like") ||
                lower.Contains("favourite") || lower.Contains("i love"))
            {
                foreach (var key in _responses.Keys)
                {
                    if (lower.Contains(key))
                    {
                        _userMemory["favouriteTopic"] = key;
                        _lastTopic = key;
                        string memResponse = GetRandomResponse(key);
                        return $"Great! I'll remember that you're interested in {key}, {UserName}. Here's a tip:\n\n"
                             + $"{sentimentPrefix}{memResponse}"
                             + $"\n\n📌 Type 'more' anytime for another {key} tip!";
                    }
                }
            }

            // ── Conversation Flow: Follow-Up Questions ─────────────────────────
            if (_followUpKeywords.Any(f => lower.Contains(f)) && !string.IsNullOrEmpty(_lastTopic))
            {
                string followUp = GetRandomResponse(_lastTopic);
                return $"{sentimentPrefix}Here's another tip on {_lastTopic}:\n\n{followUp}"
                     + $"\n\n💡 Still on {_lastTopic}. Type 'more' for yet another tip!";
            }

            // ── Greeting Responses ─────────────────────────────────────────────
            if (lower.Contains("how are you") || lower.Contains("how r u") || lower.Contains("how're you"))
                return $"I'm running smoothly and ready to help, {UserName}! 😊 " +
                       "How can I assist you with cybersecurity today?";

            if (lower.Contains("hello") || lower.Contains("hi ") || lower == "hi" || lower.Contains("hey"))
                return $"Hello, {UserName}! 👋 Great to have you back. What cybersecurity topic can I help you with?";

            // ── Purpose / Topics ───────────────────────────────────────────────
            if (lower.Contains("purpose") || lower.Contains("what can you do") || lower.Contains("what do you do"))
                return $"My purpose, {UserName}, is to educate South Africans about cybersecurity. I can help with:\n\n"
                     + "🔒 Password Safety\n🎣 Phishing & Scams\n🌍 Safe Browsing\n🔐 Privacy\n"
                     + "🦠 Malware & Ransomware\n🌐 VPNs\n🛡️ Firewalls\n📱 Two-Factor Authentication\n💾 Backups\n📶 Wi-Fi Safety";

            if (lower.Contains("what can i ask") || lower.Contains("topics") ||
                lower.Contains("help me") || lower == "help")
                return "Here are the topics I know about:\n\n"
                     + "• 🔒 Passwords    • 🎣 Phishing & Scams\n"
                     + "• 🌍 Safe Browsing • 🔐 Privacy\n"
                     + "• 🦠 Malware       • 💀 Ransomware\n"
                     + "• 🌐 VPN           • 🛡️ Firewall\n"
                     + "• 📱 2FA           • 💾 Backups\n"
                     + "• 📶 Wi-Fi Safety\n\n"
                     + "Just type any of these topics to get started!";

            // ── Keyword Matching (Keyword Recognition) ─────────────────────────
            foreach (var kvp in _responses)
            {
                if (lower.Contains(kvp.Key))
                {
                    _lastTopic = kvp.Key;

                    // Store concern if user is worried
                    if (CurrentSentiment == "worried")
                        _userMemory["concern"] = kvp.Key;

                    string response = GetRandomResponse(kvp.Key);
                    string memoryNote = BuildMemoryNote(kvp.Key);

                    return $"{sentimentPrefix}{response}{memoryNote}";
                }
            }

            // ── Recall: Suggest favourite topic if known ──────────────────────
            if (_userMemory.TryGetValue("favouriteTopic", out var favTopic))
            {
                return $"I'm not sure about that, {UserName}. Since you're interested in {favTopic}, "
                     + $"would you like another tip on that? Just type 'more about {favTopic}' or ask me something else!";
            }

            // ── Default / Unrecognised Input ───────────────────────────────────
            return $"I'm still learning, {UserName}! I didn't quite understand that. 🤔\n\n"
                 + "Try asking about: passwords, phishing, privacy, malware, VPN, firewall, or safe browsing.\n"
                 + "Or type 'help' to see all topics.";
        }

        // ─── NLP Intent Processing ──────────────────────────────────────────────

        /// <summary>
        /// Detects task, reminder, quiz, or log intents from varied phrasing using
        /// keyword groups and string.Contains(). Returns null if no intent matched,
        /// so the caller falls through to the existing Part 2 response flow.
        /// </summary>
        private string ProcessIntent(string lower, string originalInput)
        {
            // ── Awaiting a reminder answer for a task just added ────────────────
            if (_awaitingReminderResponse)
            {
                _awaitingReminderResponse = false;

                bool saidYes = lower.Contains("yes") || lower.Contains("sure") || lower.Contains("ok") || lower.Contains("remind");

                if (saidYes)
                {
                    string reminderText = ExtractReminderText(originalInput);
                    UpdateLastTaskReminder(reminderText);
                    _activityLogger?.Log($"Reminder set: '{_pendingTaskTitle}' ({reminderText})");
                    return $"Got it! I'll remind you {reminderText}.";
                }

                return "No problem, no reminder set for that task.";
            }

            // ── Show activity log intent ────────────────────────────────────────
            if (_logKeywords.Any(k => lower.Contains(k)))
            {
                if (_activityLogger == null)
                    return "Activity log isn't available right now.";

                if (lower.Contains("show more") || lower.Contains("full log") || lower.Contains("everything"))
                    return _activityLogger.GetFullLog();

                string recent = _activityLogger.GetRecentLog(10);
                if (_activityLogger.GetCount() > 10)
                    recent += "\n\n📜 Type 'show more' to see the full history.";

                return recent;
            }

            // ── Start quiz intent ───────────────────────────────────────────────
            if (_quizKeywords.Any(k => lower.Contains(k)))
            {
                if (_quizManager == null)
                    return "The quiz isn't available right now.";

                QuizRequested?.Invoke();
                _activityLogger?.Log("Quiz started");
                return "🎮 Launching the cybersecurity quiz! Head to the QUIZ tab to play — your first question is waiting.";
            }

            // ── Set reminder intent (standalone, e.g. "remind me to update my password tomorrow") ──
            if (_reminderKeywords.Any(k => lower.Contains(k)))
            {
                string taskTitle = ExtractTaskTitle(originalInput, _reminderKeywords);
                string reminderText = ExtractReminderText(originalInput);

                if (_taskManager != null)
                {
                    _taskManager.AddTask(taskTitle, taskTitle, reminderText);
                    TasksChanged?.Invoke();
                }

                return $"Reminder set for '{taskTitle}' {reminderText}.";
            }

            // ── Add task intent ─────────────────────────────────────────────────
            if (_addTaskKeywords.Any(k => lower.Contains(k)))
            {
                string taskTitle = ExtractTaskTitle(originalInput, _addTaskKeywords);

                if (_taskManager != null)
                {
                    _taskManager.AddTask(taskTitle, taskTitle, "");
                    TasksChanged?.Invoke();
                }

                _pendingTaskTitle = taskTitle;
                _awaitingReminderResponse = true;

                return $"Task added: '{taskTitle}.' Would you like to set a reminder for this task?";
            }

            // No NLP intent matched — fall through to existing Part 2 flow
            return null;
        }

        /// <summary>Extracts a clean task title from user input by stripping known trigger phrases.</summary>
        private string ExtractTaskTitle(string originalInput, string[] triggerPhrases)
        {
            string cleaned = originalInput;

            foreach (var phrase in triggerPhrases)
                cleaned = System.Text.RegularExpressions.Regex.Replace(cleaned, phrase, "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // Strip common connector words and reminder time phrases for a cleaner title
            string[] stripWords = { "to ", "that ", "a ", "tomorrow", "today", "please" };
            foreach (var word in stripWords)
                cleaned = cleaned.Replace(word, "", StringComparison.OrdinalIgnoreCase);

            cleaned = cleaned.Trim(' ', '-', '.', '!');

            if (string.IsNullOrWhiteSpace(cleaned))
                cleaned = "New cybersecurity task";

            // Capitalise first letter
            return char.ToUpper(cleaned[0]) + cleaned.Substring(1);
        }

        /// <summary>Extracts a human-readable reminder phrase (e.g. "in 3 days", "tomorrow") from input.</summary>
        private string ExtractReminderText(string input)
        {
            string lower = input.ToLower();

            if (lower.Contains("tomorrow"))
                return "tomorrow";

            var match = System.Text.RegularExpressions.Regex.Match(lower, @"in\s+(\d+)\s+day");
            if (match.Success)
                return $"in {match.Groups[1].Value} days";

            match = System.Text.RegularExpressions.Regex.Match(lower, @"(\d{1,2}\s+\w+\s+\d{4})");
            if (match.Success)
                return $"on {match.Groups[1].Value}";

            return "soon";
        }

        /// <summary>Updates the reminder field on the most recently added task.</summary>
        private void UpdateLastTaskReminder(string reminderText)
        {
            if (_taskManager == null) return;

            var tasks = _taskManager.GetAllTasks();
            var lastTask = tasks.OrderByDescending(t => t.Id).FirstOrDefault();

            if (lastTask != null)
            {
                _taskManager.DeleteTask(lastTask.Id, lastTask.Title);
                _taskManager.AddTask(lastTask.Title, lastTask.Description, reminderText);
                TasksChanged?.Invoke();
            }
        }

        // ─── Private Helper Methods ────────────────────────────────────────────

        /// <summary>Returns a randomly selected response for the given topic.</summary>
        private string GetRandomResponse(string topic)
        {
            if (_responses.TryGetValue(topic, out var list))
                return list[_random.Next(list.Count)];
            return "I don't have information on that topic yet.";
        }

        /// <summary>Detects the user's sentiment from their input.</summary>
        private string DetectSentiment(string input)
        {
            foreach (var kvp in _sentimentKeywords)
                if (kvp.Value.Any(k => input.Contains(k)))
                    return kvp.Key;
            return "neutral";
        }

        /// <summary>Builds an empathetic prefix based on detected sentiment.</summary>
        private string BuildSentimentPrefix(string sentiment)
        {
            return sentiment switch
            {
                "worried" => $"I completely understand your concern, {UserName} — it's natural to feel that way. Here's something reassuring:\n\n",
                "curious" => $"Great question, {UserName}! Your curiosity is the best defence. Here's what you need to know:\n\n",
                "frustrated" => $"No worries at all, {UserName}! Cybersecurity can feel overwhelming. Let me simplify this for you:\n\n",
                "happy" => $"Wonderful, {UserName}! 😊 Keep that positive energy going. Here's more useful info:\n\n",
                _ => ""
            };
        }

        /// <summary>Builds a contextual memory note to append to responses.</summary>
        private string BuildMemoryNote(string topic)
        {
            bool isFavourite = _userMemory.TryGetValue("favouriteTopic", out var fav) && fav == topic;

            if (isFavourite)
                return $"\n\n⭐ Since {topic} is your favourite topic, I've got plenty more tips! Type 'more' anytime.";

            return "\n\n💡 Type 'more' or 'another tip' to get a different tip on this topic.";
        }

        /// <summary>Returns a summary of what the chatbot remembers about the user.</summary>
        public string GetMemorySummary()
        {
            var parts = new List<string> { $"👤 Name: {UserName}" };

            if (_userMemory.TryGetValue("favouriteTopic", out var fav))
                parts.Add($"⭐ Favourite topic: {fav}");
            if (_userMemory.TryGetValue("concern", out var concern))
                parts.Add($"😟 Recent concern: {concern}");
            if (!string.IsNullOrEmpty(_lastTopic))
                parts.Add($"💬 Last discussed: {_lastTopic}");

            return string.Join("\n", parts);
        }
    }
}
