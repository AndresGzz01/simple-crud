using Microsoft.IdentityModel.Tokens;

using MySqlConnector;

using simple_crud.Api.Infrastructure;
using simple_crud.Api.Models;

using System.Data.Common;
using System.Text;

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
    .AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options => 
    {
        var secretKey = builder.Configuration.GetValue<string>("SecretKey");

        if (secretKey is null)
            throw new InvalidOperationException("SecretKey configuration is missing.");

        options.TokenValidationParameters = new TokenValidationParameters 
            { 
                ValidateIssuer = true, 
                ValidateAudience = true, 
                ValidateLifetime = true, 
                ValidateIssuerSigningKey = true, 
                ValidIssuer = "simple-crud.api", 
                ValidAudience = "simple-crud.client", 
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) 
        }; 
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
