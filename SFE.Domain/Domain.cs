using System;
using System.Collections.Generic;

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

    // --- EXCEPTIONS ---

    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
    }
}