using System;
using SFE.Domain.Exeptions;
using SFE.Domain.ValueObjects;

namespace SFE.Domain.Entities
{
    /// <summary>
    /// Represents a recorded money transfer between two wallets. This entity tracks historical financial movement.
    /// </summary>
    public class Transaction : Entity
    {
        /// <summary>
        /// Gets the identifier of the wallet from which funds were withdrawn.
        /// </summary>
        public Guid fromWalletId { get; private set; }

        /// <summary>
        /// Gets the identifier of the wallet to which funds were deposited.
        /// </summary>
        public Guid toWalletId { get; private set; }

        /// <summary>
        /// Gets the monetary amount and currency involved in the transaction.
        /// </summary>
        public Money Amount { get; private set; }

        /// <summary>
        /// Gets the exact date and time the transaction was processed (in UTC).
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// Private parameterless constructor required by Object-Relational Mappers (ORMs) like Entity Framework.
        /// </summary>
        private Transaction() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class, applying necessary domain logic and validations.
        /// </summary>
        /// <param name="fromWalletId">The source wallet ID.</param>
        /// <param name="toWalletId">The target wallet ID.</param>
        /// <param name="amount">The monetary amount transferred.</param>
        /// <exception cref="DomainException">Thrown if amount is negative/zero, IDs are empty, or IDs are identical.</exception>
        public Transaction(Guid fromWalletId, Guid toWalletId, Money amount)
        {
            if (amount.Amount <= 0)
                throw new DomainException("Сума транзакції має бути більшою за нуль.");
            if (toWalletId == Guid.Empty || fromWalletId == Guid.Empty)
                throw new DomainException("Отримувач та відправник не можуть бути null.");
            if (toWalletId == fromWalletId)
                throw new DomainException("Отримувач та відправник не можуть бути однаковими.");

            this.fromWalletId = fromWalletId;
            this.toWalletId = toWalletId;
            Amount = amount;
            Timestamp = DateTime.UtcNow;
        }
    }
}