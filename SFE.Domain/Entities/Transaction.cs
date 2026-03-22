using System;
using SFE.Domain.Exeptions;
using System.Collections.Generic;
using System.Text;
using SFE.Domain.ValueObjects;

namespace SFE.Domain.Entities
{
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
}
