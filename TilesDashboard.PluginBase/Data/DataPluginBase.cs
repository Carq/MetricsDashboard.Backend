﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TilesDashboard.V2.Core.Entities.Enums;

namespace TilesDashboard.PluginBase.Data
{
    public abstract class DataPluginBase<TResult> : IDataPlugin
        where TResult : PluginDataResult
    {
        /// <summary>
        /// Unique Plugin Name which is used to get plugin config from storage.
        /// </summary>
        public abstract string UniquePluginName { get; }

        /// <summary>
        /// Tile type.
        /// </summary>
        public abstract TileType TileType { get; }

        public abstract Task<TResult> GetTileValueAsync(IDictionary<string, string> pluginConfiguration, CancellationToken cancellation = default);
    }
}
