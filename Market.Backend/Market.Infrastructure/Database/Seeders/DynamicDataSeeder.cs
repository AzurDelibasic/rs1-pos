using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Catalog;
using Market.Domain.Entities.Finance;
using Market.Domain.Entities.Identity;
using Market.Domain.Entities.Shifts;
using Market.Domain.Entities.Venue;

namespace Market.Infrastructure.Database.Seeders;

/// <summary>
/// Dynamic seeder koji se pokreće u runtime-u,
/// obično pri startu aplikacije (npr. u Program.cs).
/// Koristi se za unos demo/test podataka koji nisu dio migracije.
/// </summary>
public static class DynamicDataSeeder
{
    public static async Task SeedAsync(DatabaseContext context)
    {
        // Šema baze se ne kreira ovdje: DatabaseInitializer prije seed-a pokreće migracije
        // (SQL Server) ili EnsureCreated (InMemory baza u testovima).

        await SeedProductCategoriesAsync(context);
        await SeedUsersAsync(context);
        await SeedProductsAsync(context);
        await SeedExchangeRatesAsync(context);
        await SeedRegionsAsync(context);
        await SeedTablesAsync(context);
        await SeedCashRegistersAsync(context);
    }

    private static async Task SeedProductCategoriesAsync(DatabaseContext context)
    {
        if (!await context.ProductCategories.AnyAsync())
        {
            context.ProductCategories.AddRange(
                new ProductCategoryEntity
                {
                    Name = "Računari",
                    IsEnabled = true,
                    CreatedAtUtc = DateTime.UtcNow
                },
                new ProductCategoryEntity
                {
                    Name = "Mobilni uređaji",
                    IsEnabled = true,
                    CreatedAtUtc = DateTime.UtcNow
                },
                new ProductCategoryEntity
                {
                    Name = "Periferija",
                    IsEnabled = true,
                    CreatedAtUtc = DateTime.UtcNow
                },
                new ProductCategoryEntity
                {
                    Name = "Komponente",
                    IsEnabled = true,
                    CreatedAtUtc = DateTime.UtcNow
                },
                new ProductCategoryEntity
                {
                    Name = "Audio oprema",
                    IsEnabled = false,
                    CreatedAtUtc = DateTime.UtcNow
                }
            );
            await context.SaveChangesAsync();
            Console.WriteLine("✅ Dynamic seed: product categories added.");
        }
    }

    /// <summary>
    /// Kreira demo korisnike ako ih još nema u bazi.
    /// Uloge i IsEnabled se postavljaju eksplicitno (baza nema default vrijednosti za njih).
    /// </summary>
    private static async Task SeedUsersAsync(DatabaseContext context)
    {
        if (await context.Users.AnyAsync())
            return;

        var hasher = new PasswordHasher<MarketUserEntity>();

        var admin = new MarketUserEntity
        {
            Firstname = "Admin",
            Lastname = "Market",
            Email = "admin@market.local",
            PasswordHash = hasher.HashPassword(null!, "Admin123!"),
            Address = "Maršala Tita 1",
            City = "Sarajevo",
            IsAdmin = true,
            IsManager = false,
            IsEmployee = false,
            IsEnabled = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        var manager = new MarketUserEntity
        {
            Firstname = "Manager",
            Lastname = "Market",
            Email = "manager@market.local",
            PasswordHash = hasher.HashPassword(null!, "Manager123!"),
            Address = "Ferhadija 12",
            City = "Sarajevo",
            IsAdmin = false,
            IsManager = true,
            IsEmployee = false,
            IsEnabled = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        var employee = new MarketUserEntity
        {
            Firstname = "Employee",
            Lastname = "Market",
            Email = "employee@market.local",
            PasswordHash = hasher.HashPassword(null!, "Employee123!"),
            Address = "Kralja Tvrtka 5",
            City = "Zenica",
            IsAdmin = false,
            IsManager = false,
            IsEmployee = true,
            IsEnabled = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        var dummyForSwagger = new MarketUserEntity
        {
            Firstname = "Swagger",
            Lastname = "Demo",
            Email = "string",
            PasswordHash = hasher.HashPassword(null!, "string"),
            Address = "Zmaja od Bosne 8",
            City = "Sarajevo",
            IsAdmin = false,
            IsManager = false,
            IsEmployee = true,
            IsEnabled = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        var dummyForTests = new MarketUserEntity
        {
            Firstname = "Test",
            Lastname = "User",
            Email = "test",
            PasswordHash = hasher.HashPassword(null!, "test123"),
            Address = "Titova 3",
            City = "Tuzla",
            IsAdmin = false,
            IsManager = false,
            IsEmployee = true,
            IsEnabled = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        context.Users.AddRange(
            admin,
            manager,
            employee,
            dummyForSwagger,
            dummyForTests
        );

        await context.SaveChangesAsync();
        Console.WriteLine("✅ Dynamic seed: demo users added.");
    }

    /// <summary>
    /// Kreira demo proizvode ako ih još nema u bazi.
    /// </summary>
    private static async Task SeedProductsAsync(DatabaseContext context)
    {
        if (await context.Products.AnyAsync())
            return;

        // Učitaj kategorije
        var categories = await context.ProductCategories
            .Where(c => c.IsEnabled)
            .ToListAsync();

        if (!categories.Any())
        {
            Console.WriteLine("⚠️  No categories found. Skipping product seed.");
            return;
        }

        var racunariCat = categories.FirstOrDefault(c => c.Name.Contains("Računari"));
        var mobilniCat = categories.FirstOrDefault(c => c.Name.Contains("Mobilni"));
        var periferijaCat = categories.FirstOrDefault(c => c.Name.Contains("Periferija"));
        var komponenteCat = categories.FirstOrDefault(c => c.Name.Contains("Komponente"));

        var products = new List<ProductEntity>();

        // Računari
        if (racunariCat != null)
        {
            products.AddRange(new[]
            {
                new ProductEntity
                {
                    Name = "Lenovo ThinkPad X1 Carbon Gen 11",
                    Description = "14\" FHD+ IPS, Intel i7-1365U, 16GB RAM, 512GB SSD, Windows 11 Pro",
                    Price = 2899.00m,
                    StockQuantity = 15,
                    CategoryId = racunariCat.Id,
                    IsEnabled = true,
                    CreatedAtUtc = DateTime.UtcNow.AddDays(-20)
                },
                new ProductEntity
                {
                    Name = "Dell XPS 15 9530",
                    Description = "15.6\" 4K OLED Touch, Intel i9-13900H, 32GB RAM, 1TB SSD, RTX 4070",
                    Price = 3499.00m,
                    StockQuantity = 8,
                    CategoryId = racunariCat.Id,
                    IsEnabled = true,
                    CreatedAtUtc = DateTime.UtcNow.AddDays(-18)
                },
                new ProductEntity
                {
                    Name = "HP Pavilion Gaming Desktop",
                    Description = "AMD Ryzen 7 5700G, 16GB RAM, 512GB SSD, RTX 3060 Ti, Windows 11",
                    Price = 1899.00m,
                    StockQuantity = 12,
                    CategoryId = racunariCat.Id,
                    IsEnabled = true,
                    CreatedAtUtc = DateTime.UtcNow.AddDays(-15)
                },
                new ProductEntity
                {
                    Name = "ASUS ROG Zephyrus G14",
                    Description = "14\" QHD 165Hz, AMD Ryzen 9 7940HS, 32GB RAM, 1TB SSD, RTX 4060",
                    Price = 2699.00m,
                    StockQuantity = 6,
                    CategoryId = racunariCat.Id,
                    IsEnabled = true,
                    CreatedAtUtc = DateTime.UtcNow.AddDays(-12)
                }
            });
        }

        // Mobilni uređaji
        if (mobilniCat != null)
        {
            products.AddRange(new[]
            {
                new ProductEntity
                {
                    Name = "Samsung Galaxy S24 Ultra",
                    Description = "6.8\" AMOLED 2X, 12GB RAM, 512GB, Snapdragon 8 Gen 3, S Pen",
                    Price = 1699.00m,
                    StockQuantity = 25,
                    CategoryId = mobilniCat.Id,
                    IsEnabled = true,
                    CreatedAtUtc = DateTime.UtcNow.AddDays(-10)
                },
                new ProductEntity
                {
                    Name = "Apple iPhone 15 Pro Max",
                    Description = "6.7\" Super Retina XDR, A17 Pro, 256GB, Titanium Design",
                    Price = 1899.00m,
                    StockQuantity = 18,
                    CategoryId = mobilniCat.Id,
                    IsEnabled = true,
                    CreatedAtUtc = DateTime.UtcNow.AddDays(-9)
                },
                new ProductEntity
                {
                    Name = "Google Pixel 8 Pro",
                    Description = "6.7\" LTPO OLED, Google Tensor G3, 12GB RAM, 256GB",
                    Price = 1299.00m,
                    StockQuantity = 20,
                    CategoryId = mobilniCat.Id,
                    IsEnabled = true,
                    CreatedAtUtc = DateTime.UtcNow.AddDays(-8)
                },
                new ProductEntity
                {
                    Name = "Xiaomi 14 Pro",
                    Description = "6.73\" AMOLED, Snapdragon 8 Gen 3, 12GB RAM, 512GB, Leica Camera",
                    Price = 1099.00m,
                    StockQuantity = 30,
                    CategoryId = mobilniCat.Id,
                    IsEnabled = true,
                    CreatedAtUtc = DateTime.UtcNow.AddDays(-7)
                }
            });
        }

        // Periferija
        if (periferijaCat != null)
        {
            products.AddRange(new[]
            {
                new ProductEntity
                {
                    Name = "Logitech MX Master 3S",
                    Description = "Wireless Performance Mouse, 8K DPI, USB-C, Bluetooth",
                    Price = 149.00m,
                    StockQuantity = 50,
                    CategoryId = periferijaCat.Id,
                    IsEnabled = true,
                    CreatedAtUtc = DateTime.UtcNow.AddDays(-6)
                },
                new ProductEntity
                {
                    Name = "Keychron K8 Pro Mechanical Keyboard",
                    Description = "Wireless TKL, Hot-swappable, RGB Backlight, Gateron Switch",
                    Price = 189.00m,
                    StockQuantity = 35,
                    CategoryId = periferijaCat.Id,
                    IsEnabled = true,
                    CreatedAtUtc = DateTime.UtcNow.AddDays(-5)
                },
                new ProductEntity
                {
                    Name = "Dell UltraSharp U2723DE",
                    Description = "27\" QHD IPS Monitor, USB-C Hub, 90W Power Delivery",
                    Price = 749.00m,
                    StockQuantity = 22,
                    CategoryId = periferijaCat.Id,
                    IsEnabled = true,
                    CreatedAtUtc = DateTime.UtcNow.AddDays(-4)
                },
                new ProductEntity
                {
                    Name = "Blue Yeti USB Microphone",
                    Description = "Professional USB Condenser Microphone, Multiple Patterns",
                    Price = 169.00m,
                    StockQuantity = 40,
                    CategoryId = periferijaCat.Id,
                    IsEnabled = true,
                    CreatedAtUtc = DateTime.UtcNow.AddDays(-3)
                }
            });
        }

        // Komponente
        if (komponenteCat != null)
        {
            products.AddRange(new[]
            {
                new ProductEntity
                {
                    Name = "AMD Ryzen 9 7950X",
                    Description = "16-Core, 32-Thread Desktop Processor, 5.7 GHz Max Boost",
                    Price = 899.00m,
                    StockQuantity = 15,
                    CategoryId = komponenteCat.Id,
                    IsEnabled = true,
                    CreatedAtUtc = DateTime.UtcNow.AddDays(-2)
                },
                new ProductEntity
                {
                    Name = "NVIDIA GeForce RTX 4080",
                    Description = "16GB GDDR6X, Ray Tracing, DLSS 3, Ada Lovelace Architecture",
                    Price = 1899.00m,
                    StockQuantity = 10,
                    CategoryId = komponenteCat.Id,
                    IsEnabled = true,
                    CreatedAtUtc = DateTime.UtcNow.AddDays(-1)
                },
                new ProductEntity
                {
                    Name = "Corsair Vengeance DDR5 32GB",
                    Description = "2x16GB DDR5-6000 CL30, Intel XMP 3.0, AMD EXPO",
                    Price = 249.00m,
                    StockQuantity = 45,
                    CategoryId = komponenteCat.Id,
                    IsEnabled = true,
                    CreatedAtUtc = DateTime.UtcNow
                },
                new ProductEntity
                {
                    Name = "Samsung 990 PRO NVMe SSD 2TB",
                    Description = "PCIe 4.0, 7450/6900 MB/s Read/Write, V-NAND Technology",
                    Price = 299.00m,
                    StockQuantity = 38,
                    CategoryId = komponenteCat.Id,
                    IsEnabled = true,
                    CreatedAtUtc = DateTime.UtcNow
                }
            });
        }

        context.Products.AddRange(products);
        await context.SaveChangesAsync();

        Console.WriteLine($"✅ Dynamic seed: {products.Count} products added.");
    }

    /// <summary>
    /// Približni kursevi za USD, CHF, GBP i RSD na četiri datuma u 2026, da konverzija radi odmah nakon pokretanja.
    /// Valute i fiksni kurs EUR (1,95583) dolaze iz migracije (StaticDataSeeder).
    /// Nisu stvarni kursevi: u produkciji ih unosi administrator ili se preuzimaju preko ExchangeRates:Provider.
    /// </summary>
    private static async Task SeedExchangeRatesAsync(DatabaseContext context)
    {
        const string source = "Seed (približno)";

        if (await context.ExchangeRates.AnyAsync(r => r.Source == source))
            return;

        // KM za 1 jedinicu valute, redom za 01.01, 01.04, 01.07. i 01.10.2026.
        var approximateRates = new Dictionary<string, decimal[]>
        {
            ["USD"] = [1.6650m, 1.6800m, 1.6600m, 1.6750m],
            ["CHF"] = [2.0900m, 2.1000m, 2.0850m, 2.0950m],
            ["GBP"] = [2.2400m, 2.2550m, 2.2300m, 2.2450m],
            ["RSD"] = [0.016690m, 0.016685m, 0.016692m, 0.016688m]
        };

        DateTime[] validFromDates =
        [
            new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc)
        ];

        var codes = approximateRates.Keys.ToList();
        var currencyIdsByCode = await context.Currencies
            .Where(c => codes.Contains(c.Code))
            .ToDictionaryAsync(c => c.Code, c => c.Id);

        var rates = new List<ExchangeRateEntity>();
        foreach (var (code, values) in approximateRates)
        {
            if (!currencyIdsByCode.TryGetValue(code, out var currencyId))
                continue;

            for (var i = 0; i < validFromDates.Length; i++)
            {
                rates.Add(new ExchangeRateEntity
                {
                    CurrencyId = currencyId,
                    RateToBase = values[i],
                    ValidFromUtc = validFromDates[i],
                    Source = source,
                    CreatedAtUtc = DateTime.UtcNow
                });
            }
        }

        context.ExchangeRates.AddRange(rates);
        await context.SaveChangesAsync();

        Console.WriteLine($"✅ Dynamic seed: {rates.Count} approximate exchange rates added.");
    }

    private static async Task SeedRegionsAsync(DatabaseContext context)
    {
        if (await context.Regions.AnyAsync())
            return;

        context.Regions.AddRange(
            new RegionEntity { Name = "Sala", SortOrder = 1, IsActive = true },
            new RegionEntity { Name = "Šank", SortOrder = 2, IsActive = true },
            new RegionEntity { Name = "Terasa", SortOrder = 3, IsActive = true },
            new RegionEntity { Name = "Bašta", SortOrder = 4, IsActive = false });

        await context.SaveChangesAsync();

        Console.WriteLine("Dynamic seed: regions added.");
    }

    private static async Task SeedTablesAsync(DatabaseContext context)
    {
        if (await context.Tables.AnyAsync())
            return;

        var regions = await context.Regions
            .Where(r => r.IsActive)
            .ToDictionaryAsync(r => r.Name);

        var tables = new List<TableEntity>();

        if (regions.TryGetValue("Sala", out var hall))
        {
            for (int i = 1; i <= 6; i++)
                tables.Add(new TableEntity { Label = $"S{i}", Seats = 4, RegionId = hall.Id, IsActive = true });
        }

        if (regions.TryGetValue("Šank", out var bar))
        {
            for (int i = 1; i <= 4; i++)
                tables.Add(new TableEntity { Label = $"Š{i}", Seats = 2, RegionId = bar.Id, IsActive = true });
        }

        if (regions.TryGetValue("Terasa", out var terrace))
        {
            for (int i = 1; i <= 4; i++)
                tables.Add(new TableEntity { Label = $"T{i}", Seats = 6, RegionId = terrace.Id, IsActive = true });
        }

        if (tables.Count == 0)
            return;

        context.Tables.AddRange(tables);
        await context.SaveChangesAsync();

        Console.WriteLine("Dynamic seed: tables added.");
    }

    private static async Task SeedCashRegistersAsync(DatabaseContext context)
    {
        if (await context.CashRegisters.AnyAsync())
            return;

        context.CashRegisters.AddRange(
            new CashRegisterEntity { Name = "Kasa 1 - Sala", IsActive = true },
            new CashRegisterEntity { Name = "Kasa 2 - Šank", IsActive = true },
            new CashRegisterEntity { Name = "Kasa 3 - Terasa", IsActive = false });

        await context.SaveChangesAsync();

        Console.WriteLine("Dynamic seed: cash registers added.");
    }
}