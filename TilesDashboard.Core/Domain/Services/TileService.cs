using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dawn;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using TilesDashboard.Core.Domain.Entities;
using TilesDashboard.Core.Domain.Extensions;
using TilesDashboard.Core.Domain.Repositories;
using TilesDashboard.Core.Storage;
using TilesDashboard.Core.Storage.Entities;
using TilesDashboard.Core.Type.Enums;
using TilesDashboard.Core.Type.TileData;
using TilesDashboard.Core.Type.TileData.Metric;
using TilesDashboard.Handy.Tools;

namespace TilesDashboard.Core.Domain.Services
{
    public class TileService : ITileService
    {
        public TileService(ITileContext context, ITilesRepository tilesRepository, IDateTimeOffsetProvider dateTimeOffsetProvider)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            TilesRepository = tilesRepository ?? throw new ArgumentNullException(nameof(tilesRepository));
            DateTimeOffsetProvider = dateTimeOffsetProvider ?? throw new ArgumentNullException(nameof(dateTimeOffsetProvider));
        }

        protected ITileContext Context { get; }

        protected IDateTimeOffsetProvider DateTimeOffsetProvider { get; }

        protected ITilesRepository TilesRepository { get; }

        public async Task<IList<GenericTileWithCurrentData>> GetAllAsync(int amountOfData, CancellationToken cancellationToken)
        {
            Guard.Argument(amountOfData, nameof(amountOfData)).GreaterThan(0);
            var allTiles = await TilesRepository.GetAllTilesWithLimitedRecentData(amountOfData, cancellationToken);

            var tilesWithCurrentData = new List<GenericTileWithCurrentData>();

            foreach (var tile in allTiles)
            {
                object configuration = null;
                var rawData = tile.Data.TakeLast(amountOfData);
                var data = new List<ITileData>();
                if (tile.Id.TileType == TileType.Metric)
                {
                    configuration = BsonSerializer.Deserialize<MetricConfiguration>(tile.Configuration);
                    data = DeserializeData<MetricData>(rawData).Cast<ITileData>().ToList();
                }
                else if (tile.Id.TileType == TileType.Weather)
                {
                    data = DeserializeData<WeatherData>(rawData).Cast<ITileData>().ToList();
                }
                else if (tile.Id.TileType == TileType.Integer)
                {
                    configuration = BsonSerializer.Deserialize<IntegerConfiguration>(tile.Configuration);
                    data = DeserializeData<IntegerData>(rawData).Cast<ITileData>().ToList();
                }
                else if (tile.Id.TileType == TileType.HeartBeat)
                {
                    configuration = BsonSerializer.Deserialize<HeartBeatConfiguration>(tile.Configuration);
                    data = DeserializeData<HeartBeatData>(rawData).Cast<ITileData>().ToList();
                }

                tilesWithCurrentData.Add(new GenericTileWithCurrentData(tile.Id.Name, tile.Id.TileType, data, new Group(tile.Group), configuration));
            }

            return tilesWithCurrentData;
        }

        public async Task SetGroupToTile(string tileName, TileType tileType, string groupName, CancellationToken cancellationToken)
        {
            await Context.GetTiles().UpdateOneAsync(
                TileDbEntityExtensions.TileDbFilter(tileName, tileType),
                Builders<TileDbEntity>.Update.Set(x => x.Group, groupName),
                null,
                cancellationToken);
        }

        public async Task<IList<TData>> GetRecentDataAsync<TData>(string tileName, TileType type, int amountOfData, CancellationToken cancellationToken)
            where TData : ITileData
        {
            var tileDbEntity = await TilesRepository.GetTileWithLimitedRecentData(tileName, type, amountOfData, cancellationToken);
            var rawData = tileDbEntity.Data.OrderBy(x => x[nameof(TileDataBase.AddedOn)]).TakeLast(amountOfData);
            return DeserializeData<TData>(rawData);
        }

        public async Task<IList<TData>> GetDataSinceAsync<TData>(string tileName, TileType type, DateTimeOffset sinceDate, CancellationToken cancellationToken)
            where TData : ITileData
        {
            var tileDbEntity = await TilesRepository.GetTileDataSince(tileName, type, sinceDate, cancellationToken);
            return DeserializeData<TData>(tileDbEntity?.Data ?? Array.Empty<BsonDocument>());
        }

        protected static List<TData> DeserializeData<TData>(IEnumerable<BsonDocument> rawData)
            where TData : ITileData
        {
            var result = new List<TData>();
            foreach (var item in rawData)
            {
                result.Add(BsonSerializer.Deserialize<TData>(item));
            }

            return result.OrderByDescending(x => x.AddedOn).ToList();
        }
    }
}
