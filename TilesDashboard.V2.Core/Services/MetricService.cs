﻿using System;
using System.Threading.Tasks;
using TilesDashboard.Handy.Tools;
using TilesDashboard.V2.Core.Entities;
using TilesDashboard.V2.Core.Entities.Enums;
using TilesDashboard.V2.Core.Entities.Metric;
using TilesDashboard.V2.Core.Repositories;

namespace TilesDashboard.V2.Core.Services
{
    public class MetricService : TileBaseService, IMetricService
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public MetricService(IDateTimeProvider dateTimeProvider, ITileRepository tileRepository)
            : base(tileRepository)
        {
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public async Task<MetricTile> GetMetricTile(TileId tileId) => await GetTile<MetricTile>(tileId);

        public async Task RecordValue(TileId tileId, MetricType metricType, decimal newValue)
        {
            var metric = await GetTile<MetricTile>(tileId);

            MetricValue newMetricValue;
            switch (metric.MetricType)
            {
                case MetricType.Percentage:
                    newMetricValue = new PercentageMetricValue(newValue, _dateTimeProvider.Now);
                    break;
                case MetricType.Money:
                    newMetricValue = new MoneyMetricValue(newValue, _dateTimeProvider.Now);
                    break;
                case MetricType.Time:
                    newMetricValue = new TimeMetricValue(newValue, _dateTimeProvider.Now);
                    break;
                default:
                    throw new NotSupportedException();
            }

            await TileRepository.RecordValue(tileId, newMetricValue);
        }
    }
}
