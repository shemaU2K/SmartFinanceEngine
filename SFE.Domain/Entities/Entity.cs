using System;
using System.Collections.Generic;
using System.Text;

namespace SFE.Domain.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
    }
}
