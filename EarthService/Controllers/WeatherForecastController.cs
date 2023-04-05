using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Wrap;

namespace EarthService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly Random jitterer = new Random();

        private static readonly AsyncRetryPolicy<HttpResponseMessage> AsyncRetryPolicy = Policy.HandleResult<HttpResponseMessage>(message => (int)message.StatusCode == 429 || (int)message.StatusCode >= 500
        ).WaitAndRetryAsync(retryCount: 2, sleepDurationProvider: (retryAttempt) =>
        {
            LogAttempt(retryAttempt);
            return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitterer.Next(0, 1000));
        });

        private static readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> AsyncCircuitBreakerPolicy = Policy.HandleResult<HttpResponseMessage>(message => (int)message.StatusCode == 503)
            .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 2, durationOfBreak: TimeSpan.FromMinutes(1));

        private static readonly AsyncPolicyWrap<HttpResponseMessage> _resilientPolicy = AsyncCircuitBreakerPolicy.WrapAsync(AsyncRetryPolicy);

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            var requestUrl = GetWeatherForecastService(AsyncCircuitBreakerPolicy.CircuitState, AsyncCircuitBreakerPolicy);

            //if (AsyncCircuitBreakerPolicy.CircuitState == CircuitState.Open)
            //{
            //    throw new Exception("Service unavailable in Moon");
            //}

            var httpClient = _httpClientFactory.CreateClient();

            var response = await _resilientPolicy.ExecuteAsync(() =>
            {
                return httpClient.GetAsync(requestUrl);
            });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Service unavailable.{AsyncCircuitBreakerPolicy.CircuitState}");
            }

            var result = await response.Content.ReadAsStringAsync();

            var weather = JsonConvert.DeserializeObject<List<WeatherForecast>>(result);

            return Ok(weather);
        }

        private static void LogAttempt(int attempt)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Retry Attempt: {attempt}");
            Console.ResetColor();
        }

        private string GetWeatherForecastService(CircuitState state, AsyncCircuitBreakerPolicy<HttpResponseMessage> policy)
        {
            switch(state)
            {
                case CircuitState.Open:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("FAULT: Fetching weather info from Mars Service");
                    Console.ResetColor();
                    policy.Reset();
                    return "https://localhost:7019/WeatherForecast";
                default:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("DEFAULT: Fetching weather info from Moon Service");
                    Console.ResetColor();
                    return "https://localhost:7171/WeatherForecast";
            }
        }
    }
}