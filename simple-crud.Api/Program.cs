using Microsoft.AspNetCore.Authentication.Cookies;

using MySqlConnector;

using simple_crud.Api.Infrastructure;
using simple_crud.Api.Models;

using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<Argon2Options>()
    .Bind(builder.Configuration.GetSection("Argon2Options"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddControllers();
//builder.Services.AddOpenApi();
 
builder.Services.AddScoped<DbConnection>(d =>
{
    return new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IDatabaseRepository, MariaDbRepository>();
builder.Services.AddScoped<IPasswordHasher, Argon2Hasher>();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.Cookie.Name = "simple-crud-auth-cookie";
    });

builder.Services.AddCors(options => { 
    options.AddPolicy("ClientPolicy", policy => 
    { 
        policy.WithOrigins("https://simple-crud.client.andresgilglz.dev")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

app.UseCors("ClientPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
