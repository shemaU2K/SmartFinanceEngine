using SFE.Domain.Exeptions;
using System;
using SFE.Domain.ValueObjects;

namespace SFE.Domain.Entities
{
    /// <summary>
    /// Represents a user's digital wallet, holding a balance in a specific currency.
    /// </summary>
    public class Wallet : Entity
    {
        /// <summary>
        /// Gets the descriptive name of the wallet.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the identifier of the user who owns this wallet.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets the current monetary balance of the wallet.
        /// </summary>
        public Money Balance { get; private set; }

        /// <summary>
        /// Private parameterless constructor required by Object-Relational Mappers (ORMs).
        /// </summary>
        private Wallet() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wallet"/> class with a starting balance of zero.
        /// </summary>
        /// <param name="name">The name of the wallet.</param>
        /// <param name="userId">The owner's user identifier.</param>
        /// <param name="currency">The currency operating within this wallet.</param>
        /// <exception cref="ArgumentException">Thrown if the provided name is empty or whitespace.</exception>
        public Wallet(string name, Guid userId, Currency currency)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Назва гаманця не може бути порожньою.");

            Name = name;
            UserId = userId;
            Balance = Money.Zero(currency);
        }

        /// <summary>
        /// Increases the wallet's balance by the specified monetary amount.
        /// </summary>
        /// <param name="amount">The amount to add.</param>
        /// <exception cref="DomainException">Thrown if the amount is less than or equal to zero.</exception>
        public void AddFunds(Money amount)
        {
            if (amount.Amount <= 0)
                throw new DomainException("Сума поповнення має бути більшою за нуль.");

            Balance += amount;
        }

        /// <summary>
        /// Decreases the wallet's balance by the specified monetary amount.
        /// </summary>
        /// <param name="amount">The amount to withdraw.</param>
        /// <exception cref="DomainException">Thrown if the amount is invalid or the wallet has insufficient funds.</exception>
        public void WithdrawFunds(Money amount)
        {
            if (amount.Amount <= 0)
                throw new DomainException("Сума зняття має бути більшою за нуль.");

            if (Balance.Amount < amount.Amount)
                throw new DomainException("Недостатньо коштів на балансі.");

            Balance -= amount;
        }
    }
}