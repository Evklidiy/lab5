using AutoPartsWarehouse.Controllers;
using AutoPartsWarehouse.Data;
using AutoPartsWarehouse.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AutoPartsWarehouse.Tests
{
    public class SuppliesControllerTests
    {
        private ApplicationDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            return databaseContext;
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithSupplies()
        {
            // Arrange
            using var context = GetDatabaseContext();

            // 1. Сначала создаем поставщика
            var supplier = new Supplier { Name = "Test Supplier", Rating = 10 };
            context.Suppliers.Add(supplier);
            await context.SaveChangesAsync();

            // 2. Создаем поставку, привязанную к этому поставщику
            context.Supplies.Add(new Supply
            {
                Date = DateTime.Now,
                Status = "Ожидается",
                SupplierId = supplier.Id // Важно: привязываем ID
            });
            await context.SaveChangesAsync();

            var controller = new SuppliesController(context);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<System.Collections.Generic.IEnumerable<Supply>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Create_AddsSupply_AndRedirects()
        {
            // Arrange
            using var context = GetDatabaseContext();
            var controller = new SuppliesController(context);
            var newSupply = new Supply { Date = DateTime.Now, Status = "Ожидается", SupplierId = 1 };

            // Act
            var result = await controller.Create(newSupply);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(1, await context.Supplies.CountAsync());
        }

        [Fact]
        public async Task Edit_ChangesStatus_ToReceived()
        {
            // Arrange
            using var context = GetDatabaseContext();
            var supply = new Supply { Date = DateTime.Now, Status = "Ожидается" };
            context.Supplies.Add(supply);
            await context.SaveChangesAsync();

            var controller = new SuppliesController(context);
            context.Entry(supply).State = EntityState.Detached;

            var updatedSupply = new Supply { Id = supply.Id, Date = supply.Date, Status = "Получена" };

            // Act
            var result = await controller.Edit(supply.Id, updatedSupply);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            var supplyInDb = await context.Supplies.FindAsync(supply.Id);
            Assert.Equal("Получена", supplyInDb.Status);
        }
    }
}