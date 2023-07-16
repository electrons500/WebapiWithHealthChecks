using Microsoft.Extensions.Diagnostics.HealthChecks;
using RestSharp;

namespace WebapiWithHealthChecks.Services.Health.apis.Jokes
{
    public class ChuckNorrisJokesApihealthChecks : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            string url = "https://matchilling-chuck-norris-jokes-v1.p.rapidapi.com/jokes/random";
            var client = new RestClient();
            var request = new RestRequest(url, Method.Get);
            request.AddHeader("accept", "application/json");
            request.AddHeader("X-RapidAPI-Key", "c5e3a2e511msh88bfd7e7cb99b2ap10145bjsnec94f6ab89e8");
            request.AddHeader("X-RapidAPI-Host", "matchilling-chuck-norris-jokes-v1.p.rapidapi.com");

            var response = client.Execute(request, cancellationToken: cancellationToken);
            if (response.IsSuccessful)
            {
                return Task.FromResult(HealthCheckResult.Healthy("Healthy result from ApihealthChecks"));
            }
            else
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Unhealthy result from ApihealthChecks"));
            }

        }
    }
}
