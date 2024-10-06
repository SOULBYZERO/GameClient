using Common;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Calculator
{
    public class Project :IProjectMeta
    {
        public string Name { get; set; } = "Calculator";
        public BitmapImage Image => new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}/Assets/Calculator.png"));
        public string AppInfo { get; set; } = "Calculator App Instructions:" +
            "\r\r\n\r\r\nUser Manual:" +
            "\r\r\nLaunch the calculator to start computing. " +
            "\r\nUse the numeric buttons (0-9) to input numbers. " +
            "\r\nClick on mathematical operation buttons (+, -, *, /) to perform calculations. " +
            "\r\nThe top text area shows the current operation, and the bottom area displays the result. " +
            "\r\nUse 'CE' to clear the current entry and 'C' to clear all entries. '=' computes the result of the operation displayed. " +
            "\r\nMinimize or close the app using the header buttons." +
            "\r\r\n\r\r\nFunctionality Explanation:" +
            "\r\r\nThe calculator UI consists of a header for minimizing and closing, a display area for showing current operations and results, " +
            "\r\nand a grid of buttons for numeric input and mathematical operations. Click events on buttons handle numeric input, mathematical operations, and calculation logic. " +
            "\r\nThe calculator supports addition, subtraction, multiplication, and division operations with decimal precision. " +
            "\r\nIt maintains state using private fields for operand values, current operation, and previous operator.";
        public void Run() {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
    }
}
