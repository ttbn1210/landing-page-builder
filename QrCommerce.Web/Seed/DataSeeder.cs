using QrCommerce.Shared.Enums;
using QrCommerce.Shared.Models;
using QrCommerce.Web.Data;

namespace QrCommerce.Web.Seed;

public static class DataSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (db.Campaigns.Any()) return;

        var campaigns = new List<Campaign>
        {
            new() { Name = "Summer Food Festival", Description = "QR codes at restaurant tables during summer festival", CampaignCode = "CAMP-SUMMER-2024", Status = CampaignStatus.Running, StartDate = new DateTime(2024, 6, 1), EndDate = new DateTime(2024, 8, 31), Budget = 5000m },
            new() { Name = "School Marketing", Description = "QR banners placed at university gates", CampaignCode = "CAMP-SCHOOL-2024", Status = CampaignStatus.Running, StartDate = new DateTime(2024, 9, 1), EndDate = new DateTime(2024, 12, 31), Budget = 3000m },
            new() { Name = "Vehicle Advertising", Description = "QR stickers on company fleet vehicles", CampaignCode = "CAMP-VEHICLE-2024", Status = CampaignStatus.Running, StartDate = new DateTime(2024, 1, 1), EndDate = new DateTime(2024, 12, 31), Budget = 8000m },
            new() { Name = "Mall Promotion", Description = "QR standees at shopping mall entrances", CampaignCode = "CAMP-MALL-2024", Status = CampaignStatus.Paused, StartDate = new DateTime(2024, 3, 1), EndDate = new DateTime(2024, 6, 30), Budget = 4000m },
            new() { Name = "Street Campaign", Description = "QR posters on bus stops and street poles", CampaignCode = "CAMP-STREET-2024", Status = CampaignStatus.Draft, StartDate = new DateTime(2024, 10, 1), EndDate = new DateTime(2025, 3, 31), Budget = 6000m },
        };
        db.Campaigns.AddRange(campaigns);
        db.SaveChanges();

        var locations = new List<QrLocation>
        {
            new() { CampaignId = 1, Name = "Restaurant Table 01", Description = "Main dining hall table 1", QrCode = "QR-REST-T01", Latitude = 10.7769, Longitude = 106.7009, Address = "123 Le Loi, District 1, HCMC", TotalScans = 145, TotalOrders = 32 },
            new() { CampaignId = 1, Name = "Restaurant Table 02", Description = "Main dining hall table 2", QrCode = "QR-REST-T02", Latitude = 10.7770, Longitude = 106.7010, Address = "123 Le Loi, District 1, HCMC", TotalScans = 98, TotalOrders = 21 },
            new() { CampaignId = 1, Name = "Restaurant Bar Counter", Description = "Bar counter QR placement", QrCode = "QR-REST-BAR", Latitude = 10.7771, Longitude = 106.7011, Address = "123 Le Loi, District 1, HCMC", TotalScans = 67, TotalOrders = 15 },
            new() { CampaignId = 2, Name = "School Banner Gate A", Description = "Main entrance banner", QrCode = "QR-SCHOOL-GA", Latitude = 10.8231, Longitude = 106.6297, Address = "227 Nguyen Van Cu, District 5, HCMC", TotalScans = 312, TotalOrders = 45 },
            new() { CampaignId = 2, Name = "School Cafeteria", Description = "Cafeteria table standee", QrCode = "QR-SCHOOL-CAF", Latitude = 10.8232, Longitude = 106.6298, Address = "227 Nguyen Van Cu, District 5, HCMC", TotalScans = 189, TotalOrders = 28 },
            new() { CampaignId = 3, Name = "Ford Ecosport Left Door", Description = "Driver side door sticker", QrCode = "QR-VEH-FE01", Latitude = 10.7500, Longitude = 106.6500, Address = "Mobile - HCMC Area", VehicleCode = "51A-12345", TotalScans = 78, TotalOrders = 12 },
            new() { CampaignId = 3, Name = "Toyota Vios Rear Window", Description = "Rear window QR sticker", QrCode = "QR-VEH-TV01", Latitude = 10.7600, Longitude = 106.6600, Address = "Mobile - HCMC Area", VehicleCode = "51A-67890", TotalScans = 56, TotalOrders = 8 },
            new() { CampaignId = 4, Name = "Mall Standee 03", Description = "Ground floor main entrance", QrCode = "QR-MALL-S03", Latitude = 10.7825, Longitude = 106.6942, Address = "Vincom Center, District 1, HCMC", TotalScans = 234, TotalOrders = 67 },
            new() { CampaignId = 4, Name = "Mall Food Court", Description = "Food court entrance standee", QrCode = "QR-MALL-FC01", Latitude = 10.7826, Longitude = 106.6943, Address = "Vincom Center, District 1, HCMC", TotalScans = 178, TotalOrders = 52 },
            new() { CampaignId = 5, Name = "Bus Stop District 3", Description = "Bus stop poster", QrCode = "QR-STREET-BS3", Latitude = 10.7850, Longitude = 106.6850, Address = "Vo Van Tan, District 3, HCMC", TotalScans = 45, TotalOrders = 5 },
        };
        db.QrLocations.AddRange(locations);
        db.SaveChanges();

        var products = new List<Product>
        {
            new() { Name = "Pho Bo", Description = "Traditional Vietnamese beef noodle soup", Price = 65000m, Category = "Food", ImageUrl = "/images/pho.jpg", IsActive = true },
            new() { Name = "Banh Mi", Description = "Vietnamese baguette sandwich with grilled pork", Price = 35000m, Category = "Food", ImageUrl = "/images/banhmi.jpg", IsActive = true },
            new() { Name = "Com Tam", Description = "Broken rice with grilled pork chop", Price = 55000m, Category = "Food", ImageUrl = "/images/comtam.jpg", IsActive = true },
            new() { Name = "Bun Cha", Description = "Grilled pork with rice noodles", Price = 60000m, Category = "Food", ImageUrl = "/images/buncha.jpg", IsActive = true },
            new() { Name = "Goi Cuon", Description = "Fresh spring rolls with shrimp", Price = 40000m, Category = "Food", ImageUrl = "/images/goicuon.jpg", IsActive = true },
            new() { Name = "Ca Phe Sua Da", Description = "Vietnamese iced coffee with condensed milk", Price = 29000m, Category = "Drinks", ImageUrl = "/images/caphe.jpg", IsActive = true },
            new() { Name = "Tra Da", Description = "Iced tea", Price = 10000m, Category = "Drinks", ImageUrl = "/images/trada.jpg", IsActive = true },
            new() { Name = "Sinh To Bo", Description = "Avocado smoothie", Price = 35000m, Category = "Drinks", ImageUrl = "/images/sinhto.jpg", IsActive = true },
            new() { Name = "Nuoc Mia", Description = "Fresh sugarcane juice", Price = 15000m, Category = "Drinks", ImageUrl = "/images/nuocmia.jpg", IsActive = true },
            new() { Name = "Che Ba Mau", Description = "Three-color dessert", Price = 25000m, Category = "Desserts", ImageUrl = "/images/chebamau.jpg", IsActive = true },
        };
        db.Products.AddRange(products);
        db.SaveChanges();

        var customers = new List<Customer>
        {
            new() { Name = "Nguyen Van An", Phone = "0901234567", Email = "an.nguyen@gmail.com", Address = "45 Tran Hung Dao, D1, HCMC", Latitude = 10.7700, Longitude = 106.6900, FirstVisitAt = DateTime.UtcNow.AddDays(-30), FirstOrderAt = DateTime.UtcNow.AddDays(-30), LastOrderAt = DateTime.UtcNow.AddDays(-2), TotalOrders = 5, TotalSpent = 325000m },
            new() { Name = "Tran Thi Binh", Phone = "0912345678", Email = "binh.tran@gmail.com", Address = "78 Nguyen Hue, D1, HCMC", Latitude = 10.7750, Longitude = 106.7000, FirstVisitAt = DateTime.UtcNow.AddDays(-20), FirstOrderAt = DateTime.UtcNow.AddDays(-18), LastOrderAt = DateTime.UtcNow.AddDays(-1), TotalOrders = 3, TotalSpent = 195000m },
            new() { Name = "Le Minh Cuong", Phone = "0923456789", Email = "cuong.le@gmail.com", Address = "12 Ly Tu Trong, D1, HCMC", Latitude = 10.7780, Longitude = 106.6950, FirstVisitAt = DateTime.UtcNow.AddDays(-15), FirstOrderAt = DateTime.UtcNow.AddDays(-15), LastOrderAt = DateTime.UtcNow.AddDays(-5), TotalOrders = 2, TotalSpent = 130000m },
            new() { Name = "Pham Duc Dung", Phone = "0934567890", Email = "dung.pham@gmail.com", Address = "56 Hai Ba Trung, D3, HCMC", Latitude = 10.7820, Longitude = 106.6920, FirstVisitAt = DateTime.UtcNow.AddDays(-10), FirstOrderAt = DateTime.UtcNow.AddDays(-10), LastOrderAt = DateTime.UtcNow.AddDays(-3), TotalOrders = 4, TotalSpent = 260000m },
            new() { Name = "Hoang Thu Em", Phone = "0945678901", Email = "em.hoang@gmail.com", Address = "89 Pasteur, D3, HCMC", Latitude = 10.7860, Longitude = 106.6880, FirstVisitAt = DateTime.UtcNow.AddDays(-7), FirstOrderAt = DateTime.UtcNow.AddDays(-7), LastOrderAt = DateTime.UtcNow.AddDays(-7), TotalOrders = 1, TotalSpent = 65000m },
        };
        db.Customers.AddRange(customers);
        db.SaveChanges();

        var devices = new List<TelecomDevice>
        {
            new() { DeviceCode = "DEVICE_01", ApiKey = "key-device-01-abc123def456", DeviceName = "Samsung Galaxy A14 - Office", PhoneNumber = "0981111111", Carrier = "Viettel", BatteryLevel = 82, NetworkType = "4G", IsOnline = true, LastHeartbeat = DateTime.UtcNow.AddSeconds(-15) },
            new() { DeviceCode = "DEVICE_02", ApiKey = "key-device-02-ghi789jkl012", DeviceName = "Xiaomi Redmi Note 12 - Warehouse", PhoneNumber = "0982222222", Carrier = "Mobifone", BatteryLevel = 65, NetworkType = "4G", IsOnline = true, LastHeartbeat = DateTime.UtcNow.AddSeconds(-30) },
            new() { DeviceCode = "DEVICE_03", ApiKey = "key-device-03-mno345pqr678", DeviceName = "Oppo A17 - Store", PhoneNumber = "0983333333", Carrier = "Vinaphone", BatteryLevel = 45, NetworkType = "3G", IsOnline = false, LastHeartbeat = DateTime.UtcNow.AddMinutes(-30) },
        };
        db.TelecomDevices.AddRange(devices);
        db.SaveChanges();

        var orders = new List<Order>
        {
            new() { OrderCode = "ORD20240601001", CustomerId = 1, QrLocationId = 1, TotalAmount = 100000m, Status = OrderStatus.Completed, CreatedAt = DateTime.UtcNow.AddDays(-5) },
            new() { OrderCode = "ORD20240602001", CustomerId = 2, QrLocationId = 4, TotalAmount = 95000m, Status = OrderStatus.Completed, CreatedAt = DateTime.UtcNow.AddDays(-4) },
            new() { OrderCode = "ORD20240603001", CustomerId = 3, QrLocationId = 8, TotalAmount = 65000m, Status = OrderStatus.Confirmed, CreatedAt = DateTime.UtcNow.AddDays(-3) },
            new() { OrderCode = "ORD20240604001", CustomerId = 4, QrLocationId = 6, TotalAmount = 130000m, Status = OrderStatus.Preparing, CreatedAt = DateTime.UtcNow.AddDays(-2) },
            new() { OrderCode = "ORD20240605001", CustomerId = 1, QrLocationId = 2, TotalAmount = 75000m, Status = OrderStatus.Pending, CreatedAt = DateTime.UtcNow.AddDays(-1) },
        };
        db.Orders.AddRange(orders);
        db.SaveChanges();

        var orderItems = new List<OrderItem>
        {
            new() { OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 65000m, TotalPrice = 65000m },
            new() { OrderId = 1, ProductId = 6, Quantity = 1, UnitPrice = 29000m, TotalPrice = 29000m },
            new() { OrderId = 2, ProductId = 3, Quantity = 1, UnitPrice = 55000m, TotalPrice = 55000m },
            new() { OrderId = 2, ProductId = 5, Quantity = 1, UnitPrice = 40000m, TotalPrice = 40000m },
            new() { OrderId = 3, ProductId = 1, Quantity = 1, UnitPrice = 65000m, TotalPrice = 65000m },
            new() { OrderId = 4, ProductId = 2, Quantity = 2, UnitPrice = 35000m, TotalPrice = 70000m },
            new() { OrderId = 4, ProductId = 4, Quantity = 1, UnitPrice = 60000m, TotalPrice = 60000m },
            new() { OrderId = 5, ProductId = 8, Quantity = 1, UnitPrice = 35000m, TotalPrice = 35000m },
            new() { OrderId = 5, ProductId = 5, Quantity = 1, UnitPrice = 40000m, TotalPrice = 40000m },
        };
        db.OrderItems.AddRange(orderItems);
        db.SaveChanges();

        var telecomJobs = new List<TelecomJob>
        {
            new() { DeviceId = 1, PhoneNumber = "0901234567", Message = "Thank you for your order.\n\nOrder code: ORD20240601001\n\nYour order has been received.", Priority = 1, Status = TelecomJobStatus.Sent, SentAt = DateTime.UtcNow.AddDays(-5) },
            new() { DeviceId = 1, PhoneNumber = "0912345678", Message = "Thank you for your order.\n\nOrder code: ORD20240602001\n\nYour order has been received.", Priority = 1, Status = TelecomJobStatus.Sent, SentAt = DateTime.UtcNow.AddDays(-4) },
            new() { DeviceId = 2, PhoneNumber = "0923456789", Message = "Thank you for your order.\n\nOrder code: ORD20240603001\n\nYour order has been received.", Priority = 1, Status = TelecomJobStatus.Sent, SentAt = DateTime.UtcNow.AddDays(-3) },
            new() { DeviceId = 2, PhoneNumber = "0934567890", Message = "Thank you for your order.\n\nOrder code: ORD20240604001\n\nYour order has been received.", Priority = 1, Status = TelecomJobStatus.Failed, RetryCount = 2 },
            new() { DeviceId = 1, PhoneNumber = "0901234567", Message = "Thank you for your order.\n\nOrder code: ORD20240605001\n\nYour order has been received.", Priority = 1, Status = TelecomJobStatus.Pending },
        };
        db.TelecomJobs.AddRange(telecomJobs);
        db.SaveChanges();

        var telecomLogs = new List<TelecomLog>
        {
            new() { TelecomJobId = 1, PhoneNumber = "0901234567", Message = "SMS sent successfully", Status = TelecomJobStatus.Sent, RawResponse = "OK" },
            new() { TelecomJobId = 2, PhoneNumber = "0912345678", Message = "SMS sent successfully", Status = TelecomJobStatus.Sent, RawResponse = "OK" },
            new() { TelecomJobId = 3, PhoneNumber = "0923456789", Message = "SMS sent successfully", Status = TelecomJobStatus.Sent, RawResponse = "OK" },
            new() { TelecomJobId = 4, PhoneNumber = "0934567890", Message = "SMS delivery failed", Status = TelecomJobStatus.Failed, RawResponse = "NETWORK_ERROR" },
            new() { TelecomJobId = 4, PhoneNumber = "0934567890", Message = "SMS delivery failed retry 1", Status = TelecomJobStatus.Failed, RawResponse = "NETWORK_ERROR" },
        };
        db.TelecomLogs.AddRange(telecomLogs);
        db.SaveChanges();

        var scanLogs = new List<QrScanLog>
        {
            new() { QrLocationId = 1, IpAddress = "192.168.1.100", UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 17_0)", Latitude = 10.7769, Longitude = 106.7009, CreatedAt = DateTime.UtcNow.AddDays(-5) },
            new() { QrLocationId = 4, IpAddress = "192.168.1.101", UserAgent = "Mozilla/5.0 (Linux; Android 14)", Latitude = 10.8231, Longitude = 106.6297, CreatedAt = DateTime.UtcNow.AddDays(-4) },
            new() { QrLocationId = 8, IpAddress = "10.0.0.55", UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 16_6)", Latitude = 10.7825, Longitude = 106.6942, CreatedAt = DateTime.UtcNow.AddDays(-3) },
            new() { QrLocationId = 6, IpAddress = "172.16.0.12", UserAgent = "Mozilla/5.0 (Linux; Android 13)", Latitude = 10.7500, Longitude = 106.6500, CreatedAt = DateTime.UtcNow.AddDays(-2) },
            new() { QrLocationId = 2, IpAddress = "192.168.2.50", UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 17_1)", Latitude = 10.7770, Longitude = 106.7010, CreatedAt = DateTime.UtcNow.AddDays(-1) },
        };
        db.QrScanLogs.AddRange(scanLogs);
        db.SaveChanges();
    }
}
