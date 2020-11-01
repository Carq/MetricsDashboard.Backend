using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TilesDashboard.Core.Type;
using TilesDashboard.Core.Type.TileData.Metric;
using TilesDashboard.PluginBase;
using TilesDashboard.PluginBase.Notification;

namespace TileDashboard.Plugin.CodeCoverageSlackNotification
{
    public class CodeCoverageSlackNotificationPlugin : NotificationPluginBase
    {
        private string SlackHook => ConfigProvider.GetConfigEntry($"{RootConfig}:SlackHook");

        private readonly string RootConfig = "CodeCoverageNotification";

        public CodeCoverageSlackNotificationPlugin(IPluginConfigProvider pluginConfigProvider)
            : base(pluginConfigProvider)
        {
        }

        public override TileId TileId { get; } = new TileId("BE Unit Test Coverage", TilesDashboard.Core.Type.Enums.TileType.Metric);

        public override async Task PerformNotificationAsync(object newData, CancellationToken cancellationToken)
        {
            var converted = newData as MetricData;
            if (converted != null)
            {
                await SendMessage($"New value for {TileId}: {converted}%", cancellationToken); 
            }
        }

        private async Task SendMessage(string message, CancellationToken cancellationToken)
        {
            using var httpClient = new HttpClient();
            var requestContent = JsonSerializer.Serialize(new { text = message });
            using var request = new HttpRequestMessage(HttpMethod.Post, SlackHook)
            {
                Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
            };

            await httpClient.SendAsync(request, cancellationToken);
        }
    }
}
