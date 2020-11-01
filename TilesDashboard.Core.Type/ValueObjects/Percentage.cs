using System;

namespace TilesDashboard.Core.Type.ValueObjects
{
    public class Percentage : DecimalValue
    {
        public const decimal Min = 0;

        public const decimal Max = 100;

        public Percentage(decimal value)
        {
            if (value < Min || value > Max)
            {
                throw new ArgumentException($"Percentage value has to be in range of 1-100. Given value {value}.");
            }

            Value = Math.Round(value, 2);
        }

        public static Percentage Zero => new Percentage(0);

        public decimal GetRoundedValue()
        {
            return Math.Round(Value, 0);
        }
    }
}
