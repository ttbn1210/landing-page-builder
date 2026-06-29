using Microsoft.EntityFrameworkCore;
using QrCommerce.Shared.Models;
using QrCommerce.Web.Data;

namespace QrCommerce.Web.Services;

public class CustomerService
{
    private readonly AppDbContext _db;

    public CustomerService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Customer>> GetCustomersAsync(int page = 1, int pageSize = 20, string? search = null)
    {
        var query = _db.Customers.AsQueryable();
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(c => c.Name.Contains(search) || c.Phone.Contains(search) || c.Email.Contains(search));
        }
        return await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(string? search = null)
    {
        var query = _db.Customers.AsQueryable();
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(c => c.Name.Contains(search) || c.Phone.Contains(search));
        }
        return await query.CountAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _db.Customers.Include(c => c.Orders).FirstOrDefaultAsync(c => c.Id == id);
    }
}
