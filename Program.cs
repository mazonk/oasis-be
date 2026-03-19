using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Oasis.Data;

// Load environment variables from .env
Env.Load();

// Build the app
var builder = WebApplication.CreateBuilder(args);

// --------------------
// Configure Services
// --------------------

// PostgreSQL connection string from .env
var host = Environment.GetEnvironmentVariable("DB_HOST");
var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
var db = Environment.GetEnvironmentVariable("DB_NAME");
var user = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
var ssl = Environment.GetEnvironmentVariable("DB_SSLMODE") ?? "Require";
var trustCert = Environment.GetEnvironmentVariable("DB_TRUSTCERT") ?? "True";

var connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={password};SSL Mode={ssl};Trust Server Certificate={trustCert}";

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add Controllers
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --------------------
// Build App
// --------------------
var app = builder.Build();

// --------------------
// Configure Middleware
// --------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Map controller routes
app.MapControllers();

app.Run();