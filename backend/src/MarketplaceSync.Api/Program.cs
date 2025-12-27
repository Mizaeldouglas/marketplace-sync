// ==========================================
// File: src/MarketplaceSync.Api/Program.cs
// ==========================================
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MarketplaceSync.Data.Context;
using MarketplaceSync.Data.Repositories;
using MarketplaceSync.Domain.Interfaces;
using MarketplaceSync.Service.Auth;
using MarketplaceSync.Service.Product;
using MarketplaceSync.Service.Marketplace;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. CONFIGURAR BANCO DE DADOS
// ==========================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MarketplaceSyncContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("MarketplaceSync.Data"))
);

// ==========================================
// 2. CONFIGURAR JWT
// ==========================================
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var jwtKey = jwtSettings["Key"];
var jwtIssuer = jwtSettings["Issuer"];
var jwtAudience = jwtSettings["Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)),
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// ==========================================
// 3. REGISTRAR SERVIÇOS (Dependency Injection)
// ==========================================

// Unit of Work e Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

// Services
builder.Services.AddScoped<ITokenService>(sp =>
    new TokenService(jwtKey, jwtIssuer, jwtAudience)
);
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<MarketplaceSync.Service.Marketplace.ISyncService, SyncService>();

// ==========================================
// 4. CORS
// ==========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:4201")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// ==========================================
// 5. SWAGGER
// ==========================================
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// ==========================================
// 6. CONTROLLERS
// ==========================================
builder.Services.AddControllers();

var app = builder.Build();

// ==========================================
// 7. MIDDLEWARE
// ==========================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ==========================================
// 8. SEED DATABASE (Criar tabelas)
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<MarketplaceSyncContext>();

    // Aplicar migrations
    context.Database.Migrate();

    Console.WriteLine("✓ Banco de dados configurado com sucesso!");
}

app.Run();

// ==========================================
// File: src/MarketplaceSync.Api/appsettings.json
// ==========================================
/*
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=marketplace_sync;Username=admin;Password=admin123"
  },
  "JwtSettings": {
    "Key": "sua-chave-secreta-muito-longa-aqui-com-mais-de-32-caracteres-para-seguranca",
    "Issuer": "MarketplaceSync",
    "Audience": "MarketplaceSyncUsers"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
*/

// ==========================================
// File: src/MarketplaceSync.Api/appsettings.Development.json
// ==========================================
/*
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft": "Debug",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
*/