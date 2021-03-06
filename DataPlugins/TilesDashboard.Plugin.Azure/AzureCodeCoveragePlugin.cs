﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TilesDashboard.PluginBase.Data;
using TilesDashboard.PluginBase.Data.MetricPlugin;
using TilesDashboard.V2.Core.Entities.Metric;

namespace TilesDashboard.Plugin.Azure
{
    public class AzureCodeCoveragePlugin : MetricDataPlugin
    {
        public override string UniquePluginName => $"TileCorePlugins.{nameof(AzureCodeCoveragePlugin)}";

        public override async Task<MetricData> GetTileValueAsync(IDictionary<string, string> pluginConfiguration, CancellationToken cancellationToken = default)
        {
            try
            {
                var azureDevOpsHelper = new AzureDevOpsHelper(
                                                pluginConfiguration["Organization"],
                                                pluginConfiguration["Project"],
                                                pluginConfiguration["BuildDefinition"],
                                                pluginConfiguration["PersonalAccessToken"]);

                var codeCoverageDetails = await azureDevOpsHelper.GetCodeCoverageResultForLastGreenBuildAsync(cancellationToken);
                if (codeCoverageDetails == null || codeCoverageDetails.CoverageData.Count == 0)
                {
                    return MetricData.NoUpdate();
                }

                var linesCoverageData = codeCoverageDetails.CoverageData.First().CoverageStats.Single(x => x.Label == "Lines");
                var percentageCoverage = Math.Round(100m * linesCoverageData.Covered / linesCoverageData.Total, 1);

                return new MetricData(percentageCoverage, MetricType.Percentage, Status.OK);
            }
            catch (Exception ex)
            {
                return MetricData.Error(ex.Message);
            }
        }
    }
}
