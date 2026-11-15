using IdentityService.Data;
using IdentityService.Interfaces;
using IdentityService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata.Ecma335;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        NameClaimType = "sub",
        RoleClaimType = "role",
    };
});
builder.Services.AddAuthorization();
builder.Services.AddScoped<IJwtTokenProvider,JwtTokenProvider>();
builder.Services.AddSingleton<SoftDeleteInterceptor>();
builder.Services.AddDbContext<AppDbContext>((sp,options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
    options.AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>());
});

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();    
app.MapControllers();

app.MapGet("/1", () => "Hello World!!!!!").RequireAuthorization();
app.MapGet("/2", () => "authenticated").RequireAuthorization() ;


app.Run();
