using FluentValidation;
using Microsoft.OpenApi.Models;
using MultipleHtppClient.API;
using MultipleHtppClient.Infrastructure.HTTP.Extensions;
using MultipleHttpClient.Application;
using MultipleHttpClient.Application.Commons.Behavior;
using MultipleHttpClient.Application.Extensions;
using MultipleHttpClient.Application.Users.Validators;
using MutipleHttpClient.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" });
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

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalNumericValidationFilter>();
});
builder.Services.AddScoped<GlobalNumericValidationFilter>();
builder.Services.Configure<SecurityHeadersOptions>(options =>
{
    options.ForceHSTS = !builder.Environment.IsDevelopment();
    options.ContentSecurityPolicy = "default-src 'self'; script-src 'self' 'unsafe-inline'";
});
// Add CORS configuration for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200",
                "http://localhost:4201",
                "https://localhost:4201"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddApiHttpClients(builder.Configuration);
builder.Services.AddApplicationService(builder.Configuration);
// TO DO: Remove this service in production!
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
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHsts();
app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseMiddleware<SecurityHeadersMiddleware>();


app.UseAuthentication();
app.UseAuthorization();
// app.UseCustomSecurityHeaders();
app.UseApiRouteSegregation();
app.MapControllers();

await app.RunAsync();