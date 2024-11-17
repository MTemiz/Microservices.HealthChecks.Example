using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecksUI(
        settings =>
        {
            settings.AddHealthCheckEndpoint("ServiceA", "http://servicea:8080/health");
            settings.AddHealthCheckEndpoint("ServiceB", "http://serviceb:8080/health");
            settings.SetEvaluationTimeInSeconds(3);
            settings.SetApiMaxActiveRequests(3);
            settings.ConfigureApiEndpointHttpclient((serviceProvider, httpClient) =>
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "...");
            });
            settings.ConfigureWebhooksEndpointHttpclient((serviceProvider, httpClient) =>
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "...");
            });
        }
    )
    .AddSqlServerStorage(
        "Server=sql,1433;Database=MonitoringDB;User ID=SA;Password=YourStrong@Passw0rd;TrustServerCertificate=True");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHealthChecksUI(opt =>
{
    opt.UIPath = "/health-ui";
});

app.Run();