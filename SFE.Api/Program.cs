/// <summary>
/// The main entry point for the SFE.Api web application.
/// This file configures the web application builder, registers necessary services,
/// and sets up the HTTP request pipeline.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// Add services to the dependency injection container.
// Register OpenAPI services to automatically generate API documentation (e.g., Swagger/OpenAPI spec).
builder.Services.AddOpenApi();

// Build the ASP.NET Core web application instance based on the configured services.
var app = builder.Build();

// Configure the HTTP request pipeline.
// Only enable OpenAPI endpoint mapping if the application is running in a Development environment, 
// preventing API schema exposure in Production.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Enforce HTTPS redirection for all incoming HTTP requests to ensure secure communication.
app.UseHttpsRedirection();

/// <summary>
/// A statically defined array of descriptive summaries for the mock weather conditions.
/// </summary>
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

/// <summary>
/// Defines a GET endpoint at "/weatherforecast" that generates and returns randomized mock weather data.
/// </summary>
/// <returns>An array of 5 randomly generated <see cref="WeatherForecast"/> objects.</returns>
app.MapGet("/weatherforecast", () =>
{
    // Generate a sequence of 5 forecast items, starting from the next day.
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    return forecast;
})
// Assigns a unique name to the endpoint. This is highly recommended for routing link generation 
// and makes the OpenAPI documentation more robust.
.WithName("GetWeatherForecast");

// Start the web application and begin listening for incoming API requests.
app.Run();

/// <summary>
/// Represents a single daily weather forecast.
/// </summary>
/// <param name="Date">The date the forecast applies to.</param>
/// <param name="TemperatureC">The projected temperature in degrees Celsius.</param>
/// <param name="Summary">A brief qualitative summary of the temperature (e.g., "Chilly").</param>
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    /// <summary>
    /// Gets the temperature converted from Celsius to Fahrenheit. 
    /// This is an evaluated property that computes dynamically.
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}