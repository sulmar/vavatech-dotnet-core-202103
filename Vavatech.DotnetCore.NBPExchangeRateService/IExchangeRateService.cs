using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Vavatech.DotnetCore.NBPExchangeRateService.Models;

namespace Vavatech.DotnetCore.NBPExchangeRateService
{
    public interface IExchangeRateService
    {
        Task<IEnumerable<ExchangeRate>> GetAsync(string table);
        Task<ExchangeRate> GetAsync(string table, string currencyCode);        
    }

    public class NBPApiExchangeRateService : IExchangeRateService
    {
        private readonly HttpClient client;

        public NBPApiExchangeRateService(HttpClient client)
        {
            this.client = client;
        }

        public Task<IEnumerable<ExchangeRate>> GetAsync(string table)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aktualnie obowiązujący kurs waluty {code} z tabeli kursów typu {table}
        /// </summary>
        /// <param name="table"></param>
        /// <param name="currencyCode"></param>
        /// <returns></returns>
        // http://api.nbp.pl/api/exchangerates/rates/{table}/{code}/
        public async Task<ExchangeRate> GetAsync(string table, string code)
        {
            string url = $"api/exchangerates/rates/{table}/{code}/";

            HttpResponseMessage response = await client.GetAsync(url);

            //if (response.IsSuccessStatusCode)
            //{
           //        throw new Exception();
            //}

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            ExchangeRate exchangeRate = JsonConvert.DeserializeObject<ExchangeRate>(json);

            return exchangeRate;
        }
    }


}
