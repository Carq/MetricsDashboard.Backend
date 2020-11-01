using MongoDB.Driver;
using TilesDashboard.Core.Storage.Entities;
using TilesDashboard.Core.Type.Enums;

namespace TilesDashboard.Core.Domain.Extensions
{
    public static class TileDbEntityExtensions
    {
        public static ProjectionDefinition<TileDbEntity> FetchRecentData(
            this ProjectionDefinitionBuilder<TileDbEntity> source, int amountOfData)
        {
            return source.Slice(x => x.Data, -amountOfData);
        }

        public static ProjectionDefinition<TileDbEntity> FetchGroup(
            this ProjectionDefinition<TileDbEntity> source)
        {
            return source.Include(x => x.Group);
        }

        public static ProjectionDefinition<TileDbEntity> FetchConfiguration(
            this ProjectionDefinition<TileDbEntity> source)
        {
            return source.Include(x => x.Configuration);
        }

        public static FilterDefinition<TileDbEntity> TileDbFilter(string tileName, TileType tileType)
        {
            return Builders<TileDbEntity>.Filter.And(
                Builders<TileDbEntity>.Filter.Eq(x => x.Id.Name, tileName),
                Builders<TileDbEntity>.Filter.Eq(x => x.Id.TileType, tileType));
        }
    }
}