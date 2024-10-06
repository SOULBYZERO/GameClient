using Common;
using System.Windows.Media.Imaging;

namespace CurrencyConverter
{
    public class Project : IProjectMeta
    {
        public string Name { get; set; } = "Currency Converter";
        public BitmapImage Image => new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}/Assets/CurrencyConverter.png"));
        public string AppInfo { get; set; } = "Currency Converter App Instructions:" +
            "\r\r\n\r\r\nUser Manual:" +
            "\r\r\nOpen the Currency Converter to start." +
            "\r\r\n- Select 'From' and 'To' currencies using dropdowns." +
            "\r\r\n- Enter the amount in 'Amount'." +
            "\r\r\n- The 'Result' displays the converted amount." +
            "\r\r\n\r\r\nFunctionality Explanation:" +
            "\r\r\nThe app uses an API to fetch latest exchange rates." +
            "\r\r\n- Loads currencies on startup using Service class." +
            "\r\r\n- Dropdowns populate with fetched currencies." +
            "\r\r\n- TextChanged event in 'Amount' triggers conversion." +
            "\r\r\n- Conversion formula computes based on selected currencies and amount." +
            "\r\r\n\r\r\nApp Architecture:" +
            "\r\r\nThe app UI features a Viewbox containing a Grid with four rows." +
            "\r\r\n- Top row: Title 'Currency Converter'." +
            "\r\r\n- Second row: 'From' and 'To' ComboBoxes for currency selection." +
            "\r\r\n- Third row: 'Amount' TextBox for user input." +
            "\r\r\n- Bottom row: 'Result' TextBlock for displaying converted amount." +
            "\r\r\n\r\r\nService Class:" +
            "\r\r\n- Loads currency data from 'https://api.currencyapi.com'." +
            "\r\r\n- Deserializes JSON response into Dictionary 'Currencies'." +
            "\r\r\n- Handles exceptions and errors with proper messaging." +
            "\r\r\n\r\r\nError Handling:" +
            "\r\r\n- Handles HTTP request exceptions and general errors gracefully." +
            "\r\r\n- Displays error messages in console for debugging purposes.\"";
        public void Run()
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
    }
}
