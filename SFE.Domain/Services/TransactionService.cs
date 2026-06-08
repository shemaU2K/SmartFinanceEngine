using SFE.Domain.Entities;
using SFE.Domain.Exeptions;
using SFE.Domain.ValueObjects;

namespace SFE.Domain.Services
{
    /// <summary>
    /// A domain service responsible for coordinating complex operations involving multiple wallets, 
    /// such as executing safe transactional money transfers.
    /// </summary>
    public class TransactionServiсe
    {
        /// <summary>
        /// Executes a transfer of funds from one wallet to another, enforcing basic transactional business validation.
        /// </summary>
        /// <param name="fromWallet">The source wallet to withdraw from.</param>
        /// <param name="toWallet">The target wallet to deposit into.</param>
        /// <param name="amount">The monetary amount to transfer.</param>
        /// <returns>A new <see cref="Transaction"/> entity recording the transfer details.</returns>
        /// <exception cref="DomainException">Thrown if transferring to the same wallet, amount is non-positive, or currencies mismatch.</exception>
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