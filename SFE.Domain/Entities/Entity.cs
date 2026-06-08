using System;

namespace SFE.Domain.Entities
{
    /// <summary>
    /// Acts as a base class for all domain entities, enforcing equality by identity rather than attributes.
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Gets or sets the globally unique identifier for this entity.
        /// </summary>
        public Guid Id { get; protected set; } = Guid.NewGuid();
    }
}