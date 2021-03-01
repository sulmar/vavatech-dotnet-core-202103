using System;
using System.Collections.Generic;

namespace Vavatech.DotnetCore.Models
{

    public class Customer : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public decimal? CreditAmount { get; set; }
        public string Pesel { get; set; }
        public CustomerType Type { get; set; }
        public string Description { get; set; }

        public string Username { get; set; }
        public string HashedPassword { get; set; }

        public bool IsRemoved { get; set; }

        public Customer Partner { get; set; }

        // public IEnumerable<Order> Orders { get; set; }
    }
}
