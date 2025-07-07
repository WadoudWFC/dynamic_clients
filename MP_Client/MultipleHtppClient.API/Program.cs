using FluentValidation;
using Microsoft.OpenApi.Models;
using MultipleHtppClient.API;
using MultipleHtppClient.Infrastructure.HTTP.Extensions;
using MultipleHttpClient.Application.Extensions;
using MultipleHttpClient.Application.Users.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" });
    // Configure Swagger to use the Bearer token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddControllers();

// Add CORS configuration for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200",
                "http://localhost:4201", // Add additional ports if needed
                "https://localhost:4201"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowedToAllowWildcardSubdomains();
    });

    // Alternative: Allow all origins for development (use with caution)
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddApiHttpClients(builder.Configuration);
builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddSingleton<IUseHttpService, UseHttpService>();
builder.Services.AddResponseCompressionConfiguration();

// Validators
builder.Services.AddValidatorsFromAssemblyContaining<LoginCommandValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS - This must be placed before UseAuthentication and UseAuthorization
app.UseCors("AllowAngularApp"); // Use "AllowAll" for development if you encounter issues

app.UseAuthentication();
app.UseAuthorization();
app.UseCustomSecurityHeaders();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();