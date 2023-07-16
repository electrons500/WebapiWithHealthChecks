using Microsoft.Extensions.Diagnostics.HealthChecks;
using RestSharp;

namespace WebapiWithHealthChecks.Services.Health.apis.weather
{
    public class WeatherApihealthChecks : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            string url = "https://localhost:44351/WeatherForecast";
            var client = new RestClient();  //you also use HttpClient for the api call
            var request = new RestRequest(url, Method.Get);
            request.AddHeader("accept", "application/json");

            var response = client.Execute(request, cancellationToken: cancellationToken);
            if (response.IsSuccessful)
            {
                return Task.FromResult(HealthCheckResult.Healthy("Healthy result from WeatherApihealthChecks"));
            }
            else
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Unhealthy result from WeatherApihealthChecks"));
            }
        }
    }
}
