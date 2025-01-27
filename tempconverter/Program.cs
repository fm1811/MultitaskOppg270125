using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",policy =>
    {
        policy.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    });
});
var app = builder.Build();

app.UseCors("AllowAll");
app.UseStaticFiles();
app.MapGet("/", context => 
{
    context.Response.Redirect("/index.html");
    return Task.CompletedTask;
});

app.MapPost("/convert", ([FromBody] TemperatureConverter converter) =>
{
    double inputValue;
    if (!double.TryParse(converter.InputValue, out inputValue))
    {
        return Results.BadRequest("Invalid InputValue. It must be a valid number.");
    }

    string fromUnit = converter.FromUnit;
    string toUnit = converter.ToUnit;
    if (string.IsNullOrEmpty(fromUnit) || string.IsNullOrEmpty(toUnit))
    {
        return Results.BadRequest("FromUnit and ToUnit are required.");
    }

    if (fromUnit != "celsius" && fromUnit != "fahrenheit" && fromUnit != "kelvin")
    {
        return Results.BadRequest("Invalid FromUnit. Valid values are 'celsius', 'fahrenheit', 'kelvin'.");
    }

    if (toUnit != "celsius" && toUnit != "fahrenheit" && toUnit != "kelvin")
    {
        return Results.BadRequest("Invalid ToUnit. Valid values are 'celsius', 'fahrenheit', 'kelvin'.");
    }

    if (double.IsNaN(inputValue) || double.IsInfinity(inputValue))
    {
        return Results.BadRequest("Invalid InputValue.");
    }
    if (fromUnit == "celsius")
    {
        converter.ConvertFromCelsius(inputValue);
    }
    else if (fromUnit == "fahrenheit")
    {
        converter.ConvertFromFahrenheit(inputValue);
    }
    else if (fromUnit == "kelvin")
    {
        converter.ConvertFromKelvin(inputValue);
    }
    return Results.Ok(new { Celsius = converter.Celsius, Fahrenheit = converter.Fahrenheit, Kelvin = converter.Kelvin });
}).DisableAntiforgery();

app.Run();

public class TemperatureConverter
{
    public double Celsius { get; set; } = 0;
    public double Fahrenheit { get; set; } = 0;
    public double Kelvin { get; set; } = 0;
    public required string FromUnit { get; set; } = "";
    public required string ToUnit { get; set; } = "";
    public required string InputValue { get; set; } = ""; 

    public void ConvertFromCelsius(double celsius)
    {
        Celsius = celsius;
        if (ToUnit == "fahrenheit")
        {
            Fahrenheit = (celsius * 9 / 5) + 32;
        }
        else if (ToUnit == "kelvin")
        {
            Kelvin = celsius + 273.15;
        }
    }

    public void ConvertFromFahrenheit(double fahrenheit)
    {
        Fahrenheit = fahrenheit;
        if (ToUnit == "celsius")
        {
            Celsius = (fahrenheit - 32) * 5 / 9;
        }
        else if (ToUnit == "kelvin")
        {
            Kelvin = (fahrenheit - 32) * 5 / 9 + 273.15;
        }
    }

    public void ConvertFromKelvin(double kelvin)
    {
        Kelvin = kelvin;
        if (ToUnit == "celsius")
        {
            Celsius = kelvin - 273.15;
        }
        else if (ToUnit == "fahrenheit")
        {
            Fahrenheit = (kelvin - 273.15) * 9 / 5 + 32;
        }
    }
}