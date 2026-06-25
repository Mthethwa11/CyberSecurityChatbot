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

GRADING CRITERIA COVERED (PART 2)
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

================================================================================
              CYBERSECURITY AWARENESS CHATBOT — PART 3 (DATABASE,
                    QUIZ, NLP, ACTIVITY LOG)
                         PROG6221 — Programming 2A
================================================================================

PROJECT OVERVIEW (PART 3)
================================================================================
Part 3 extends the Part 2 WPF chatbot with four major new features: a Task
Assistant backed by a SQLite database, a cybersecurity quiz mini-game, NLP-style
intent detection so the chatbot understands varied phrasing, and an Activity Log
that records everything the bot has done. All Part 1 and Part 2 functionality
(keyword recognition, sentiment detection, memory/recall, conversation flow)
remains fully intact and accessible alongside the new features.

NEW FEATURES IMPLEMENTED (PART 3)
================================================================================
1. Task Assistant with Reminders   – Add, view, complete, delete cybersecurity
                                      tasks with optional reminders
2. Database Integration            – SQLite + Entity Framework Core, all CRUD
                                      operations sync between GUI and database.db
3. Cybersecurity Mini-Game (Quiz)  – 11 questions (multiple-choice & true/false)
                                      covering phishing, passwords, safe browsing,
                                      social engineering, 2FA, malware, privacy,
                                      and backups, with immediate feedback and a
                                      final score
4. NLP Simulation                  – Keyword-based intent detection using
                                      string.Contains() so the chatbot understands
                                      varied phrasing for tasks, reminders, the
                                      quiz, and the activity log
5. Activity Log                    – Records every significant chatbot action
                                      with a timestamp; shows the last 10 entries
                                      with a "show more" option for full history

UPDATED FILE STRUCTURE (PART 3 ADDITIONS)
================================================================================
CyberSecurityChatbotGUI/
├── CyberTask.cs               – NEW: Task model (Id, Title, Description,
│                                 Reminder, IsComplete, CreatedAt)
├── LogEntry.cs                – NEW: Activity log model (Id, Description,
│                                 CreatedAt)
├── ApplicationDbContext.cs    – NEW: EF Core DbContext, configures SQLite
│                                 (database.db) and lazy loading proxies
├── TaskStorageHelper.cs       – NEW: All database read/write operations for
│                                 tasks (Load, Add, MarkAsComplete, Delete)
├── TaskManager.cs             – NEW: Business logic layer between GUI and
│                                 TaskStorageHelper; logs actions via
│                                 ActivityLogger
├── ActivityLogger.cs          – NEW: Writes log entries to the database;
│                                 retrieves recent/full log history
├── QuizQuestion.cs            – NEW: Quiz question model (Question, Options,
│                                 CorrectAnswer, Explanation, IsTrueFalse)
├── QuizManager.cs             – NEW: Stores 11 quiz questions, handles scoring,
│                                 feedback, and quiz state
├── ChatbotEngine.cs           – EDITED: Added ProcessIntent() for NLP-style
│                                 intent detection (task/reminder/quiz/log),
│                                 wired to TaskManager, QuizManager, and
│                                 ActivityLogger
├── MainWindow.xaml            – EDITED: Added TabControl with Chat, Tasks,
│                                 and Quiz tabs
└── MainWindow.xaml.cs         – EDITED: Added event handlers for Task
                                  Assistant and Quiz GUI elements

CLASS DESCRIPTIONS (PART 3 ADDITIONS)
================================================================================
ApplicationDbContext.cs
-----------------------
  – DbSet<CyberTask> Tasks  – Tasks table
  – DbSet<LogEntry> Logs    – Activity log table
  – Configured to use SQLite with database.db as the data source

TaskStorageHelper.cs
-----------------------
  – LoadTasks()        – Reads all tasks from the database
  – AddTask()           – Inserts a new task and saves
  – MarkAsComplete(id)  – Updates IsComplete to true for a given task
  – DeleteTask(id)      – Removes a task from the database

TaskManager.cs
-----------------------
  – AddTask() / GetAllTasks() / MarkAsComplete() / DeleteTask()
  – Sits between the GUI/chatbot and TaskStorageHelper
  – Logs every task action via ActivityLogger

QuizManager.cs
-----------------------
  – GetCurrentQuestion() / SubmitAnswer() / GetExplanation()
  – IsFinished() / GetScore() / GetFinalMessage() / ResetQuiz()
  – Holds 11 questions covering all required cybersecurity topics

ActivityLogger.cs
-----------------------
  – Log(action)            – Writes a timestamped entry to the database
  – GetRecentLog(count)     – Returns the last N entries (default 10)
  – GetFullLog()            – Returns the complete log history
  – GetCount()              – Returns total number of log entries

ChatbotEngine.cs (Part 3 additions)
-----------------------
  – ProcessIntent()  – Runs before existing Part 2 logic; detects task,
                        reminder, quiz, and activity log intents from varied
                        phrasing using keyword groups and string.Contains()
  – QuizRequested event – Notifies the GUI to switch to the Quiz tab
  – TasksChanged event  – Notifies the GUI to refresh the Task list

HOW TO RUN THE APPLICATION (PART 3)
================================================================================
1. Open the solution folder in Visual Studio 2022 or later
2. Ensure .NET 8.0 SDK is installed (net8.0-windows)
3. Restore NuGet packages (see below) — Visual Studio usually does this
   automatically on build
4. Place your greeting.wav file in the project root directory
5. Press F5 to build and run
6. database.db is created automatically in the output folder the first time
   a task or log entry is saved — no manual setup required
7. Enter your name when prompted by the bot
8. Use the CHAT tab to talk to the bot, the TASKS tab to manage tasks
   directly, and the QUIZ tab to play the cybersecurity quiz
9. Type 'exit' or 'quit' in the chat to close the application

REQUIRED NUGET PACKAGES (PART 3)
================================================================================
Install via Tools > NuGet Package Manager > Manage NuGet Packages for Solution,
or via the Package Manager Console:

  Install-Package Microsoft.EntityFrameworkCore.Sqlite -Version 8.0.28
  Install-Package Microsoft.EntityFrameworkCore.Proxies -Version 8.0.28

Both packages must match the project's .NET 8.0 target (use the latest 8.x
release of each package).

EXAMPLE INTERACTIONS (PART 3)
================================================================================
You:   Add a task to enable two-factor authentication.
[Bot]  Task added: 'Enable two-factor authentication.' Would you like to set
       a reminder for this task?

You:   Yes, remind me in 5 days.
[Bot]  Got it! I'll remind you in 5 days.

You:   start quiz
[Bot]  🎮 Launching the cybersecurity quiz! Head to the QUIZ tab to play —
       your first question is waiting.

You:   show activity log
[Bot]  Here's a summary of recent actions:
       1. Task added: 'Enable two-factor authentication' (Reminder: in 5 days)
       2. Reminder set: 'Enable two-factor authentication' (in 5 days)
       3. Quiz started

NLP INTENT GROUPS (PART 3)
================================================================================
Intent          | Example Phrases Detected
──────────────────────────────────────────────────────────────────────────────
Add task        | "add task", "add a task", "create task", "enable"
Set reminder    | "remind me", "reminder", "set a reminder", "don't forget"
Start quiz      | "start quiz", "take quiz", "quiz me", "play the game"
Show log        | "show activity log", "what have you done", "show log"

QUIZ TOPICS COVERED (PART 3)
================================================================================
  – Phishing                – Password safety
  – Safe browsing (HTTPS)   – Social engineering
  – Two-factor authentication – Malware and ransomware
  – Privacy settings        – Data backup (3-2-1 rule)

TECHNICAL REQUIREMENTS (PART 3 ADDITIONS)
================================================================================
  – Database: SQLite (database.db), accessed via Entity Framework Core 8.x
  – NuGet:    Microsoft.EntityFrameworkCore.Sqlite, Microsoft.EntityFrameworkCore.Proxies

TROUBLESHOOTING (PART 3)
================================================================================
Issue                          | Solution
──────────────────────────────────────────────────────────────────────────────
Tasks not saving                | Confirm both EF Core NuGet packages are
                                  installed and match your .NET version
database.db not found            | It is created automatically on first task/log
                                  write — check the output folder if missing
Quiz button errors on build      | Ensure QuizManager.cs and QuizQuestion.cs
                                  exist and MainWindow.xaml.cs event handlers
                                  match the XAML button Click attributes
Chat doesn't recognise task/quiz | Confirm ChatbotEngine is constructed with
phrases                          | TaskManager, QuizManager, and ActivityLogger
                                  passed in (see MainWindow constructor)

GRADING CRITERIA COVERED (PART 3)
================================================================================
[✓] Correct Submission                          (5 Marks)
[✓] GitHub and Releases with Tags                (10 Marks)
[✓] Task Assistant with Reminders (GUI)          (15 Marks)
[✓] Task Assistant Database Integration          (15 Marks)
[✓] Cybersecurity Mini-Game (Quiz) with GUI       (15 Marks)
[✓] NLP Simulation with GUI Interaction          (10 Marks)
[✓] Activity Log Feature with GUI                (10 Marks)
[✓] Combining Parts 1, 2, and 3                  (10 Marks)
[✓] Video Presentation                            (10 Marks)


VIDEO PRESENTATION
================================================================================
YouTube (Unlisted): [https://youtu.be/HyuTxefyL4I?si=py2c2Vv0YHMnyNZn]

RELEASES
================================================================================
v2.0 – Part 2: WPF GUI chatbot with sentiment detection, memory, keyword
       recognition
v3.0 – Part 3: Task Assistant + SQLite database integration
v3.1 – Part 3: Quiz mini-game + Activity Log
v3.2 – Part 3: Full integration of NLP, all parts combined, final version

AUTHOR INFORMATION
================================================================================
Module:      PROG6221 – Programming 2A
Project:     Part 3 – Cybersecurity Awareness Chatbot (Database, Quiz, NLP, Log)
Institution: The Independent Institute of Education (Pty) Ltd
Year:        2026

REFERENCES
================================================================================
Pieterse, H. 2021. The Cyber Threat Landscape in South Africa: A 10-Year Review.
The African Journal of Information and Communication, 28(28).
doi: https://doi.org/10.23962/10339/32213

Microsoft. 2024. WPF Overview – Windows Presentation Foundation.
https://learn.microsoft.com/en-us/dotnet/desktop/wpf/overview/

Microsoft. 2024. Entity Framework Core Overview.
https://learn.microsoft.com/en-us/ef/core/

================================================================================
                              END OF README
================================================================================