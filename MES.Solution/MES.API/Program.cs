using MES.API.Hubs;
using MES.API.Middleware;
using MES.API.Settings;
using MES.Application;
using MES.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



// Auth
var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>()
    ?? throw new InvalidOperationException(
        "JwtSettings section is missing from configuration.");

//  Validate at startup — fail fast

if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey))
    throw new InvalidOperationException(
        "JwtSettings:SecretKey is not set.\n" +
        "For local dev run:\n" +
        "  dotnet user-secrets set \"JwtSettings:SecretKey\" \"<your-key>\"");

if (jwtSettings.SecretKey.Length < 32)
    throw new InvalidOperationException(
        $"JwtSettings:SecretKey is too short ({jwtSettings.SecretKey.Length} chars). " +
        "Minimum 32 characters required for HMAC-SHA256.");

// Register JwtSettings as singleton
builder.Services.AddSingleton(jwtSettings);


// Application + Infrastructure
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// JWT Authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ClockSkew = TimeSpan.FromMinutes(1)
        };

        // Allow SignalR to pass token via query string
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/hubs"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

//SignalR 
builder.Services.AddSignalR();

//Controllers 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//Swagger with JWT support 
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MES API",
        Version = "v1",
        Description = "Manufacturing Execution System REST API"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Enter: Bearer {your token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

//CORS for Blazor WASM
builder.Services.AddCors(options =>
    options.AddPolicy("BlazorWasm", p =>
        p.WithOrigins("https://localhost:7216", "http://localhost:5054")
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials()));

var app = builder.Build();

//Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MES API v1");
        c.RoutePrefix = string.Empty;   // Swagger at root https://localhost:7216
    });
}

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors("BlazorWasm");
app.UseAuthentication();    // ← must come before UseAuthorization
app.UseAuthorization();
app.MapControllers();
app.MapHub<ProductionHub>("/hubs/production");

app.Run();
