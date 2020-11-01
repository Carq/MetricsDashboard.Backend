using System.Collections.Generic;
using System.Linq;
using TilesDashboard.Contract;
using TilesDashboard.Core.Domain.Entities;
using TilesDashboard.Core.Type.Enums;
using TilesDashboard.Core.Type.TileData;
using TilesDashboard.Core.Type.TileData.Metric;
using TilesDashboard.Handy.Extensions;

namespace TilesDashboard.WebApi.Mappers
{
    public static class TileDtoMapper
    {
        public static IList<TileWithCurrentDataDto> Map(IList<GenericTileWithCurrentData> list)
        {
            var result = new List<TileWithCurrentDataDto>();
            foreach (var item in list)
            {
                var tileWithDataDto = new TileWithCurrentDataDto
                {
                    Name = item.Name,
                    Type = item.Type,
                    Configuration = item.Configuration,
                    Group = item.Group != null ? new GroupDto(item.Group.Name, item.Group.Order) : null,
                };

                tileWithDataDto.Data.AddRange(MapRecentData(item.Data));
                result.Add(tileWithDataDto);
            }

            return result;
        }

        public static IList<object> MapRecentData(IList<MetricData> recentData)
        {
            return recentData.Select(MapTileData).ToList();
        }

        public static IList<object> MapRecentData(IList<IntegerData> recentData)
        {
            return recentData.Select(MapTileData).ToList();
        }

        public static IList<object> MapRecentData(IList<WeatherData> recentData)
        {
            return recentData.Select(MapTileData).ToList();
        }

        public static IList<object> MapRecentData(IList<HeartBeatData> recentData)
        {
            return recentData.Select(MapTileData).ToList();
        }

        public static IList<object> MapRecentData(IList<ITileData> recentData)
        {
            return recentData.Select(MapTileData).ToList();
        }

        private static object MapTileData(ITileData tileData)
        {
            return tileData.GetData();
        }
    }
}
