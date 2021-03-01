using System;
using System.Collections.Generic;
using System.Text;

namespace Vavatech.DotnetCore.Models.SearchCriterias
{
    public class CustomerSearchCriteria : Base
    {
        public CustomerType? CustomerType { get; set; }
        public bool? IsRemoved { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
