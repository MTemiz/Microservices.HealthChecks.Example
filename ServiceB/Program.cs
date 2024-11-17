using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString: "User ID=postgres; Password=54321; Host=postgres; Port=5432; Database=postgres",
        name: "PostgreSQL Check",
        healthQuery: "SELECT 1",
        failureStatus: HealthStatus.Degraded | HealthStatus.Unhealthy,
        tags: new string[] { "PostgreSQL", "sql", "db" })
    .AddRedis(redisConnectionString: "redis",
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