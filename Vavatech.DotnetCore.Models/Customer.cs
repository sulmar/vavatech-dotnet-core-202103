using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vavatech.DotnetCore.Models
{
    // dotnet add package System.ComponentModel.Annotations
    public class Customer : BaseEntity
    {
        public string FirstName { get; set; }
        
       // [Required]
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        
        //[Required]
        //[EmailAddress]
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        
       // [Range(1, 1000)]
        public decimal? CreditAmount { get; set; }

        //[Required]
        //[StringLength(11, MinimumLength = 11)]
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
