using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TilesDashboard.Contract.RecordData;
using TilesDashboard.Core.Domain.Services;
using TilesDashboard.Core.Domain.ValueObjects;
using TilesDashboard.Core.Type.ValueObjects;
using TilesDashboard.WebApi.Authorization;
using TilesDashboard.WebApi.Mappers;

namespace TilesDashboard.WebApi.Controllers
{
    [Route("tiles/weather")]
    [ApiController]
    public class WeatherTilesController : ControllerBase
    {
        private const int AmountOfDate = 5;

        private readonly IWeatherServices _weatherService;

        public WeatherTilesController(IWeatherServices weatherServices)
        {
            _weatherService = weatherServices ?? throw new ArgumentNullException(nameof(weatherServices));
        }

        [HttpGet("{tileName}/recent")]
        [BearerReadAuthorization]
        public async Task<IList<object>> GetWeatherRecentData(string tileName, CancellationToken cancellationToken)
        {
            return TileDtoMapper.MapRecentData(await _weatherService.GetWeatherRecentDataAsync(tileName, AmountOfDate, cancellationToken));
        }

        [HttpGet("{tileName}/since")]
        [BearerReadAuthorization]
        public async Task<IList<object>> GetWeatherDataSince(string tileName, [FromQuery][Required][Range(1, 24)] int hours, CancellationToken cancellationToken)
        {
            return TileDtoMapper.MapRecentData(await _weatherService.GetWeatherDataSinceAsync(tileName, hours, cancellationToken));
        }

        [HttpPost("{tileName}/record")]
        [BearerAuthorization]
        public async Task RecordWeatherData(string tileName, [FromBody]RecordWeatherDataDto weatherDataDto, CancellationToken cancellationToken)
        {
            await _weatherService.RecordWeatherDataAsync(tileName, new Temperature(weatherDataDto.Temperature), new Percentage(weatherDataDto.Humidity), null, cancellationToken);
        }

        [HttpDelete("{tileName}/removeFakeData")]
        [BearerAuthorization]
        public async Task RemoveWeatherFakeData(string tileName, CancellationToken cancellationToken)
        {
            await _weatherService.RemoveFakeDataAsync(tileName, cancellationToken);
        }
    }
}