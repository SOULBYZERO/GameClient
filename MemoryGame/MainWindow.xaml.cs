using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MemoryGame
{
    public partial class MainWindow : Window
    {
        private int level = 1;
        private int gridSize = 4;
        private int lives = 5;
        private List<Button> sequence;
        private Random random = new Random();
        private readonly Brush correctColor = (Brush)new BrushConverter().ConvertFrom("#B3E6B5");
        private readonly Brush wrongColor = (Brush)new BrushConverter().ConvertFrom("#FF7F7F");

        public MainWindow()
        {
            InitializeComponent();
            InitializeGameGrid();
            UpdateLivesDisplay();
        }

        private void InitializeGameGrid()
        {
            GameGrid.Children.Clear();
            GameGrid.RowDefinitions.Clear();
            GameGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < gridSize; i++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition());
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    Button button = new Button { Background = Brushes.Gray, Margin = new Thickness(5), IsEnabled = false };
                    button.Click += Button_Click;
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    GameGrid.Children.Add(button);
                }
            }
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            ResetButtonColors();
            sequence = GenerateSequence(level + 2); // Example: sequence length = level + 2
            await HighlightSequence();
            EnableGameGrid();
        }

        private List<Button> GenerateSequence(int length)
        {
            HashSet<Button> selectedButtons = new HashSet<Button>();
            List<Button> sequence = new List<Button>();
            while (sequence.Count < length)
            {
                int row = random.Next(gridSize);
                int column = random.Next(gridSize);
                Button button = GameGrid.Children
                                        .Cast<Button>()
                                        .FirstOrDefault(b => Grid.GetRow(b) == row && Grid.GetColumn(b) == column);
                if (!selectedButtons.Contains(button))
                {
                    selectedButtons.Add(button);
                    sequence.Add(button);
                }
            }
            return sequence;
        }

        private async Task HighlightSequence()
        {
            EnableGameGrid();
            // Highlight all buttons in the sequence
            foreach (Button button in sequence)
            {
                button.Background = Brushes.White;
            }
            await Task.Delay(1000); // Keep them lit for 1 second

            // Turn off the highlight
            foreach (Button button in sequence)
            {
                button.Background = Brushes.Gray;
            }
            DisableGameGrid();
        }

        private void ResetButtonColors()
        {
            foreach (Button button in GameGrid.Children)
            {
                button.Background = Brushes.Gray;
                button.IsEnabled = false; // Disable buttons
            }
        }

        private void EnableGameGrid()
        {
            foreach (Button button in GameGrid.Children)
            {
                button.IsEnabled = true; // Enable buttons
            }
        }

        private void DisableGameGrid()
        {
            foreach (Button button in GameGrid.Children)
            {
                button.IsEnabled = false; // Disable buttons
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (sequence.Contains(clickedButton))
            {
                clickedButton.Background = correctColor; // Highlight correct button in green
                sequence.Remove(clickedButton); // Remove the button from the sequence

                if (sequence.Count == 0)
                {
                    MessageBox.Show("Level completed!");
                    level++;
                    LevelTextBlock.Text = level.ToString();
                    if (level % 3 == 0)
                    {
                        gridSize++;
                        InitializeGameGrid();
                    }
                    StartButton_Click(this, null);
                }
            }
            else
            {
                clickedButton.Background = wrongColor; // Highlight wrong button in red
                lives--;
                UpdateLivesDisplay();
                if (lives == 0)
                {
                    MessageBox.Show("No more lives! Game over.");
                    level = 1;
                    gridSize = 4;
                    lives = 5;
                    LevelTextBlock.Text = level.ToString();
                    UpdateLivesDisplay();
                    InitializeGameGrid();
                }
            }
        }

        private void UpdateLivesDisplay()
        {
            LivesStackPanel.Children.Clear();
            for (int i = 0; i < 5; i++)
            {
                var heart = new TextBlock
                {
                    Text = i < lives ? "❤️" : "🖤",
                    FontSize = 24,
                    Margin = new Thickness(2, 0, 2, 0),
                    Foreground = i < lives ? Brushes.Red : Brushes.Gray
                };
                LivesStackPanel.Children.Add(heart);
            }
        }
    }
}
