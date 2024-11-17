using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks()
    .AddMongoDb(mongodbConnectionString: "mongodb://localhost:27017",
        name: "Mongo Check",
        failureStatus: HealthStatus.Degraded | HealthStatus.Unhealthy,
        tags: new[] { "MongoDB" }
    )
    .AddNpgSql(connectionString: "User ID=postgres; Password=54321; Host=localhost; Port=5432; Database=postgres",
        name: "PostgreSQL Check",
        healthQuery: "SELECT 1",
        failureStatus: HealthStatus.Degraded | HealthStatus.Unhealthy,
        tags: new string[] { "PostgreSQL", "sql", "db" })
    .AddRedis(redisConnectionString: "localhost:6379",
        name: "Redis Check",
        failureStatus: HealthStatus.Degraded | HealthStatus.Unhealthy,
        tags: new string[] { "redis" });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();