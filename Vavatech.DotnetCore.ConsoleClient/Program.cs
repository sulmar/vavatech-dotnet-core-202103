using System;
using System.Net.Http;
using System.Threading.Tasks;
using Vavatech.DotnetCore.NBPExchangeRateService;
using Vavatech.DotnetCore.NBPExchangeRateService.Models;

namespace Vavatech.DotnetCore.ConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello NBP API!");

            string baseAddress = "http://api.nbp.pl/";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            // client.DefaultRequestHeaders.Add("Authorization", "Basic base64({login:password})");

            await GetExchangeRateTest(client);

           // await GetExchangeRatesTest(client);

        }

        private static async Task GetExchangeRatesTest(HttpClient client)
        {
            IExchangeRateService exchangeRateService = new NBPApiExchangeRateService(client);

            ExchangeRate exchangeRate = await exchangeRateService.GetAsync("A");

            foreach (var rate in exchangeRate.Rates)
            {
                Console.WriteLine($"{rate.Currency} {rate.Mid}");
            }
          
        }

        private static async Task GetExchangeRateTest(HttpClient client)
        {
            IExchangeRateService exchangeRateService = new NBPApiExchangeRateService(client);

            ExchangeRate exchangeRate = await exchangeRateService.GetAsync("A", "CHF");

            Console.WriteLine(exchangeRate.Rates[0].Mid);
        }
    }
}
