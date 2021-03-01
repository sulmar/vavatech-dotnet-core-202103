using System;
using System.Collections.Generic;
using System.Text;

namespace Vavatech.DotnetCore.Models
{
    public class Order : BaseEntity
    {
        public string Number { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
