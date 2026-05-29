================================================================================
              CYBERSECURITY AWARENESS CHATBOT — PART 2 (GUI)
                         PROG6221 — Programming 2A
================================================================================

PROJECT OVERVIEW
================================================================================
This is the Part 2 upgrade of the Cybersecurity Awareness Chatbot. It transforms
the Part 1 console application into a full WPF (Windows Presentation Foundation)
GUI application with advanced features including sentiment detection, memory/recall,
random responses, and seamless conversation flow.

FEATURES IMPLEMENTED
================================================================================
1. GUI Design             – Dark-themed WPF interface with cybersecurity aesthetic
2. ASCII Art Logo         – CYBER AWARE banner displayed in the GUI header
3. Voice Greeting         – greeting.wav plays on application startup
4. Keyword Recognition    – 12 cybersecurity topic keywords with targeted responses
5. Random Responses       – Lists of 3-5 responses per topic, randomly selected
6. Conversation Flow      – "more", "another tip", "explain" follow-up handling
7. Memory & Recall        – Stores name, favourite topic, and recent concern
8. Sentiment Detection    – Detects: worried, curious, frustrated, happy, neutral
9. Error Handling         – Input validation and graceful unknown-input responses
10. Code Optimisation     – OOP classes, Dictionary<string, List<string>>, methods

FILE STRUCTURE
================================================================================
CyberSecurityChatbot/
├── App.xaml                  – WPF application entry point
├── App.xaml.cs               – Application code-behind
├── MainWindow.xaml           – GUI layout with dark cybersecurity theme
├── MainWindow.xaml.cs        – UI logic: message bubbles, events, animation
├── ChatbotEngine.cs          – Core chatbot: keywords, memory, sentiment, flow
├── AudioService.cs           – Voice greeting WAV playback
├── greeting.wav              – Voice greeting audio file
└── README.md                 – This file

CLASS DESCRIPTIONS
================================================================================
App.xaml / App.xaml.cs
-----------------------
  – WPF application entry point
  – Sets StartupUri to MainWindow

MainWindow.xaml
-----------------------
  – Dark-themed GUI layout (background #0A0A0F)
  – Green/cyan accent colors matching cybersecurity aesthetic
  – ASCII art header, status bar, chat area, input area, footer

MainWindow.xaml.cs
-----------------------
  – AddUserMessage()  – Creates right-aligned cyan user chat bubbles
  – AddBotMessage()   – Creates left-aligned green bot chat bubbles
  – AddSystemMessage()– Creates centred system/info messages
  – ProcessAndSend()  – Handles input, calls engine, displays response
  – AnimateFadeIn()   – Fade-in animation for each new message
  – UpdateStatusBar() – Shows username and current detected sentiment
  – ShowMemory_Click()– Displays memory profile summary
  – ClearChat_Click() – Clears chat history

ChatbotEngine.cs
-----------------------
  – GetResponse()          – Main response dispatcher
  – DetectSentiment()      – Identifies: worried/curious/frustrated/happy/neutral
  – BuildSentimentPrefix() – Empathetic response prefix based on mood
  – GetRandomResponse()    – Selects random item from List<string> per topic
  – BuildMemoryNote()      – Appends contextual memory hints to responses
  – GetMemorySummary()     – Returns formatted memory profile string
  – UserName property      – Gets/sets name from _userMemory dictionary
  – CurrentSentiment prop  – Exposes detected sentiment for UI

AudioService.cs
-----------------------
  – PlayGreeting()  – Plays greeting.wav using System.Media.SoundPlayer
                      Falls back gracefully if file not found

HOW TO RUN THE APPLICATION
================================================================================
1. Open the solution folder in Visual Studio 2022 or later
2. Ensure .NET 8.0 SDK is installed (net8.0-windows)
3. Place your greeting.wav file in the project root directory
4. Press F5 to build and run
5. Enter your name when prompted by the bot
6. Type cybersecurity questions to interact with the chatbot
7. Type 'exit' or 'quit' to close the application

EXAMPLE CONVERSATION
================================================================================
[Bot]  May I have your name?
You:   Mnqobi
[Bot]  Welcome, MNQOBI! I'm your Cybersecurity Awareness Assistant...

You:   I'm worried about phishing
[Bot]  I completely understand your concern, Mnqobi — it's natural to feel
       that way. Here's something reassuring:
       🎣 PHISHING TIP: Check the sender's email address carefully...
       💡 Type 'more' for another tip on this topic.

You:   more
[Bot]  Here's another tip on phish:
       🎣 PHISHING TIP: Hover over links to see the real URL before clicking.

You:   I'm interested in privacy
[Bot]  Great! I'll remember that you're interested in privacy, Mnqobi...

[Bot]  📋 MY PROFILE button shows:
       👤 Name: Mnqobi
       ⭐ Favourite topic: privacy
       😟 Recent concern: phish
       💬 Last discussed: privacy

SUPPORTED TOPICS & KEYWORDS
================================================================================
Keyword(s)              Example Questions
─────────────────────────────────────────────────────────────────────────────
password                "Tell me about passwords" / "Password tips"
phish                   "What is phishing?" / "I'm worried about phishing"
scam                    "How do I spot a scam?"
privacy                 "How do I protect my privacy?"
malware                 "What is malware?"
ransomware              "Tell me about ransomware"
vpn                     "What is a VPN?" / "Should I use a VPN?"
firewall                "What does a firewall do?"
brows / website         "Safe browsing tips" / "Website security"
2fa                     "What is 2FA?"
backup                  "How should I back up my data?"
wifi                    "Is public Wi-Fi safe?"

SENTIMENT DETECTION
================================================================================
Keyword Examples         → Detected Mood   → Bot Response Style
─────────────────────────────────────────────────────────────────────────────
worried, scared, afraid  → worried         → Empathetic, reassuring
curious, interested      → curious         → Enthusiastic, informative
frustrated, confused     → frustrated      → Patient, simplified
thanks, great, awesome   → happy           → Warm, encouraging
(none of the above)      → neutral         → Informative, direct

MEMORY FEATURES
================================================================================
The bot remembers:
  • Your name (entered at the start)
  • Your favourite topic (detected from "I'm interested in X")
  • Your most recent concern (detected when sentiment is "worried")
  • The last discussed topic (for follow-up continuity)

View your profile anytime by clicking the "📋 MY PROFILE" button.

TECHNICAL REQUIREMENTS
================================================================================
  – IDE:    Visual Studio 2022 or later
  – OS:     Windows (required for WPF and SoundPlayer)
  – .NET:   .NET 8.0 or later (net8.0-windows)
  – Audio:  System.Media.SoundPlayer (PCM WAV format, Windows only)
  – NuGet:  No additional packages required

TROUBLESHOOTING
================================================================================
Issue                     | Solution
──────────────────────────────────────────────────────────────────────────────
Audio not playing         | Ensure greeting.wav is in bin/Debug/net8.0-windows/
Build errors              | Verify UseWPF is set to true in .csproj
Fonts look wrong          | Ensure Consolas is installed on the system
Window too small          | Resize — minimum 800x600 supported

GRADING CRITERIA COVERED
================================================================================
[✓] GUI Design and Implementation         (10 Marks)
[✓] Keyword Recognition — 12 keywords     (15 Marks)
[✓] Random Responses — Lists per topic    (10 Marks)
[✓] Conversation Flow — follow-ups        (10 Marks)
[✓] Memory and Recall — name/topic/mood   (10 Marks)
[✓] Sentiment Detection — 4 sentiments    (10 Marks)
[✓] Error Handling and Edge Cases         (included in Code)
[✓] Code Optimisation — OOP, Dictionary   (10 Marks)
[✓] Correct Submission                    (5 Marks)
[✓] GitHub and CI Setup                   (10 Marks)
[✓] Video Presentation                    (10 Marks)

AUTHOR INFORMATION
================================================================================
Module:      PROG6221 – Programming 2A
Project:     Part 2 – Cybersecurity Awareness Chatbot (GUI)
Institution: The Independent Institute of Education (Pty) Ltd
Year:        2026

REFERENCES
================================================================================
Pieterse, H. 2021. The Cyber Threat Landscape in South Africa: A 10-Year Review.
The African Journal of Information and Communication, 28(28).
doi: https://doi.org/10.23962/10339/32213

Microsoft. 2024. WPF Overview – Windows Presentation Foundation.
https://learn.microsoft.com/en-us/dotnet/desktop/wpf/overview/

================================================================================
                              END OF README
================================================================================
