using Microsoft.EntityFrameworkCore;
using StockPortfolioAPI.Data;
using StockPortfolioAPI.Repositories;
using StockPortfolioAPI.Services;
using StockPortfolioAPI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using StockPortfolioAPI.Services.Auth;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Stock Portfolio API", Version = "v1",Description = "API for managing stock portfolios" });

    // Add JWT Bearer authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme{
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme, 
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Add Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAuthenticatedUser", policy => policy.RequireAuthenticatedUser());
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOrAdmin", policy => policy.RequireRole("User", "Admin"));
});

// Add Entity Framework
builder.Services.AddDbContext<StockPortfolioDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Repositories
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

// Add Services
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

//Add Auth Services
builder.Services.AddScoped<IAuthService, AuthService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Add authentication middleware
app.UseAuthentication();

// Add authorization middleware
app.UseAuthorization();


// Add global exception handling middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

// Ensure database is created and seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<StockPortfolioDbContext>();
    await SeedDataService.SeedAsync(context);
}

app.Run();
