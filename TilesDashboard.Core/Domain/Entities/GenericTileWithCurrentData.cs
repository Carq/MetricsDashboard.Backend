using System.Collections.Generic;
using Dawn;
using TilesDashboard.Core.Storage.Entities;
using TilesDashboard.Core.Type.Enums;
using TilesDashboard.Core.Type.TileData;
using TilesDashboard.Handy.Extensions;

namespace TilesDashboard.Core.Domain.Entities
{
    public class GenericTileWithCurrentData
    {
        public GenericTileWithCurrentData(string name, TileType type, IList<ITileData> data, Group group, object configuration = null)
        {
            Name = Guard.Argument(name, nameof(name)).NotNull().NotEmpty();
            Type = Guard.Argument(type, nameof(type)).NotDefault();
            Data.AddRange(data);
            Group = group;
            Configuration = configuration;
        }

        public string Name { get; private set; }

        public TileType Type { get; private set; }

        public object Configuration { get; private set; }

        public Group Group { get; private set; }

        public IList<ITileData> Data { get; private set; } = new List<ITileData>();
    }
}
