using Microsoft.OpenApi;
using Application.DI;
using Infrastructure.DI;
using Web.Extensions;
using Web.Middleware;

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
builder.Services.AddApplication();

//Db, UoW, Repositories, Storage, Services
builder.Services.AddInfrastructure(builder.Configuration);

//Jwt + Policy
builder.Services.AddAuth(builder.Configuration);

//RateLimit
builder.Services.AddRateLimit();

//Serilog
builder.Host.UseCustomSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SpendScope API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseIPValidation();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();

app.Run();