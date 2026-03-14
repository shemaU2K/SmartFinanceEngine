using System;
using System.Collections.Generic;
using System.Text;

namespace SFE.Domain.ValueObjects
{
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
}
