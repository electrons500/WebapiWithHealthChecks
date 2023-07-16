using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebapiWithHealthChecks.Services.Health.apis;
using WebapiWithHealthChecks.Services.Health.apis.Jokes;
using WebapiWithHealthChecks.Services.Health.apis.weather;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//configure health checks services
//Degraded means our application is still running, but not responding within an expected time frame.

builder.Services.AddHealthChecks()
                .AddCheck<ChuckNorrisJokesApihealthChecks>("Chuck Norris Jokes Api", failureStatus: HealthStatus.Degraded, tags: new[] { "api" })
                .AddCheck<WeatherApihealthChecks>("Weather Api", failureStatus: HealthStatus.Degraded, tags: new[] { "api" })
                .AddSqlServer(connectionString: builder.Configuration.GetConnectionString("sqlserver"), healthQuery: "SELECT 1;", name: "Sqlserver 2022",
                failureStatus: HealthStatus.Degraded,
                tags: new string[] { "db", "sql", "sqlserver" })
                .AddMySql(builder.Configuration.GetConnectionString("mysql"), failureStatus: HealthStatus.Degraded, tags: new string[] { "db", "sql", "mysqlserver" });




builder.Services.AddHealthChecksUI()
                .AddInMemoryStorage();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//Endpoint for health checks
app.MapHealthChecks("/health/apis", new HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("api"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/databases", new HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("db"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(setup => setup.AddCustomStylesheet("dotnet.css"));


app.Run();
