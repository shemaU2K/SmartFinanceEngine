using SFE.Domain.Entities;
using SFE.Domain.Exeptions;
using System;
using System.Collections.Generic;
using System.Text;
using SFE.Domain.ValueObjects;

namespace SFE.Domain.Services
{
    public class TransactionServiсe()
    {
        public Transaction ExecuteTransaction(Wallet fromWallet, Wallet toWallet, Money amount)
    {
        if (toWallet == fromWallet)
            throw new DomainException("Отримувач та відправник не можуть бути однаковими.");
        if (amount.Amount <= 0)
            throw new DomainException("Сума транзакції має бути більшою за нуль.");
        if (amount.Currency != fromWallet.Balance.Currency || amount.Currency != toWallet.Balance.Currency)
            throw new DomainException("Валюта транзакції повинна відповідати валюті гаманців.");
        fromWallet.WithdrawFunds(amount);
        toWallet.AddFunds(amount);
        return new Transaction(fromWallet.Id, toWallet.Id, amount);
    }
}
}
