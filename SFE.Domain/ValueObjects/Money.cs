using System;

namespace SFE.Domain.ValueObjects
{
    /// <summary>
    /// A value object representing a specific amount of money in a particular currency.
    /// Designed to enforce currency safety during arithmetic operations.
    /// </summary>
    /// <param name="Amount">The decimal numeric value of the money.</param>
    /// <param name="Currency">The currency form of the money.</param>
    public record Money(decimal Amount, Currency Currency)
    {
        /// <summary>
        /// Creates a new <see cref="Money"/> instance representing zero in the specified currency.
        /// </summary>
        /// <param name="currency">The target currency.</param>
        /// <returns>A <see cref="Money"/> object with an amount of 0.</returns>
        public static Money Zero(Currency currency) => new(0, currency);

        /// <summary>
        /// Adds two <see cref="Money"/> amounts together, ensuring they share the same currency.
        /// </summary>
        /// <param name="a">The first monetary amount.</param>
        /// <param name="b">The second monetary amount.</param>
        /// <returns>A new <see cref="Money"/> instance representing the combined amount.</returns>
        /// <exception cref="InvalidOperationException">Thrown if an attempt is made to add differing currencies.</exception>
        public static Money operator +(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException("Неможливо додати різні валюти без конвертації.");

            return a with { Amount = a.Amount + b.Amount };
        }

        /// <summary>
        /// Subtracts one <see cref="Money"/> amount from another, ensuring they share the same currency.
        /// </summary>
        /// <param name="a">The base monetary amount.</param>
        /// <param name="b">The monetary amount to subtract.</param>
        /// <returns>A new <see cref="Money"/> instance representing the remaining amount.</returns>
        /// <exception cref="InvalidOperationException">Thrown if an attempt is made to subtract differing currencies.</exception>
        public static Money operator -(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException("Неможливо відняти різні валюти.");

            return a with { Amount = a.Amount - b.Amount };
        }
    }
}