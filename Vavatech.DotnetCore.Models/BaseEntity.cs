using System;

namespace Vavatech.DotnetCore.Models
{
    public abstract class BaseEntity<TKey> : Base
    {
        public TKey Id { get; set; }
    }

    public abstract class BaseEntity : BaseEntity<int>
    {
        public DateTime CreatedDate { get; set; }
    }
}
