using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CyberSecurityChatbot
{
    /// <summary>
    /// Code-behind for the main WPF window.
    /// Handles UI interactions, message bubbles, and chatbot engine integration.
    /// </summary>
    public partial class MainWindow : Window
    {
        // ─── Fields ────────────────────────────────────────────────────────────
        private ChatbotEngine _engine;
        private bool _isFirstMessage = true;
        private ActivityLogger _activityLogger;
        private TaskManager _taskManager;
        private System.Collections.Generic.List<CyberTask> _currentTasks = new System.Collections.Generic.List<CyberTask>();
        private QuizManager _quizManager;
        private string _selectedQuizAnswer = "";

        // ─── Constructor ───────────────────────────────────────────────────────
        public MainWindow()
        {
            InitializeComponent();
            _activityLogger = new ActivityLogger();
            _taskManager = new TaskManager(_activityLogger);
            _quizManager = new QuizManager();
            _engine = new ChatbotEngine(_taskManager, _quizManager, _activityLogger);
            _engine.QuizRequested += () => MainTabControl.SelectedIndex = 2;
            _engine.TasksChanged += LoadTasksIntoList;
            LoadTasksIntoList();
            Loaded += MainWindow_Loaded;
        }

        // ─── Startup ───────────────────────────────────────────────────────────

        /// <summary>Runs on window load: plays audio, asks for name, shows welcome.</summary>
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Play voice greeting in background
            await Task.Run(() =>
            {
                string audioStatus = AudioService.PlayGreeting();
                Dispatcher.Invoke(() => AudioStatusLabel.Text = $"[{audioStatus}]");
            });

            // Small pause for readability
            await Task.Delay(300);

            // Ask for user's name
            await AskForNameAsync();
        }

        /// <summary>Shows the name entry dialog and sets up the welcome message.</summary>
        private async Task AskForNameAsync()
        {
            // Show name prompt as a bot message
            AddBotMessage("👋 Welcome to the Cybersecurity Awareness Bot!\n\nMay I have your name?");
            UserInputBox.Focus();

            // Wait for name input (handled by flag in ProcessInput)
        }

        // ─── Message Handling ──────────────────────────────────────────────────

        /// <summary>Sends the user's typed message when Enter is pressed.</summary>
        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrWhiteSpace(UserInputBox.Text))
                ProcessAndSend();
        }

        /// <summary>Sends the user's typed message when Send button is clicked.</summary>
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(UserInputBox.Text))
                ProcessAndSend();
        }

        /// <summary>Processes the input, displays it, and generates a bot response.</summary>
        private async void ProcessAndSend()
        {
            string input = UserInputBox.Text.Trim();
            UserInputBox.Clear();

            // ── First message = user's name ────────────────────────────────────
            if (_isFirstMessage)
            {
                _isFirstMessage = false;
                string name = string.IsNullOrWhiteSpace(input) ? "Friend" : input;
                _engine.UserName = name;

                AddUserMessage(name);
                await Task.Delay(400);

                // Show welcome message
                string welcome = $"✅ Welcome, {name.ToUpper()}!\n\n"
                               + "I'm your Cybersecurity Awareness Assistant! 🛡️\n"
                               + "I can help you learn about:\n\n"
                               + "  🔒 Password Safety\n"
                               + "  🎣 Phishing & Scams\n"
                               + "  🌍 Safe Browsing\n"
                               + "  🔐 Privacy & VPNs\n"
                               + "  🦠 Malware & Ransomware\n"
                               + "  📱 Two-Factor Authentication\n\n"
                               + "What would you like to learn about today?\n"
                               + "Type 'help' to see all topics, or just ask away!";

                AddBotMessage(welcome);
                UpdateStatusBar();
                return;
            }

            // ── Regular conversation ───────────────────────────────────────────
            AddUserMessage(input);

            // Slight delay to simulate "thinking"
            await Task.Delay(500);

            string response = _engine.GetResponse(input);

            // Handle exit
            if (response == "__EXIT__")
            {
                string goodbye = $"👋 Goodbye, {_engine.UserName}! Stay safe online!\n"
                               + "Remember: Think before you click! 🛡️";
                AddBotMessage(goodbye, isGoodbye: true);
                await Task.Delay(2000);
                Application.Current.Shutdown();
                return;
            }

            AddBotMessage(response);
            UpdateStatusBar();
        }

        // ─── UI Helper Methods ─────────────────────────────────────────────────

        /// <summary>Adds a user message bubble to the chat panel.</summary>
        private void AddUserMessage(string text)
        {
            var container = new Grid { Margin = new Thickness(0, 4, 0, 4) };
            container.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            container.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var bubble = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0x0D, 0x21, 0x37)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0x00, 0xE5, 0xFF)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(10, 2, 10, 10),
                Padding = new Thickness(12, 8, 12, 8),
                MaxWidth = 600,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            var stack = new StackPanel();
            stack.Children.Add(new TextBlock
            {
                Text = "YOU",
                Foreground = new SolidColorBrush(Color.FromRgb(0x00, 0xE5, 0xFF)),
                FontSize = 9,
                FontFamily = new FontFamily("Consolas"),
                Margin = new Thickness(0, 0, 0, 3)
            });
            stack.Children.Add(new TextBlock
            {
                Text = text,
                Foreground = new SolidColorBrush(Color.FromRgb(0xE0, 0xE0, 0xE0)),
                FontFamily = new FontFamily("Consolas"),
                FontSize = 13,
                TextWrapping = TextWrapping.Wrap
            });

            bubble.Child = stack;
            Grid.SetColumn(bubble, 1);
            container.Children.Add(bubble);

            ChatPanel.Children.Add(container);
            ScrollToBottom();
            AnimateFadeIn(container);
        }

        /// <summary>Adds a bot message bubble to the chat panel.</summary>
        private void AddBotMessage(string text, bool isGoodbye = false)
        {
            var container = new Grid { Margin = new Thickness(0, 4, 0, 4) };
            container.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            container.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Color borderColor = isGoodbye
                ? Color.FromRgb(0xFF, 0x00, 0xFF)
                : Color.FromRgb(0x00, 0xFF, 0x41);

            var bubble = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0x0A, 0x1A, 0x0A)),
                BorderBrush = new SolidColorBrush(borderColor),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(2, 10, 10, 10),
                Padding = new Thickness(14, 10, 14, 10),
                MaxWidth = 650,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            var stack = new StackPanel();
            stack.Children.Add(new TextBlock
            {
                Text = "[BOT]",
                Foreground = new SolidColorBrush(borderColor),
                FontSize = 9,
                FontFamily = new FontFamily("Consolas"),
                Margin = new Thickness(0, 0, 0, 3)
            });
            stack.Children.Add(new TextBlock
            {
                Text = text,
                Foreground = new SolidColorBrush(Color.FromRgb(0xE0, 0xE0, 0xE0)),
                FontFamily = new FontFamily("Consolas"),
                FontSize = 13,
                TextWrapping = TextWrapping.Wrap,
                LineHeight = 20
            });

            bubble.Child = stack;
            Grid.SetColumn(bubble, 0);
            container.Children.Add(bubble);

            ChatPanel.Children.Add(container);
            ScrollToBottom();
            AnimateFadeIn(container);
        }

        /// <summary>Adds a system/info message (centred, dimmed).</summary>
        private void AddSystemMessage(string text)
        {
            var tb = new TextBlock
            {
                Text = text,
                Foreground = new SolidColorBrush(Color.FromRgb(0x55, 0x55, 0x88)),
                FontFamily = new FontFamily("Consolas"),
                FontSize = 11,
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 6, 0, 6),
                TextWrapping = TextWrapping.Wrap
            };
            ChatPanel.Children.Add(tb);
            ScrollToBottom();
        }

        /// <summary>Animates a message container fading in.</summary>
        private static void AnimateFadeIn(UIElement element)
        {
            element.Opacity = 0;
            var anim = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(250));
            element.BeginAnimation(OpacityProperty, anim);
        }

        /// <summary>Scrolls the chat panel to the most recent message.</summary>
        private void ScrollToBottom()
        {
            Dispatcher.InvokeAsync(() => ChatScrollViewer.ScrollToBottom(),
                System.Windows.Threading.DispatcherPriority.Loaded);
        }

        /// <summary>Updates the status bar with current user name and sentiment.</summary>
        private void UpdateStatusBar()
        {
            StatusUserLabel.Text = $"USER: {_engine.UserName.ToUpper()}";

            string sentiment = _engine.CurrentSentiment;
            StatusSentimentLabel.Text = sentiment;
            StatusSentimentLabel.Foreground = sentiment switch
            {
                "worried" => new SolidColorBrush(Color.FromRgb(0xFF, 0x88, 0x00)),
                "curious" => new SolidColorBrush(Color.FromRgb(0x00, 0xFF, 0x41)),
                "frustrated" => new SolidColorBrush(Color.FromRgb(0xFF, 0x44, 0x44)),
                "happy" => new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0x00)),
                _ => new SolidColorBrush(Color.FromRgb(0x00, 0xE5, 0xFF))
            };
        }

        // ─── Button Handlers ───────────────────────────────────────────────────

        /// <summary>Clears the chat panel and shows a divider message.</summary>
        private void ClearChat_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Children.Clear();
            AddSystemMessage("─── Chat cleared ───");
            AddBotMessage($"Chat cleared! What would you like to know, {_engine.UserName}?");
        }

        /// <summary>Shows the memory profile summary as a bot message.</summary>
        private void ShowMemory_Click(object sender, RoutedEventArgs e)
        {
            string summary = _engine.GetMemorySummary();
            AddSystemMessage("─── Memory Profile ───");
            AddBotMessage($"📋 Here's what I remember about you:\n\n{summary}");
        }

        // ─── Task Assistant Handlers ───────────────────────────────────────────

        /// <summary>Reloads the task list from the database into the TaskListBox.</summary>
        private void LoadTasksIntoList()
        {
            _currentTasks = _taskManager.GetAllTasks();
            TaskListBox.Items.Clear();

            foreach (var task in _currentTasks)
            {
                string status = task.IsComplete ? "[DONE]" : "[ ]";
                string reminderText = string.IsNullOrEmpty(task.Reminder) ? "" : $" | Reminder: {task.Reminder}";
                TaskListBox.Items.Add($"{status} {task.Title} - {task.Description}{reminderText}");
            }
        }

        /// <summary>Adds a new task from the Task Assistant panel.</summary>
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TaskTitleBox.Text.Trim();
            string description = TaskDescriptionBox.Text.Trim();
            string reminder = TaskReminderBox.Text.Trim();

            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Please enter a task title.");
                return;
            }

            _taskManager.AddTask(title, description, reminder);
            LoadTasksIntoList();

            TaskTitleBox.Text = "";
            TaskDescriptionBox.Text = "";
            TaskReminderBox.Text = "";
        }

        /// <summary>Marks the selected task as complete.</summary>
        private void MarkCompleteButton_Click(object sender, RoutedEventArgs e)
        {
            int index = TaskListBox.SelectedIndex;
            if (index < 0 || index >= _currentTasks.Count)
            {
                MessageBox.Show("Please select a task first.");
                return;
            }

            var task = _currentTasks[index];
            _taskManager.MarkAsComplete(task.Id, task.Title);
            LoadTasksIntoList();
        }

        /// <summary>Deletes the selected task.</summary>
        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            int index = TaskListBox.SelectedIndex;
            if (index < 0 || index >= _currentTasks.Count)
            {
                MessageBox.Show("Please select a task first.");
                return;
            }

            var task = _currentTasks[index];
            _taskManager.DeleteTask(task.Id, task.Title);
            LoadTasksIntoList();
        }

        // ─── Quiz Handlers ──────────────────────────────────────────────────────

        /// <summary>Starts (or restarts) the quiz and displays the first question.</summary>
        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            _quizManager.ResetQuiz();
            _activityLogger.Log("Quiz started");
            QuizFeedbackText.Text = "";
            DisplayCurrentQuestion();
        }

        /// <summary>Renders the current question and its answer options as radio buttons.</summary>
        private void DisplayCurrentQuestion()
        {
            QuizOptionsPanel.Children.Clear();
            _selectedQuizAnswer = "";

            var question = _quizManager.GetCurrentQuestion();
            if (question == null)
            {
                QuizQuestionText.Text = "Quiz finished! Press 'Start Quiz' to play again.";
                return;
            }

            QuizScoreLabel.Text = $"Score: {_quizManager.GetScore()} / {_quizManager.GetTotalQuestions()}";
            QuizQuestionText.Text = question.Question;

            foreach (var option in question.Options)
            {
                var radio = new RadioButton
                {
                    Content = option,
                    GroupName = "QuizOptions",
                    Foreground = new SolidColorBrush(Color.FromRgb(0xE0, 0xE0, 0xE0)),
                    FontFamily = new FontFamily("Consolas"),
                    FontSize = 13,
                    Margin = new Thickness(0, 4, 0, 4),
                    Tag = question.IsTrueFalse ? option : option.Substring(0, 1)
                };
                radio.Checked += (s, e) => { _selectedQuizAnswer = radio.Tag.ToString(); };
                QuizOptionsPanel.Children.Add(radio);
            }
        }

        /// <summary>Submits the selected answer and shows feedback.</summary>
        private void SubmitAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (_quizManager.GetCurrentQuestion() == null)
            {
                MessageBox.Show("Press 'Start Quiz' first.");
                return;
            }

            if (string.IsNullOrEmpty(_selectedQuizAnswer))
            {
                MessageBox.Show("Please select an answer first.");
                return;
            }

            bool isCorrect = _quizManager.SubmitAnswer(_selectedQuizAnswer);
            string explanation = _quizManager.GetExplanation();

            QuizFeedbackText.Text = isCorrect
                ? $"✅ Correct! {explanation}"
                : $"❌ Incorrect. {explanation}";

            QuizScoreLabel.Text = $"Score: {_quizManager.GetScore()} / {_quizManager.GetTotalQuestions()}";

            if (_quizManager.IsFinished())
            {
                string finalMsg = _quizManager.GetFinalMessage();
                QuizFeedbackText.Text += $"\n\n🏁 Quiz complete! Final score: {_quizManager.GetScore()}/{_quizManager.GetTotalQuestions()}\n{finalMsg}";
                QuizOptionsPanel.Children.Clear();
                QuizQuestionText.Text = "Quiz finished! Press 'Start Quiz' to play again.";
                _activityLogger.Log($"Quiz completed - score: {_quizManager.GetScore()} out of {_quizManager.GetTotalQuestions()}");
            }
        }

        /// <summary>Advances to the next question after feedback has been shown.</summary>
        private void NextQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (_quizManager.GetCurrentQuestion() == null && !_quizManager.IsFinished())
            {
                MessageBox.Show("Press 'Start Quiz' first.");
                return;
            }

            QuizFeedbackText.Text = "";
            DisplayCurrentQuestion();
        }
    }
}