using System;
using Dawn;
using TilesDashboard.Core.Domain.ValueObjects;
using TilesDashboard.Core.Type;
using TilesDashboard.Core.Type.TileData;
using TilesDashboard.Core.Type.ValueObjects;

namespace TilesDashboard.Core.Domain.Entities
{
    public class WeatherData : ITileData
    {
        public WeatherData(Temperature temperature, Percentage humidity, DateTimeOffset addedOn)
        {
            Temperature = Guard.Argument(temperature, nameof(temperature)).NotNull();
            Humidity = humidity;
            AddedOn = addedOn;
        }

        public Temperature Temperature { get; private set; }

        public Percentage Humidity { get; private set; }

        public DateTimeOffset AddedOn { get; }

        public ValueObject GetData()
        {
            throw new NotImplementedException();
        }
    }
}
