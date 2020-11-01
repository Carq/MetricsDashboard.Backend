using System;

namespace TilesDashboard.Core.Type.TileData
{
    public interface ITileData
    {
        DateTimeOffset AddedOn { get; }

        ValueObject GetData();
    }
}
