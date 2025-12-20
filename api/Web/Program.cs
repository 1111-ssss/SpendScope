using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Application.Service.Auth;
using Application.Abstractions.Auth;
using Infrastructure.Interfaces;
using Application.Service.Auth.Handlers;
using Application.Service.Auth.Helpers;
using Application.Abstractions.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddScoped<RegisterUserHandler>();
builder.Services.AddScoped<LoginUserHandler>();
builder.Services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
builder.Services.AddAuth(config);

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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

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

var apkStoragePath = builder.Configuration["AppSettings:ApkStoragePath"]
                     ?? Path.Combine(Directory.GetCurrentDirectory(), "ApkStorage");
if (!Directory.Exists(apkStoragePath))
{
    Directory.CreateDirectory(apkStoragePath);
}

app.Run();