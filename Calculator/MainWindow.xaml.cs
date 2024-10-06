using System.Windows;
using System.Windows.Controls;

namespace Calculator
{

    public partial class MainWindow : Window
    {
        private decimal numA = 0;
        private decimal numB = 0;
        private decimal result = 0;
        private string mathOperation = "";
        private string prevOperator = "";
        private bool numIsPressed = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void numBtn_Click(object sender, RoutedEventArgs e)
        {
            numIsPressed = true;
            if (txtInputBottom.Text == "0")
            {
                txtInputBottom.Text = "";
            }

            txtInputBottom.Text = txtInputBottom.Text + ((Button)sender).Content;

            if (txtInputBottom.Text == ".")
            {
                txtInputBottom.Text = "0.";
            }

            if (txtInputBottom.Text.Count(c => c == '.') > 1)
            {
                txtInputBottom.Text = txtInputBottom.Text.Remove(txtInputBottom.Text.Length - 1);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (txtInputBottom.Text == "0")
            {
                return;
            }

            txtInputBottom.Text = txtInputBottom.Text.Remove(txtInputBottom.Text.Length - 1);

            if (txtInputBottom.Text == "")
            {
                txtInputBottom.Text = "0";
                numIsPressed = false;
            }
        }

        private void btnClr_Click(object sender, RoutedEventArgs e)
        {
            txtInputBottom.Text = "0";
            txtInputTop.Text = "";
            numIsPressed = false;
        }

        private void btnClrEntry_Click(object sender, RoutedEventArgs e)
        {
            txtInputBottom.Text = "0";
            numIsPressed = false;
        }

        private void mathBtn_Click(object sender, RoutedEventArgs e)
        {
            mathOperation = " " + ((Button)sender).Content;
            if (numIsPressed)
            {
                if (txtInputTop.Text == "")
                {
                    txtInputTop.Text = txtInputBottom.Text + mathOperation;
                    decimal.TryParse(txtInputBottom.Text, out numA);
                    txtInputBottom.Text = "0";
                    prevOperator = mathOperation;
                }
                else
                {
                    decimal.TryParse(txtInputBottom.Text, out numB);
                    numIsPressed = false;
                    switch (prevOperator)
                    {
                        case string text when text.Contains("+"):
                            result = numA + numB;
                            txtInputTop.Text = result.ToString("0.####") + " " + mathOperation;
                            numA = result;
                            txtInputBottom.Text = "0";
                            prevOperator = mathOperation;
                            break;
                        case string text when text.Contains("-"):
                            result = numA - numB;
                            txtInputTop.Text = result.ToString("0.####") + " " + mathOperation;
                            numA = result;
                            txtInputBottom.Text = "0";
                            prevOperator = mathOperation;
                            break;
                        case string text when text.Contains("*"):
                            result = numA * numB;
                            txtInputTop.Text = result.ToString("0.####") + " " + mathOperation;
                            numA = result;
                            txtInputBottom.Text = "0";
                            prevOperator = mathOperation;
                            break;
                        case string text when text.Contains("/"):
                            if (numA != 0 && numB != 0)
                            {
                                result = numA / numB;
                                txtInputTop.Text = result.ToString("0.####") + " " + mathOperation;
                                numA = result;
                                txtInputBottom.Text = "0";
                                prevOperator = mathOperation;
                            }
                            else
                            {
                                txtInputTop.Text = "0 " + mathOperation;
                                numA = 0;
                                txtInputBottom.Text = "0";
                                prevOperator = mathOperation;
                            }
                            break;
                        default:
                            break;
                    }
                }
            } else
            {
                txtInputTop.Text = result.ToString("0.####") + " " + mathOperation;
                prevOperator = mathOperation;
            }
        }

        private void equalsBtn_Click(object sender, RoutedEventArgs e)
        {
            decimal.TryParse(txtInputBottom.Text, out numB);
            numIsPressed = false;
            switch(txtInputTop.Text)
            {
                case string text when text.Contains("+"):
                    result = numA+numB;
                    txtInputTop.Text =numA.ToString("0.####") + " + " + numB.ToString("0.####") + " = " + result.ToString("0.####");
                    txtInputBottom.Text = "0";
                    numA = result;
                    break;
                case string text when text.Contains("-"):
                    result = numA - numB;
                    txtInputTop.Text = numA.ToString("0.####") + " - " + numB.ToString("0.####") + " = " + result.ToString("0.####");
                    txtInputBottom.Text = "0";
                    numA = result;
                    break;
                case string text when text.Contains("*"):
                    result = numA * numB;
                    txtInputTop.Text = numA.ToString("0.####") + " * " + numB.ToString("0.####") + " = " + result.ToString("0.####");
                    txtInputBottom.Text = "0";
                    numA = result;
                    break;
                case string text when text.Contains("/"):
                    if (numA != 0 && numB != 0)
                    {
                        result = numA / numB;
                        txtInputTop.Text = numA.ToString("0.####") + " / " + numB.ToString("0.####") + " = " + result.ToString("0.####");
                        txtInputBottom.Text = "0";
                        numA = result;
                    } else
                    {
                        txtInputTop.Text = numA.ToString("0.####") + " / " + numB.ToString("0.####") + " = 0";
                        numA = 0;
                        txtInputBottom.Text = "0";
                    }
                    break;
                default:
                    break;
            }
        }
    }
}