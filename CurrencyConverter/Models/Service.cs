using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CurrencyConverter.Models
{
    public class Service
    {
        public Dictionary<string, Currency> Currencies { get; private set; }

        private static readonly HttpClient client = new HttpClient();
        public async Task LoadCurrencies()
        {
            string url = "https://api.currencyapi.com/v3/latest?apikey=cur_live_KgTDakeFgNA4pyQIJEiEHYQUdhX2Fh2giTTQVMOd";

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                CurrencyResponse currencyResponse = JsonSerializer.Deserialize<CurrencyResponse>(responseBody, options);

                if (currencyResponse != null)
                {
                    Currencies = currencyResponse.Data;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
