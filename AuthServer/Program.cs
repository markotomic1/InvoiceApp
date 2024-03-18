using AuthServer.Data;
using AuthServer.DTOs;
using AuthServer.Extensions;
using AuthServer.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddAuthServices(builder.Configuration);
builder.Services.AddDataProtection();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<AuthMiddleware>("/login");

app.UseCors(builder => builder
    .WithOrigins(["http://localhost:5002", "http://localhost:3000"])
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());


app.UseAuthentication();
app.UseAuthorization();


app.MapGet("/.well-known/openid-configuration", async (context) =>
{
    var configuration = new OpenIdConfigurationDto
    {

        Issuer = "http://localhost:5001",
        AuthorizationEndpoint = "http://localhost:5001/auth/authorize",
        TokenEndpoint = "http://localhost:5001/auth/token",
        JwksUri = "http://auth-server:8080/auth/.well-known/jwks.json",
    };
    await context.Response.WriteAsJsonAsync(configuration);
});

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();
app.MapFallbackToController("Index", "Fallback");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();

    await context.Database.MigrateAsync();

}
catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}


app.Run();

