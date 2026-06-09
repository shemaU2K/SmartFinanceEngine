using System;
using Xunit;
using SFE.Domain.ValueObjects;

namespace SFE.Tests.Domain.ValueObjects
{
    public class MoneyTests
    {
        [Fact]
        public void OperatorAdd_SameCurrency_ShouldReturnSum()
        {
            // Arrange
            var moneyA = new Money(100m, Currency.USD);
            var moneyB = new Money(50m, Currency.USD);

            // Act
            var result = moneyA + moneyB;

            // Assert
            Assert.Equal(150m, result.Amount);
            Assert.Equal(Currency.USD, result.Currency);
        }

        [Fact]
        public void OperatorAdd_DifferentCurrency_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var moneyA = new Money(100m, Currency.USD);
            var moneyB = new Money(50m, Currency.EUR);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => moneyA + moneyB);
            Assert.Equal("Неможливо додати різні валюти без конвертації.", exception.Message);
        }

        [Fact]
        public void OperatorSubtract_SameCurrency_ShouldReturnDifference()
        {
            // Arrange
            var moneyA = new Money(100m, Currency.UAH);
            var moneyB = new Money(30m, Currency.UAH);

            // Act
            var result = moneyA - moneyB;

            // Assert
            Assert.Equal(70m, result.Amount);
            Assert.Equal(Currency.UAH, result.Currency);
        }
    }
}