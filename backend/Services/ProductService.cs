namespace Areco.Services
{
    using Areco.Data;
    using Areco.DTOs;
    using Areco.Models;
    using Microsoft.EntityFrameworkCore;

    public class ProductService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductService> _logger;

        public ProductService(AppDbContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PagedResult<Product>> GetAll(int page, int pageSize)
        {
            var totalItems = await _context.Products.CountAsync();

            var products = await _context.Products
                .OrderByDescending(p => p.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return new PagedResult<Product>
            {
                Data = products,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<Product> Create(ProductCreateDto dto)
        {
            try
            {
                if (dto.StockQuantity < 0)
                    throw new Exception("Estoque não pode ser negativo");

                if (dto.Category == "Eletrônicos" && dto.Price < 50)
                    throw new Exception("Preço mínimo para eletrônicos é de R$50,00");

                var skuExists = await _context.Products
                    .AnyAsync(p => p.SKU == dto.SKU);

                if (skuExists)
                    throw new Exception("SKU já cadastrado");

                var product = new Product
                {
                    Name = dto.Name,
                    SKU = dto.SKU,
                    Category = dto.Category,
                    Price = dto.Price,
                    StockQuantity = dto.StockQuantity,
                    Timestamp = DateTime.UtcNow
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Produto criado: {SKU} - {Name}", dto.SKU, dto.Name);

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar produto: {SKU} - {Name}", dto.SKU, dto.Name);

                throw;
            }

        }

        public async Task<Product> Update(int id, ProductCreateDto dto)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                    throw new Exception("Produto não encontrado");

                if (dto.StockQuantity < 0)
                    throw new Exception("Estoque não pode ser negativo");

                if (dto.Category == "Eletrônicos" && dto.Price < 50)
                    throw new Exception("Preço mínimo para eletrônicos é de R$50,00");

                var skuExists = await _context.Products
                    .AnyAsync(p => p.SKU == dto.SKU && p.Id != id);

                if (skuExists)
                    throw new Exception("SKU já cadastrado");

                product.Name = dto.Name;
                product.SKU = dto.SKU;
                product.Category = dto.Category;
                product.Price = dto.Price;
                product.StockQuantity = dto.StockQuantity;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Produto atualizado: {Id} - {Name}", product.Id,  product.Name);

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar produto: {Id} - {Name}", id, dto?.Name);

                throw;
            }

        }

        public async Task<bool> Delete(int id)
        {
            Product? product = null;

            try
            {
                product = await _context.Products.FindAsync(id);

                if (product == null)
                    return false;

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Produto excluído: {Id} - {Name}", product.Id, product.Name);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir produto: {Id} - {Name}", id, product?.Name);

                throw;
            }

        }

        public async Task<Product?> GetById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

    }
}
