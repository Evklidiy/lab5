using AutoPartsWarehouse.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AutoPartsWarehouse.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            // 1. Создание ролей
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. Создание Админа (если нет)
            var adminEmail = "admin@lab5.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                var result = await userManager.CreateAsync(admin, "AdminPass");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            // 3. Заполнение данными (если база пуста)
            if (context.AutoParts.Any())
            {
                return;
            }

            // Поставщики
            var suppliers = new Supplier[]
            {
                new Supplier { Name="ООО АвтоОпт", ContactInfo="opt@auto.com", Rating=9 },
                new Supplier { Name="ИП Иванов", ContactInfo="+79990000000", Rating=7 },
                new Supplier { Name="Japan Parts Ltd", ContactInfo="jp@parts.com", Rating=10 },
                new Supplier { Name="ЕвроДеталь", ContactInfo="euro@detal.ru", Rating=8 },
                new Supplier { Name="КитайПром", ContactInfo="china@parts.cn", Rating=5 }
            };
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            // Запчасти (Больше данных)
            var parts = new AutoPart[]
            {
                new AutoPart { Article="A001", Name="Масляный фильтр", Manufacturer="Bosch", PurchasePrice=300, SalePrice=500, CompatibleModels="Toyota, Mazda" },
                new AutoPart { Article="B002", Name="Тормозные колодки", Manufacturer="Brembo", PurchasePrice=1200, SalePrice=2000, CompatibleModels="BMW, Audi" },
                new AutoPart { Article="C003", Name="Свеча зажигания", Manufacturer="NGK", PurchasePrice=150, SalePrice=300, CompatibleModels="Lada, Renault" },
                new AutoPart { Article="D004", Name="Амортизатор передний", Manufacturer="KYB", PurchasePrice=2500, SalePrice=3800, CompatibleModels="Toyota Camry" },
                new AutoPart { Article="E005", Name="Ремень ГРМ", Manufacturer="Contitech", PurchasePrice=800, SalePrice=1500, CompatibleModels="Ford Focus" },
                new AutoPart { Article="F006", Name="Лампа H7", Manufacturer="Osram", PurchasePrice=200, SalePrice=450, CompatibleModels="Universal" },
                new AutoPart { Article="G007", Name="Аккумулятор 60Ah", Manufacturer="Varta", PurchasePrice=4000, SalePrice=5500, CompatibleModels="Universal" },
                new AutoPart { Article="H008", Name="Фильтр воздушный", Manufacturer="Mann", PurchasePrice=400, SalePrice=700, CompatibleModels="Volkswagen Polo" },
                new AutoPart { Article="I009", Name="Стойка стабилизатора", Manufacturer="CTR", PurchasePrice=600, SalePrice=1100, CompatibleModels="Hyundai Solaris" },
                new AutoPart { Article="J010", Name="Масло моторное 5W30", Manufacturer="Shell", PurchasePrice=1800, SalePrice=2500, CompatibleModels="Universal" }
            };
            context.AutoParts.AddRange(parts);
            context.SaveChanges();

            // Поставки
            var supplies = new Supply[]
            {
                new Supply { Date=DateTime.Now.AddDays(-10), Status="Получена", SupplierId=suppliers[0].Id },
                new Supply { Date=DateTime.Now.AddDays(-5), Status="Получена", SupplierId=suppliers[2].Id },
                new Supply { Date=DateTime.Now.AddDays(-2), Status="Получена", SupplierId=suppliers[1].Id },
                new Supply { Date=DateTime.Now.AddDays(2), Status="Ожидается", SupplierId=suppliers[3].Id },
                new Supply { Date=DateTime.Now.AddDays(5), Status="Ожидается", SupplierId=suppliers[4].Id }
            };
            context.Supplies.AddRange(supplies);
            context.SaveChanges();
        }
    }
}