using Microsoft.EntityFrameworkCore;
using QrCommerce.Web.Background;
using QrCommerce.Web.Data;
using QrCommerce.Web.Helpers;
using QrCommerce.Web.Seed;
using QrCommerce.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=qrcommerce.db"));

builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<TelecomService>();
builder.Services.AddScoped<CampaignService>();
builder.Services.AddScoped<QrLocationService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<AnalyticsService>();
builder.Services.AddScoped<DeviceService>();

builder.Services.AddHostedService<TelecomRetryWorker>();

builder.Services.AddRazorPages();
builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    DataSeeder.Seed(db);
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<RateLimitMiddleware>();
app.UseMiddleware<ApiKeyMiddleware>();

app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapControllers();

app.Run();
