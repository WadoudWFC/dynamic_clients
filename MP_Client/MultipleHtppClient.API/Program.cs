using MultipleHtppClient.API;
using MultipleHtppClient.Infrastructure.HTTP.Extensions;
using MultipleHttpClient.Application;
using MultipleHttpClient.Application.Users.Validators;
using MutipleHttpClient.Domain;
using FluentValidation.AspNetCore;
using MediatR;
using MultipleHttpClient.Application.Commons.Behavior;
using FluentValidation;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddApiHttpClients(builder.Configuration);
builder.Services.AddApplicationService();
builder.Services.AddSingleton<IUseHttpService, UseHttpService>();
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<MultipleHttpClient.Application.Users.Handlers.LoginCommandHandler>());

// Validators
builder.Services.AddValidatorsFromAssemblyContaining<LoginCommandValidator>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

