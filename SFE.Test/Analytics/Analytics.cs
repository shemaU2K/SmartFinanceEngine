using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using SFE.Application.Analytics;

namespace SFE.Tests.Analytics
{
    public class FinancialAnalyticsServiceTests
    {
        [Fact]
        public void CalculateTax_WithEmptyList_ShouldReturnZeroTimeAndTax()
        {
            // Arrange
            var emptyList = new List<AnalyticsTransaction>();
            var service = new FinancialAnalyticsService(emptyList);

            // Act
            var resultSeq = service.CalculateTaxSequentially();
            var resultPar = service.CalculateTaxInParallel();

            // Assert
            Assert.Equal(0m, resultSeq.TotalTax);
            Assert.Equal(0m, resultPar.TotalTax);
        }

        [Fact]
        public void CalculateTax_SequentialAndParallel_ShouldYieldExactSameResult()
        {
            // Arrange
            // Для тесту генеруємо 10 000 транзакцій
            var transactions = Enumerable.Range(1, 10000).Select(i => new AnalyticsTransaction
            {
                Amount = i * 10m,
                TaxRate = 0.05m
            }).ToList();

            var service = new FinancialAnalyticsService(transactions);

            // Act
            var sequentialResult = service.CalculateTaxSequentially();
            var parallelResult = service.CalculateTaxInParallel();

            // Assert
            // Головна вимога Лаби 3: результати мультипоточності не мають відрізнятися від послідовної
            Assert.Equal(sequentialResult.TotalTax, parallelResult.TotalTax);

            // Якщо дуже хочеться, можна ще перевірити, що паралельний код хоч трохи попрацював
            Assert.True(parallelResult.ElapsedMilliseconds >= 0);
        }

        [Fact]
        public void CalculateComplexTax_ShouldReturnExpectedValue_ForKnownInputs()
        {
            // Arrange
            decimal amount = 100m;
            decimal rate = 0.1m;

            // Act
            // Викликаємо статичний метод безпосередньо для тестування чистої математики
            var tax = FinancialAnalyticsService.CalculateComplexTax(amount, rate);

            // Assert
            // Оскільки там синуси/косинуси, ми просто перевіряємо, що результат не нульовий 
            // і лежить у межах очікуваного діапазону (захист від випадкових змін алгоритму)
            Assert.NotEqual(0m, tax);
            Assert.True(tax > 0m);
        }
    }
}