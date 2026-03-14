using SFE.Domain.Exeptions;
using System;
using System.Collections.Generic;
using System.Text;
using SFE.Domain.ValueObjects;

namespace SFE.Domain.Entities
{
    public class Wallet : Entity
    {
        public string Name { get; private set; }
        public Guid UserId { get; private set; }
        public Money Balance { get; private set; }

        // Приватний конструктор для ORM
        private Wallet() { }

        public Wallet(string name, Guid userId, Currency currency)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Назва гаманця не може бути порожньою.");

            Name = name;
            UserId = userId;
            Balance = Money.Zero(currency);
        }
        /// <summary>
        /// Поповнення балансу.
        /// </summary>
        public void AddFunds(Money amount)
        {
            if (amount.Amount <= 0)
                throw new DomainException("Сума поповнення має бути більшою за нуль.");

            Balance += amount;
        }

        /// <summary>
        /// Зняття коштів.
        /// </summary>
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
