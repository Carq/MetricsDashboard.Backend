using System;
using System.Threading;
using System.Threading.Tasks;
using TilesDashboard.Core.Type;

namespace TilesDashboard.PluginBase.Notification
{
    public abstract class NotificationPluginBase : INotificationPlugin
    {
        public NotificationPluginBase(IPluginConfigProvider configProvider)
        {
            ConfigProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
        }

        /// <inheritdoc/>
        public abstract TileId TileId { get; }

         /// <summary>
        /// Give access to config entries. Config Provider will be injected by PluginSystem.
        /// </summary>
        public IPluginConfigProvider ConfigProvider { get; }

        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public abstract Task PerformNotificationAsync(object newData, CancellationToken cancellationToken);
    }
}
