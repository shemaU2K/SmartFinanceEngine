using System;
using Xunit;
using SFE.Domain.Entities;
using SFE.Domain.ValueObjects;
using SFE.Domain.Services;
using SFE.Domain.Exeptions;

namespace SFE.Tests.Domain.Services
{
    public class TransactionServiceTests
    {
        [Fact]
        public void ExecuteTransaction_ValidTransfer_ShouldUpdateBalancesAndReturnTransaction()
        {
            // Arrange
            var service = new TransactionServiсe();

            var fromWallet = new Wallet("Sender", Guid.NewGuid(), Currency.USD);
            fromWallet.AddFunds(new Money(500m, Currency.USD));

            var toWallet = new Wallet("Receiver", Guid.NewGuid(), Currency.USD);
            var transferAmount = new Money(150m, Currency.USD);

            // Act
            var transaction = service.ExecuteTransaction(fromWallet, toWallet, transferAmount);

            // Assert
            Assert.NotNull(transaction);
            Assert.Equal(350m, fromWallet.Balance.Amount);
            Assert.Equal(150m, toWallet.Balance.Amount);
            Assert.Equal(transferAmount, transaction.Amount);
            Assert.Equal(fromWallet.Id, transaction.fromWalletId);
            Assert.Equal(toWallet.Id, transaction.toWalletId);
        }

        [Fact]
        public void ExecuteTransaction_SameWallet_ShouldThrowDomainException()
        {
            // Arrange
            var service = new TransactionServiсe();
            var wallet = new Wallet("My Wallet", Guid.NewGuid(), Currency.UAH);
            var transferAmount = new Money(100m, Currency.UAH);

            // Act & Assert
            var exception = Assert.Throws<DomainException>((() => service.ExecuteTransaction(wallet, wallet, transferAmount)));
            Assert.Equal("Отримувач та відправник не можуть бути однаковими.", exception.Message);
        }
    }
}