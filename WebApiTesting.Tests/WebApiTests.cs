using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace WebApiTesting.Tests;

public class WebApiTests
{
    private WebApplicationFactory<Program> factory;

    [SetUp]
    public void Setup()
    {
        factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration(
                (context, config) =>
                {
                    config.AddInMemoryCollection(
                        new Dictionary<string, string?> { ["ForecastLength"] = "7" }
                    );
                }
            );
        });
    }

    [Test]
    public async Task HttpCallSucceeds()
    {
        using var httpClient = factory.CreateClient();

        var weatherForecasts = await httpClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>(
            "/WeatherForecast"
        );

        weatherForecasts.Should().HaveCount(7);
    }

    [TearDown]
    public void TearDown()
    {
        factory.Dispose();
    }
}
