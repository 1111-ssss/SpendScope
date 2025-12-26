using Infrastructure;
using Logger;
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
using Application.Service.Versions.Handlers;
using Application.Service.Profiles.Handlers;
using Application.Service.Profiles.Helpers;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddScoped<RegisterUserHandler>();
builder.Services.AddScoped<LoginUserHandler>();
builder.Services.AddScoped<UploadApkHandler>();
builder.Services.AddScoped<GetLatestHandler>();
builder.Services.AddScoped<GetProfileHandler>();
builder.Services.AddScoped<UpdateAvatarHandler>();
builder.Services.AddScoped<UpdateProfileHandler>();
builder.Services.AddScoped<ProfileValidator>();
builder.Services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
builder.Services.AddScoped(typeof(ICustomLogger<>), typeof(ConsoleLogger<>));
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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var apkStoragePath = builder.Configuration["AppStorage:ApkPath"]
                     ?? Path.Combine(Directory.GetCurrentDirectory(), "ApkStorage");
if (!Directory.Exists(apkStoragePath))
{
    Directory.CreateDirectory(apkStoragePath);
}
var avatarPath = builder.Configuration["AppStorage:AvatarPath"]
                     ?? Path.Combine(Directory.GetCurrentDirectory(), "AvatarStorage");
if (!Directory.Exists(avatarPath))
{
    Directory.CreateDirectory(avatarPath);
}

app.Run();