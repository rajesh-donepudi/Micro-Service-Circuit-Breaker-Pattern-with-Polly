namespace MoonService.Contracts
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherForecast();
    }
}
