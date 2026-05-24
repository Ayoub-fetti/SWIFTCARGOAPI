using Microsoft.EntityFrameworkCore;
using SWIFTCARGOAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Récupérer la chaîne de connexion
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Ajouter le DbContext au conteneur de services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();