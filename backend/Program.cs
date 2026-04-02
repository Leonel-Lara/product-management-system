using Areco.Data;
using Areco.Models;
using Areco.Services;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=products.db"));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<ProductService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.Products.Any())
    {
        db.Products.AddRange(
            new Product { Name = "Notebook Dell Inspiron", SKU = "NOTE-DELL-001", Category = "Eletrônicos", Price = 3500, StockQuantity = 5 },
            new Product { Name = "Smartphone Samsung Galaxy S21", SKU = "CEL-SAMS-021", Category = "Eletrônicos", Price = 2800, StockQuantity = 8 },
            new Product { Name = "Fone de Ouvido Bluetooth JBL", SKU = "FONE-JBL-110", Category = "Eletrônicos", Price = 250, StockQuantity = 15 },
            new Product { Name = "Monitor LG 24 Polegadas", SKU = "MON-LG-024", Category = "Eletrônicos", Price = 900, StockQuantity = 6 },

            new Product { Name = "Sofá 3 Lugares Retrátil", SKU = "SOFA-RET-003", Category = "Móveis", Price = 1800, StockQuantity = 2 },
            new Product { Name = "Mesa de Jantar 6 Lugares", SKU = "MESA-JANT-006", Category = "Móveis", Price = 1200, StockQuantity = 3 },
            new Product { Name = "Cadeira Gamer Ergonômica", SKU = "CADEIRA-GAME-01", Category = "Móveis", Price = 950, StockQuantity = 4 },

            new Product { Name = "Camiseta Básica Algodão", SKU = "CAMISETA-BAS-001", Category = "Roupas", Price = 50, StockQuantity = 20 },
            new Product { Name = "Calça Jeans Masculina", SKU = "CALCA-JEANS-032", Category = "Roupas", Price = 120, StockQuantity = 12 },
            new Product { Name = "Jaqueta Feminina Couro Sintético", SKU = "JAQUETA-FEM-010", Category = "Roupas", Price = 220, StockQuantity = 7 },

            new Product { Name = "Panela Antiaderente 5L", SKU = "PANELA-ANTI-005", Category = "Utilidades", Price = 80, StockQuantity = 10 },
            new Product { Name = "Liquidificador 1000W", SKU = "LIQ-1000W-220", Category = "Utilidades", Price = 180, StockQuantity = 6 },
            new Product { Name = "Garrafa Térmica Inox 1L", SKU = "GARRAFA-INOX-1L", Category = "Utilidades", Price = 60, StockQuantity = 14 },
            new Product { Name = "Kit Talheres 24 Peças", SKU = "TALHERES-KIT-24", Category = "Utilidades", Price = 90, StockQuantity = 9 }
        );

        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Areco API v1"));
}

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
