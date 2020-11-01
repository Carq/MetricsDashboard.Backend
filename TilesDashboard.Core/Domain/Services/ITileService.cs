using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TilesDashboard.Core.Domain.Entities;
using TilesDashboard.Core.Type.Enums;
using TilesDashboard.Core.Type.TileData;

namespace TilesDashboard.Core.Domain.Services
{
    public interface ITileService
    {
        Task<IList<GenericTileWithCurrentData>> GetAllAsync(int amountOfData, CancellationToken cancellationToken);

        Task SetGroupToTile(string tileName, TileType tileType, string groupName, CancellationToken cancellationToken);

        Task<IList<TData>> GetRecentDataAsync<TData>(string tileName, TileType type, int amountOfData, CancellationToken cancellationToken)
            where TData : ITileData;

        Task<IList<TData>> GetDataSinceAsync<TData>(string tileName, TileType type, DateTimeOffset sinceDate, CancellationToken cancellationToken)
            where TData : ITileData;
    }
}
