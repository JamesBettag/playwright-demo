using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Text.Json;
using Xunit;

namespace WebApp.Tests.ApiTests;

public class ApiTests(AspireFixture aspireFixture) : PlaywrightTest, IClassFixture<AspireFixture>
{
    private IAPIRequestContext? _requestContext;
    
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        
        _requestContext = await Playwright.APIRequest.NewContextAsync(new()
        {
            BaseURL = aspireFixture.ApiEndpoint.ToString(),
        });
    }

    public override async Task DisposeAsync()
    {
        await (_requestContext?.DisposeAsync() ?? ValueTask.CompletedTask);

        await base.DisposeAsync();
    }
    
    [Fact]
    public async Task WeatherForecast_ReturnsValidStructure()
    {
        // Act
        var response = await _requestContext!.GetAsync("/weatherforecast");
        Assert.True(response.Ok);

        var jsonResponse = await response.JsonAsync();
        var forecasts = JsonSerializer.Deserialize<JsonElement>(jsonResponse.ToString());

        // Assert
        Assert.True(forecasts.ValueKind == JsonValueKind.Array);
        Assert.Equal(5, forecasts.GetArrayLength());

        // Check the structure of the first forecast
        var firstForecast = forecasts[0];
        Assert.True(firstForecast.TryGetProperty("date", out var date));
        Assert.True(firstForecast.TryGetProperty("temperatureC", out var tempC));
        Assert.True(firstForecast.TryGetProperty("temperatureF", out var tempF));
        Assert.True(firstForecast.TryGetProperty("summary", out var summary));

        // Verify data types
        Assert.Equal(JsonValueKind.String, date.ValueKind);
        Assert.Equal(JsonValueKind.Number, tempC.ValueKind);
        Assert.Equal(JsonValueKind.Number, tempF.ValueKind);
        Assert.Equal(JsonValueKind.String, summary.ValueKind);
    }
}