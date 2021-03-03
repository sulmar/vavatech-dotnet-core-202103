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
        Task<ExchangeRate> GetAsync(string table);
        Task<ExchangeRate> GetAsync(string table, string currencyCode);        
    }


    // dotnet add package Microsoft.AspNet.WebApi.Client

    public class NBPApiExchangeRateService : IExchangeRateService
    {
        private readonly HttpClient client;

        // dotnet add package Microsoft.Extensions.Http
        public NBPApiExchangeRateService(IHttpClientFactory httpClientFactory)
        {
            client = httpClientFactory.CreateClient("nbpapi");
        }

        //public NBPApiExchangeRateService(HttpClient client)
        //{
        //    this.client = client;
        //}

        /// <summary>
        /// Aktualnie obowiązująca tabela kursów typu {table}
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        // http://api.nbp.pl/api/exchangerates/tables/{table}/
        public async Task<ExchangeRate> GetAsync(string table)
        {
            string url = $"api/exchangerates/tables/{table}/";

            HttpResponseMessage response = await client.GetAsync(url);

            //if (response.IsSuccessStatusCode)
            //{
            //        throw new Exception();
            //}

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            Rootobject exchangeRate = JsonConvert.DeserializeObject<Rootobject>(json);



            return exchangeRate.ExchangeRates[0];
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

            //string json = await response.Content.ReadAsStringAsync();

            //ExchangeRate exchangeRate = JsonConvert.DeserializeObject<ExchangeRate>(json);

            ExchangeRate exchangeRate = await response.Content.ReadAsAsync<ExchangeRate>();

            return exchangeRate;
        }
    }


}
