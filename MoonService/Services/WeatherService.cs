using MoonService.Contracts;

namespace MoonService.Services
{
    public class WeatherService : IWeatherService
    {
        private static readonly Random random = new Random();

        private DateTime _recoveryTime = DateTime.UtcNow;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast()
        {
            if(_recoveryTime > DateTime.UtcNow)
            {
                throw new Exception("Something went wrong!!");
            }

            if(_recoveryTime < DateTime.UtcNow && random.Next(1, 4) == 1)
            {
                _recoveryTime = DateTime.UtcNow.AddSeconds(30);
            }

            return await Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray());
        }
    }
}
