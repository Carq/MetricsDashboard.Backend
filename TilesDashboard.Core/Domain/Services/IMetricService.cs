using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TilesDashboard.Core.Type.Enums;
using TilesDashboard.Core.Type.TileData.Metric;

namespace TilesDashboard.Core.Domain.Services
{
    public interface IMetricService
    {
        Task RecordMetricDataAsync(string tileName, MetricType metricType, decimal currentValue, CancellationToken cancellationToken);

        Task<IList<MetricData>> GetMetricDataSinceAsync(string tileName, int sinceDays, CancellationToken token);

        Task<IList<MetricData>> GetMetricRecentDataAsync(string tileName, int amountOfData, CancellationToken token);
    }
}
