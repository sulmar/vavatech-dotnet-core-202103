using System;
using System.Collections.Generic;
using System.Text;

namespace Vavatech.DotnetCore.NBPExchangeRateService.Models
{

    public class ExchangeRate
    {
        public string Table { get; set; }
        public string Currency { get; set; }
        public string Code { get; set; }
        public Rate[] Rates { get; set; }
    }

    public class Rate
    {
        public string No { get; set; }
        public string EffectiveDate { get; set; }
        public float Mid { get; set; }
    }

}
