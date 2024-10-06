using CurrencyConverter.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace CurrencyConverter
{
    public partial class MainWindow : Window
    {
        private readonly Service service;
        private KeyValuePair<string, Currency> From { get; set; }
        private KeyValuePair<string, Currency> To { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            service = new Service();
            LoadCurrencies();
        }

        private async void LoadCurrencies()
        {
            await service.LoadCurrencies();
            foreach (var pair in service.Currencies)
            {
                CurrencyFrom.Items.Add(pair);
                CurrencyTo.Items.Add(pair);
            }
            CurrencyFrom.SelectedIndex = 0;
            CurrencyTo.SelectedIndex = 0;
        }

        private void CurrencyFrom_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            From = (KeyValuePair<string, Currency>)comboBox.SelectedItem;
            UpdateCoversion();
        }

        private void CurrencyTo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            To = (KeyValuePair<string, Currency>)comboBox.SelectedItem;
           UpdateCoversion();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCoversion();
        }

        private void UpdateCoversion()
        {
            if (From.Value != null && To.Value != null)
            {
                double.TryParse(AmountFrom.Text, out double amount);
                double converted = ConvertCurrency(amount, From.Value, To.Value);
                AmountTo.Text = converted.ToString("F2");
            }
            else
            {
                AmountTo.Text = string.Empty;
            }
        }

        private double ConvertCurrency(double amount, Currency from, Currency to)
        {
            double fromRate = from.Value;
            double toRate = to.Value;
            return (amount / fromRate) * toRate;
        }
    }
}