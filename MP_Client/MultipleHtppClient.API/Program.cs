using MultipleHtppClient.API;
using MultipleHtppClient.Infrastructure.HTTP.Extensions;
using MultipleHttpClient.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddApiHttpClients(builder.Configuration);
builder.Services.AddSingleton<IUseHttpService, UseHttpService>();
builder.Services.AddSingleton<IHttpManagementAglou, HttpManagementAglouService>();
builder.Services.AddSingleton<IHttpUserAglou, HttpUserAglouService>();


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

