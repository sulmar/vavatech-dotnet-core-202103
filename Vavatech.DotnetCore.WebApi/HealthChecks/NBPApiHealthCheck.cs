using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vavatech.DotnetCore.NBPExchangeRateService;

namespace Vavatech.DotnetCore.WebApi.HealthChecks
{
    public class NBPApiHealthCheck : IHealthCheck
    {
        private readonly IExchangeRateService exchangeRateService;

        public NBPApiHealthCheck(IExchangeRateService exchangeRateService)
        {
            this.exchangeRateService = exchangeRateService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var exchangeRate = await exchangeRateService.GetAsync("A", "CHF");

                return HealthCheckResult.Healthy($"kurs: {exchangeRate.Rates[0].Mid}");
            }
            catch(Exception e)
            {
                return HealthCheckResult.Degraded("API nie odpowiada", e);
            }
        }
    }
}
