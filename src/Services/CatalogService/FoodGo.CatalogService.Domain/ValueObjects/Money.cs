using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Domain.ValueObjects
{
    public sealed class Money : IEquatable<Money>
    {
        public decimal Amount { get; }
        public string Currency { get; }

        private Money()
        {
            
        }

        public Money(decimal amount, string currency = "TRY")
        {
            if (amount < 0) throw new ArgumentException("Miktar negatif olamaz", nameof(amount));
            Amount = amount;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        }

        public Money Change(decimal newAmount) => new Money(newAmount, Currency);

        public bool Equals(Money? other) => other is not null && Amount == other.Amount && Currency == other.Currency;
        public override bool Equals(object? obj) => Equals(obj as Money);
        public override int GetHashCode() => HashCode.Combine(Amount, Currency);
        public override string ToString() => $"{Amount:0.00}{Currency}";

    }
}
