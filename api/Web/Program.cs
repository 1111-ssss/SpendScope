using Infrastructure;
using Logger;
// using Application.Abstractions.Auth;
using Application.Service.Auth.Handlers;
// using Application.Service.Auth.Helpers;
using Application.Service.Versions.Handlers;
using Application.Service.Profiles.Handlers;
using Application.Service.Profiles.Helpers;
using Application.Service.Achievements.Handlers;
using Application.Service.Follows.Handlers;
using Microsoft.OpenApi;
using MediatR;
using Application.Common.Behaviors;
using Application;
using FluentValidation;
using Infrastructure.Abstractions.Interfaces.Auth;
using Infrastructure.Services.Auth;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;
using Application.Abstractions.DataBase;
using Infrastructure.UnitOfWork;
using Application.Abstractions.Repository;
using Infrastructure.DataBase.Repository;

var builder = WebApplication.CreateBuilder(args);

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SpendScope API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
});

builder.Services.AddControllers();

//for CurrentUserService
builder.Services.AddHttpContextAccessor();

//MediatR + FluentValidation
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly);

    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

//FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<AssemblyMarker>();

//Auth
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();


//Services
var config = builder.Configuration;
builder.Services.AddScoped<RegisterUserHandler>();
builder.Services.AddScoped<LoginUserHandler>();
builder.Services.AddScoped<UploadApkHandler>();
builder.Services.AddScoped<GetLatestHandler>();
builder.Services.AddScoped<GetProfileHandler>();
builder.Services.AddScoped<UpdateAvatarHandler>();
builder.Services.AddScoped<UpdateProfileHandler>();
builder.Services.AddScoped<GetProfileHandler>();
builder.Services.AddScoped<AddAchievementHandle>();
builder.Services.AddScoped<AchievementIconHandle>();
builder.Services.AddScoped<GetAchievementHandle>();
builder.Services.AddScoped<GetFollowsHandler>();
builder.Services.AddScoped<FollowHandler>();
builder.Services.AddScoped<ProfileValidator>();
// builder.Services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
builder.Services.AddScoped(typeof(ICustomLogger<>), typeof(ConsoleLogger<>));
// builder.Services.AddAuth(config);


//Authentication + Authorization
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new ArgumentNullException("Jwt:Key пустой в конфигурации"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

//DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Repository
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));


builder.Services.AddDataAccess(config);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SpendScopeApi v1");
        c.RoutePrefix = string.Empty;
    });
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
var iconPath = builder.Configuration["AppStorage:AchievementsPath"]
                     ?? Path.Combine(Directory.GetCurrentDirectory(), "AchievementsStorage");
if (!Directory.Exists(iconPath))
{
    Directory.CreateDirectory(iconPath);
}

app.Run();