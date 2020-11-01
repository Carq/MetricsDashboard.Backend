using System;
using TilesDashboard.Core.Type.ValueObjects;

namespace TilesDashboard.Core.Domain.ValueObjects
{
    public class Temperature : DecimalValue
    {
        public Temperature(decimal value)
        {
            Value = Math.Round(value, 1);
        }

        public static Temperature Zero => new Temperature(0);

        public decimal GetRoundedValue()
        {
            return Math.Round(Value, 1);
        }
    }
}
