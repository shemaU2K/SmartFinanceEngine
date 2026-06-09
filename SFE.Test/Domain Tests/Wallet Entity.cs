using System;
using Xunit;
using SFE.Domain.Entities;
using SFE.Domain.ValueObjects;
using SFE.Domain.Exeptions;

namespace SFE.Tests.Domain.Entities
{
    public class WalletTests
    {
        [Fact]
        public void AddFunds_ValidAmount_ShouldIncreaseBalance()
        {
            // Arrange
            var wallet = new Wallet("My Wallet", Guid.NewGuid(), Currency.EUR);
            var amountToAdd = new Money(500m, Currency.EUR);

            // Act
            wallet.AddFunds(amountToAdd);

            // Assert
            Assert.Equal(500m, wallet.Balance.Amount);
        }

        [Fact]
        public void AddFunds_NegativeAmount_ShouldThrowDomainException()
        {
            // Arrange
            var wallet = new Wallet("My Wallet", Guid.NewGuid(), Currency.EUR);
            var invalidAmount = new Money(-50m, Currency.EUR);

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => wallet.AddFunds(invalidAmount));
            Assert.Equal("Сума поповнення має бути більшою за нуль.", exception.Message);
        }

        [Fact]
        public void WithdrawFunds_WithSufficientFunds_ShouldDecreaseBalance()
        {
            // Arrange
            var wallet = new Wallet("My Wallet", Guid.NewGuid(), Currency.USD);
            wallet.AddFunds(new Money(200m, Currency.USD));
            var amountToWithdraw = new Money(50m, Currency.USD);

            // Act
            wallet.WithdrawFunds(amountToWithdraw);

            // Assert
            Assert.Equal(150m, wallet.Balance.Amount);
        }

        [Fact]
        public void WithdrawFunds_WithInsufficientFunds_ShouldThrowDomainException()
        {
            // Arrange
            var wallet = new Wallet("My Wallet", Guid.NewGuid(), Currency.USD);
            wallet.AddFunds(new Money(50m, Currency.USD)); // Only 50 USD available
            var amountToWithdraw = new Money(100m, Currency.USD); // Try to withdraw 100 USD

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => wallet.WithdrawFunds(amountToWithdraw));
            Assert.Equal("Недостатньо коштів на балансі.", exception.Message);
        }
    }
}