using System.Threading;
using System.Threading.Tasks;
using TilesDashboard.Core.Domain.Entities;
using TilesDashboard.Core.Domain.ValueObjects;
using TilesDashboard.Core.Type.ValueObjects;

namespace TilesDashboard.Core.Domain.Repositories
{
    public interface IWeatherRepository
    {
        Task<bool> AddDataAsync(string tileName, WeatherData weatherData, CancellationToken cancellationToken);

        Task RemoveWeatherDataAsync(string tileName, Temperature temperature, Percentage humidity, CancellationToken cancellationToken);
    }
}
