using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Application.Service.Auth;
using Application.Abstractions.Auth;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddScoped<RegisterUserHandler>();
builder.Services.AddScoped<LoginUserHandler>();
builder.Services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();

builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Logging.AddConsole();

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

builder.Services.AddDataAccess(config);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();