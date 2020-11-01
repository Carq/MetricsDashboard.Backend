using System;
using TilesDashboard.Core.Type.ValueObjects;

namespace TilesDashboard.Core.Type.TileData.Metric
{
    public class MetricData : ITileData
    {
        private readonly DecimalValue _decimalValue;

        protected MetricData(DecimalValue value, DateTimeOffset addedOn)
        {
            _decimalValue = value;
            AddedOn = addedOn;
        }

        public DateTimeOffset AddedOn { get; }

        public static MetricData Percentage(decimal percentage, DateTimeOffset addedOn) => new MetricData(new Percentage(percentage), addedOn);

        public static MetricData Money(decimal money, DateTimeOffset addedOn) => new MetricData(DecimalValue.TwoDecimalPlaces(money), addedOn);

        public static MetricData Time(decimal timeInSecs, DateTimeOffset addedOn) => new MetricData(DecimalValue.FourDecimalPlaces(timeInSecs), addedOn);

        public ValueObject GetData() => _decimalValue;
    }
}
