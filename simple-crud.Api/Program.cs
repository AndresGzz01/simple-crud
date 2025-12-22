using MySqlConnector;

using simple_crud.Api.Infrastructure;
using simple_crud.Api.Models;

using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<DbConnection>(d =>
{
    return new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IDatabaseRepository, MariaDbRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
