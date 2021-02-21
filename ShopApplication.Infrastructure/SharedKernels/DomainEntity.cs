using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApplication.Infrastructure.SharedKernels
{
    public abstract class DomainEntity<T>
    {
        public T Id { get; set; }
        //True if domain entity has an identity
        public bool IsTransient()
        {
            return Id.Equals(default(T));
        }
    }
}
