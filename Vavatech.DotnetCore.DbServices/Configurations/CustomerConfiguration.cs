using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Vavatech.DotnetCore.Models;

namespace Vavatech.DotnetCore.DbServices.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(p => p.FirstName).HasMaxLength(50);
            builder.Property(p => p.LastName).HasMaxLength(50).IsRequired();
            builder.Property(p => p.Pesel).HasMaxLength(11).IsRequired().IsUnicode(false);
        }
    }
}
