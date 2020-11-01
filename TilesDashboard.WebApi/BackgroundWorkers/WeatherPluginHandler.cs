using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TilesDashboard.Core.Domain.Services;
using TilesDashboard.Core.Domain.ValueObjects;
using TilesDashboard.Core.Type.ValueObjects;
using TilesDashboard.Handy.Extensions;
using TilesDashboard.PluginBase;
using TilesDashboard.PluginBase.Data;
using TilesDashboard.PluginBase.Data.WeatherPlugin;

namespace TilesDashboard.WebApi.BackgroundWorkers
{
    public class WeatherPluginHandler
    {
        private readonly ILogger<WeatherPluginHandler> _logger;

        private readonly IWeatherServices _weatherServices;

        public WeatherPluginHandler(ILogger<WeatherPluginHandler> logger, IWeatherServices weatherServices)
        {
            _logger = logger;
            _weatherServices = weatherServices;
        }

        public async Task<Result> HandlePlugin(WeatherPluginBase weatherPlugin, CancellationToken stoppingToken)
        {
            var data = await weatherPlugin.GetDataAsync();
            _logger.LogDebug($"Weather plugin: \"{weatherPlugin.TileName}\", Temperature: {data.Temperature}, Huminidy: {data.Huminidy}%");
            if (data.Status.Is(Status.OK))
            {
                await _weatherServices.RecordWeatherDataAsync(weatherPlugin.TileName, new Temperature(data.Temperature), data.Huminidy.HasValue ? new Percentage(data.Huminidy.Value) : null, null, stoppingToken);
            }

            return data;
        }
    }
}
