using System;
using System.Collections.Generic;
using System.Text;

namespace Vavatech.DotnetCore.Models
{
    public class Order : BaseEntity
    {
        public string Number { get; set; }
        public DateTime CreatedDate { get; set; }

        public Customer Customer { get; set; }

        public ICollection<OrderDetail> Details { get; set; }
    }


    public class OrderDetail : BaseEntity
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
