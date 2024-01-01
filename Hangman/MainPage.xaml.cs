using System.ComponentModel;

namespace Hangman
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        #region UI Properties

        public string Spotlight
        {
            get => spotlight;
            set
            {
                spotlight = value;
                OnPropertyChanged();
            }
        }

        public List<char> Letters
        {
            get => letters;
            set
            {
                letters = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        public string GameStatus
        {
            get => gameStatus;
            set
            {
                gameStatus = value;
                OnPropertyChanged();
            }
        }

        public string CurrentImage
        {
            get => currentImage;
            set
            {
                currentImage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Fields

        List<string> words = new()
        {
            "python",
            "javascript",
            "maui",
            "csharp",
            "mongodb",
            "sql",
            "xaml",
            "word",
            "excel",
            "powerpoint",
            "code",
            "hotreload",
            "snippets",
            "itna",
            "ctmehr",
            "pooyesh"
        };
        string answer = "";
        private string spotlight;
        private List<char> guessed = new();
        private List<char> letters = new();
        private string message;
        private int mistakes = 0;
        private int maxWrong = 6;
        private string gameStatus;
        private string currentImage = "img0.jpg";

        #endregion

        #region Constructor

        public MainPage()
        {
            InitializeComponent();
            Letters.AddRange("abcdefghijklmnopqrstuvwxyz");
            BindingContext = this;
            PickWord();
            CalculateWord(answer, guessed);
        }

        #endregion

        #region GameEngine

        private void PickWord()
        {
            answer =
                words[new Random().Next(0, words.Count)];
        }

        private void CalculateWord(string answer, List<char> guessed)
        {
            var temp =
                answer.Select(x => (guessed.IndexOf(x) >= 0 ? x : '_'))
                .ToArray();

            Spotlight = string.Join(" ", temp);
        }

        private void HandleGuess(char letter)
        {
            if (guessed.IndexOf(letter) == -1)
                guessed.Add(letter);
            if (answer.IndexOf(letter) >= 0)
            {
                CalculateWord(answer, guessed);
                CheckIfGameWon();
            }
            else if (answer.IndexOf(letter) == -1)
            {
                mistakes++;
                UpdateStatus();
                CheckIfGameLost();
                CurrentImage = $"img{mistakes}.jpg";
            }
        }

        private void CheckIfGameWon()
        {
            if (Spotlight.Replace(" ", "") == answer)
            {
                Message = "You Win";
                DisableLetters();
            }
        }

        private void CheckIfGameLost()
        {
            if (mistakes == maxWrong)
            {
                Message = "You Lost!!";
                DisableLetters();
            }
        }

        private void DisableLetters()
        {
            foreach (var child in LettersContainer.Children)
            {
                var btn = child as Button;
                if (btn != null)
                    btn.IsEnabled = false;
            }
        }

        private void EnableLetters()
        {
            foreach (var child in LettersContainer.Children)
            {
                var btn = child as Button;
                if (btn != null)
                    btn.IsEnabled = true;
            }
        }

        private void UpdateStatus()
        {
            GameStatus = $"Error: {mistakes} of {maxWrong}";
        }

        #endregion

        #region EventHandlers

        private void Button_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                var letter = btn.Text;
                btn.IsEnabled = false;
                HandleGuess(letter[0]);
            }
        }

        private void ResetClicked(object sender, EventArgs e)
        {
            mistakes = 0;
            guessed = new List<char>();
            CurrentImage = "img0.jpg";
            PickWord();
            CalculateWord(answer,guessed);
            Message = "";
            UpdateStatus();
            EnableLetters();
        }

        #endregion
    }
}