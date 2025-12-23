using MySqlConnector;

using simple_crud.Api.Infrastructure;
using simple_crud.Api.Models;

using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<DbConnection>(d =>
{
    return new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IDatabaseRepository, MariaDbRepository>();
builder.Services.AddScoped<IPasswordHasher>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
