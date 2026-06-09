using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SFE.Application.Analytics
{
    public class AnalyticsTransaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public decimal Amount { get; set; }
        public decimal TaxRate { get; set; }
    }

    public class FinancialAnalyticsService
    {
        private readonly IReadOnlyList<AnalyticsTransaction> _transactions;
        public FinancialAnalyticsService(IEnumerable<AnalyticsTransaction> transactions)
        {
            if (transactions == null) throw new ArgumentNullException(nameof(transactions));
            _transactions = transactions.ToList();
        }

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

        public (decimal TotalTax, long ElapsedMilliseconds) CalculateTaxInParallel()
        {
            var sw = Stopwatch.StartNew();

            decimal totalTax = _transactions.AsParallel()
                .Sum(t => CalculateComplexTax(t.Amount, t.TaxRate));

            sw.Stop();
            return (totalTax, sw.ElapsedMilliseconds);
        }

        public static decimal CalculateComplexTax(decimal amount, decimal rate)
        {
            double temp = Math.Sqrt((double)amount) * Math.Pow((double)rate, 1.5);
            for (int i = 0; i < 10; i++) { temp = Math.Sin(temp) + Math.Cos(temp) + 2; }
            return (decimal)temp;
        }
    }
}