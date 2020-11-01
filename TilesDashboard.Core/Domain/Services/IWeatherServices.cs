using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TilesDashboard.Core.Domain.Entities;
using TilesDashboard.Core.Domain.ValueObjects;
using TilesDashboard.Core.Type.ValueObjects;

namespace TilesDashboard.Core.Domain.Services
{
    public interface IWeatherServices
    {
        Task<IList<WeatherData>> GetWeatherRecentDataAsync(string tileName, int amountOfData, CancellationToken token);

        Task<IList<WeatherData>> GetWeatherDataSinceAsync(string tileName, int since, CancellationToken token);

        Task RecordWeatherDataAsync(string tileName, Temperature temperature, Percentage humidity, DateTimeOffset? dateOfChange, CancellationToken token);

        Task RemoveFakeDataAsync(string tileName, CancellationToken cancellationToken);
    }
}
