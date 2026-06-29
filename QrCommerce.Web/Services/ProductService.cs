using Microsoft.EntityFrameworkCore;
using QrCommerce.Shared.Models;
using QrCommerce.Web.Data;

namespace QrCommerce.Web.Services;

public class ProductService
{
    private readonly AppDbContext _db;

    public ProductService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Product>> GetProductsAsync(int page = 1, int pageSize = 20, string? search = null)
    {
        var query = _db.Products.AsQueryable();
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p => p.Name.Contains(search) || p.Category.Contains(search));
        }
        return await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(string? search = null)
    {
        var query = _db.Products.AsQueryable();
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p => p.Name.Contains(search) || p.Category.Contains(search));
        }
        return await query.CountAsync();
    }

    public async Task<List<Product>> GetActiveProductsAsync()
    {
        return await _db.Products.Where(p => p.IsActive).OrderBy(p => p.Category).ThenBy(p => p.Name).ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _db.Products.FindAsync(id);
    }

    public async Task CreateAsync(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _db.Products.Update(product);
        await _db.SaveChangesAsync();
    }
}
