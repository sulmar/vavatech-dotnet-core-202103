using System;
using System.Collections.Generic;
using System.Text;

namespace Vavatech.DotnetCore.Models.SearchCriterias
{
    public abstract class SearchCriteria : Base
    {

    }

    public class PeriodSearchCriteria : SearchCriteria
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class CustomerSearchCriteria : PeriodSearchCriteria
    {
        public CustomerType? CustomerType { get; set; }
        public bool? IsRemoved { get; set; }
      
    }

    public class OrderSearchCriteria : PeriodSearchCriteria
    {
        public string OrderNumber { get; set; }
    }
}
