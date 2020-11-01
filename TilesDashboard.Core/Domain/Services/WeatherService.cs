using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TilesDashboard.Contract.Events;
using TilesDashboard.Core.Domain.Entities;
using TilesDashboard.Core.Domain.Repositories;
using TilesDashboard.Core.Domain.ValueObjects;
using TilesDashboard.Core.Storage;
using TilesDashboard.Core.Type;
using TilesDashboard.Core.Type.Enums;
using TilesDashboard.Core.Type.ValueObjects;
using TilesDashboard.Handy.Events;
using TilesDashboard.Handy.Tools;

namespace TilesDashboard.Core.Domain.Services
{
    public class WeatherService : TileService, IWeatherServices
    {
        private readonly IWeatherRepository _weatherRepository;

        private readonly IEventDispatcher _eventDispatcher;

        public WeatherService(ITileContext context, ITilesRepository tilesRepository, IWeatherRepository weatherRepository, IDateTimeOffsetProvider dateTimeOffsetProvider, IEventDispatcher eventDispatcher)
            : base(context, tilesRepository, dateTimeOffsetProvider)
        {
            _weatherRepository = weatherRepository;
            _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
        }

        public async Task<IList<WeatherData>> GetWeatherRecentDataAsync(string tileName, int amountOfData, CancellationToken token)
        {
            return await GetRecentDataAsync<WeatherData>(tileName, TileType.Weather, amountOfData, token);
        }

        public async Task<IList<WeatherData>> GetWeatherDataSinceAsync(string tileName, int since, CancellationToken token)
        {
            return await GetDataSinceAsync<WeatherData>(tileName, TileType.Weather, DateTimeOffsetProvider.Now.AddHours(-since), token);
        }

        public async Task RecordWeatherDataAsync(string tileName, Temperature temperature, Percentage humidity, DateTimeOffset? dateOfChange, CancellationToken token)
        {
            var weatherData = new WeatherData(temperature, humidity, dateOfChange ?? DateTimeOffsetProvider.Now);
            if (await _weatherRepository.AddDataAsync(tileName, weatherData, token))
            {
                await _eventDispatcher.PublishAsync(new NewDataEvent(new TileId(tileName, TileType.Weather), new { Temperature = weatherData.Temperature.GetRoundedValue(), Humidity = weatherData.Humidity.GetRoundedValue(), weatherData.AddedOn }), token);
            }
        }

        public async Task RemoveFakeDataAsync(string tileName, CancellationToken cancellationToken)
        {
            await _weatherRepository.RemoveWeatherDataAsync(tileName, Temperature.Zero, Percentage.Zero, cancellationToken);
        }
    }
}
