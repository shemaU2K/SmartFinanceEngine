using SFE.Domain;
using System;
using System.Collections.Generic;
using static SFE.Domain.Transaction;
using static System.Net.Mime.MediaTypeNames;

namespace SFE.Domain
{
    // --- VALUE OBJECTS ---

    /// <summary>
    /// Перелік підтримуваних валют.
    /// </summary>
    public enum Currency
    {
        UAH,
        USD,
        EUR
    }

    /// <summary>
    /// Value Object 'Money'. 
    /// Демонструє Immutable (незмінність) та інкапсуляцію валютної логіки.
    /// </summary>
    public record Money(decimal Amount, Currency Currency)
    {
        public static Money Zero(Currency currency) => new(0, currency);

        // Перевантаження оператора додавання для безпечних розрахунків
        public static Money operator +(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException("Неможливо додати різні валюти без конвертації.");

            return a with { Amount = a.Amount + b.Amount };
        }

        public static Money operator -(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException("Неможливо відняти різні валюти.");

            return a with { Amount = a.Amount - b.Amount };
        }
    }

    // --- ENTITIES ---

    /// <summary>
    /// Базовий клас сутності.
    /// </summary>
    public abstract class Entity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
    }

    /// <summary>
    /// Aggregate Root 'Wallet'. 
    /// Демонструє 'Rich Domain Model' (багата модель) з приватними сеттерами та Guard Clauses.
    /// </summary>
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
    public class Transaction : Entity
    {
        public Guid fromWalletId { get; private set; }
        public Guid toWalletId { get; private set; }
        public Money Amount { get; private set; }
        public DateTime Timestamp { get; private set; }
        private Transaction() { }
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

    class TransactionServiсe()
    {
        public Transaction ExecuteTransaction(Wallet fromWallet, Wallet toWallet, Money amount)
        {
            if (toWallet == fromWallet)
                throw new DomainException("Отримувач та відправник не можуть бути однаковими.");
            if(amount.Amount <= 0)
                throw new DomainException("Сума транзакції має бути більшою за нуль.");
            if(amount.Currency != fromWallet.Balance.Currency || amount.Currency != toWallet.Balance.Currency)
                throw new DomainException("Валюта транзакції повинна відповідати валюті гаманців.");
            fromWallet.WithdrawFunds(amount);
            toWallet.AddFunds(amount);
            return new Transaction(fromWallet.Id, toWallet.Id, amount);
        }
    }
    // --- EXCEPTIONS ---

    public class DomainException : Exception
        {
            public DomainException(string message) : base(message) { }
        } 
        
}