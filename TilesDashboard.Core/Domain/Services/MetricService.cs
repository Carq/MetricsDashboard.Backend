using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using TilesDashboard.Contract.Events;
using TilesDashboard.Core.Domain.Entities;
using TilesDashboard.Core.Domain.Repositories;
using TilesDashboard.Core.Exceptions;
using TilesDashboard.Core.Storage;
using TilesDashboard.Core.Type;
using TilesDashboard.Core.Type.Enums;
using TilesDashboard.Core.Type.TileData.Metric;
using TilesDashboard.Handy.Events;
using TilesDashboard.Handy.Tools;

namespace TilesDashboard.Core.Domain.Services
{
    public class MetricService : TileService, IMetricService
    {
        private readonly IEventDispatcher _eventDispatcher;

        public MetricService(ITileContext context, ITilesRepository tilesRepository, IDateTimeOffsetProvider dateTimeOffsetProvider, IEventDispatcher eventDispatcher)
            : base(context, tilesRepository, dateTimeOffsetProvider)
        {
            _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
        }

        public async Task<IList<MetricData>> GetMetricRecentDataAsync(string tileName, int amountOfData, CancellationToken token)
        {
            return await GetRecentDataAsync<MetricData>(tileName, TileType.Metric, amountOfData, token);
        }

        public async Task<IList<MetricData>> GetMetricDataSinceAsync(string tileName, int sinceDays, CancellationToken token)
        {
            return await GetDataSinceAsync<MetricData>(tileName, TileType.Metric, DateTimeOffsetProvider.Now.AddDays(-sinceDays), token);
        }

        public async Task RecordMetricDataAsync(string tileName, MetricType metricType, decimal currentValue, CancellationToken cancellationToken)
        {
            MetricData metricData = metricType switch {
                MetricType.Percentage => MetricData.Percentage(currentValue, DateTimeOffsetProvider.Now),
                MetricType.Money => MetricData.Money(currentValue, DateTimeOffsetProvider.Now),
                MetricType.Time => MetricData.Time(currentValue, DateTimeOffsetProvider.Now),
                _ => throw new NotSupportedException()
            };

            var tile = await TilesRepository.GetTileWithoutData(tileName, TileType.Metric, cancellationToken);

            var metricConfiguration = BsonSerializer.Deserialize<MetricConfiguration>(tile.Configuration);
            if (metricConfiguration.MetricType != metricType)
            {
                throw new NotSupportedOperationException($"Metric {tileName} is type of {metricConfiguration.MetricType} but type {metricType} has been passed.");
            }

            await TilesRepository.InsertData(tileName, TileType.Metric, metricData.ToBsonDocument(), cancellationToken);
            await _eventDispatcher.PublishAsync(new NewDataEvent(new TileId(tileName, TileType.Metric), metricData), cancellationToken);
        }
    }
}
