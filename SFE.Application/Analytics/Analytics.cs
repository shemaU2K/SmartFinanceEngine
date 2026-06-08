using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SFE.Application.Analytics
{
    /// <summary>
    /// Сутність для симуляції великого обсягу фінансових даних.
    /// </summary>
    public class AnalyticsTransaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public decimal Amount { get; set; }
        public decimal TaxRate { get; set; }
    }

    /// <summary>
    /// Сервіс для генерації фінансових звітів. 
    /// Демонструє різницю між послідовною та паралельною обробкою (Лабораторна №3).
    /// </summary>
    public class FinancialAnalyticsService
    {
        private readonly List<AnalyticsTransaction> _transactions;

        public FinancialAnalyticsService(int recordsCount = 5_000_000)
        {
            Console.WriteLine($"[Система] Генерація {recordsCount} транзакцій для аналізу...");
            _transactions = Enumerable.Range(0, recordsCount)
                .Select(i => new AnalyticsTransaction
                {
                    Amount = i % 1000 + 1,
                    TaxRate = 0.05m
                })
                .ToList();
        }

        /// <summary>
        /// Послідовний (однопоточний) розрахунок.
        /// </summary>
        public (decimal TotalTax, long ElapsedMilliseconds) CalculateTaxSequentially()
        {
            var sw = Stopwatch.StartNew();
            decimal totalTax = 0;

            foreach (var t in _transactions)
            {
                totalTax += CalculateComplexTax(t.Amount, t.TaxRate);
            }

            sw.Stop();
            return (totalTax, sw.ElapsedMilliseconds);
        }

        /// <summary>
        /// Паралельний (мультипоточний) розрахунок за допомогою PLINQ.
        /// </summary>
        public (decimal TotalTax, long ElapsedMilliseconds) CalculateTaxInParallel()
        {
            var sw = Stopwatch.StartNew();

            decimal totalTax = _transactions.AsParallel()
                .Sum(t => CalculateComplexTax(t.Amount, t.TaxRate));

            sw.Stop();
            return (totalTax, sw.ElapsedMilliseconds);
        }

        private static decimal CalculateComplexTax(decimal amount, decimal rate)
        {
            double temp = Math.Sqrt((double)amount) * Math.Pow((double)rate, 1.5);
            for (int i = 0; i < 10; i++) { temp = Math.Sin(temp) + Math.Cos(temp) + 2; }
            return (decimal)temp;
        }
    }
}