using System;

namespace SFE.Domain.Exeptions
{
    /// <summary>
    /// Represents an exception that occurs due to a violation of a core domain or business rule.
    /// </summary>
    public class DomainException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the validation error.</param>
        public DomainException(string message) : base(message) { }
    }
}