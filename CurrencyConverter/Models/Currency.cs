namespace CurrencyConverter.Models
{
    public class Currency
    {
        public string Code { get; set; }
        public double Value { get; set; }

    }

    public class CurrencyResponse
    {
        public Dictionary<string, Currency> Data { get; set; }
    }
}

