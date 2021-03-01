using System;
using System.Collections.Generic;
using Vavatech.DotnetCore.Models;
using Vavatech.DotnetCore.Models.SearchCriterias;

namespace Vavatech.DotnetCore.IServices
{
    public interface IOrderService : IEntityService<Order, OrderSearchCriteria>
    {
        IEnumerable<Order> Get(DateTime from, DateTime to);
    }
}
