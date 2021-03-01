using System;
using System.Collections.Generic;
using Vavatech.DotnetCore.Models;

namespace Vavatech.DotnetCore.IServices
{
    public interface IOrderService : IEntityService<Order>
    {
        IEnumerable<Order> Get(DateTime from, DateTime to);
    }
}
