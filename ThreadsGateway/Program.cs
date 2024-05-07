using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile("ocelot.json");
builder.Configuration.AddEnvironmentVariables();
var jwtKey = builder.Configuration["JWT:Key"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(jwtKey, options =>
    {
        options.Authority = "https://localhost:7024";
        options.Audience = "https://localhost:7063";
    }
);
builder.Services.AddOcelot();
var app = builder.Build();
app.UseOcelot().Wait();
app.Run();
