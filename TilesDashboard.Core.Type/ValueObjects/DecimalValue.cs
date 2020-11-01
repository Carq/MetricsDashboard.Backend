using System;
using System.Collections.Generic;

namespace TilesDashboard.Core.Type.ValueObjects
{
    public class DecimalValue : ValueObject
    {
        protected DecimalValue()
        {
        }

        public decimal Value { get; protected set; }

        public static DecimalValue TwoDecimalPlaces(decimal value) => new DecimalValue() { Value = Math.Round(value, 2) };

        public static DecimalValue FourDecimalPlaces(decimal value) => new DecimalValue() { Value = Math.Round(value, 2) };

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
