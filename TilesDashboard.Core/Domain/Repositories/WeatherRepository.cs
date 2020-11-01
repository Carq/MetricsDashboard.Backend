using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using TilesDashboard.Core.Domain.Entities;
using TilesDashboard.Core.Domain.Extensions;
using TilesDashboard.Core.Domain.ValueObjects;
using TilesDashboard.Core.Storage;
using TilesDashboard.Core.Storage.Entities;
using TilesDashboard.Core.Type;
using TilesDashboard.Core.Type.Enums;
using TilesDashboard.Core.Type.ValueObjects;

namespace TilesDashboard.Core.Domain.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly ITileContext _context;

        public WeatherRepository(ITileContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> AddDataAsync(string tileName, WeatherData weatherData, CancellationToken cancellationToken)
        {
            var result = await _context.GetTiles().UpdateOneAsync(
                TileDbEntityExtensions.TileDbFilter(tileName, TileType.Weather),
                Builders<TileDbEntity>.Update.Push(x => x.Data, weatherData.ToBsonDocument()),
                null,
                cancellationToken);

            return result.ModifiedCount > 0;
        }

        public async Task RemoveWeatherDataAsync(string tileName, Temperature temperature, Percentage humidity, CancellationToken cancellationToken)
        {
            var pullFilter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq(y => y[$"{nameof(WeatherData.Temperature)}.Value"], temperature.Value),
                Builders<BsonDocument>.Filter.Eq(y => y[$"{nameof(WeatherData.Humidity)}.Value"], humidity.Value));

            await _context.GetTiles().UpdateOneAsync(
                TileDbEntityExtensions.TileDbFilter(tileName, TileType.Weather),
                Builders<TileDbEntity>.Update.PullFilter(x => x.Data, pullFilter),
                null,
                cancellationToken);
        }
    }
}
