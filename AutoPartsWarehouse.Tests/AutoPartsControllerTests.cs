using AutoPartsWarehouse.Controllers;
using AutoPartsWarehouse.Data;
using AutoPartsWarehouse.Models;
using AutoPartsWarehouse.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AutoPartsWarehouse.Tests
{
    public class AutoPartsControllerTests
    {
        // Вспомогательный метод для создания чистой базы перед каждым тестом
        private ApplicationDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString()) // Уникальное имя для каждого теста
                .Options;
            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            return databaseContext;
        }

        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfAutoParts()
        {
            // Arrange
            using var context = GetDatabaseContext();
            context.AutoParts.Add(new AutoPart { Article = "1", Name = "Part1", PurchasePrice = 10, SalePrice = 20 });
            context.AutoParts.Add(new AutoPart { Article = "2", Name = "Part2", PurchasePrice = 10, SalePrice = 20 });
            await context.SaveChangesAsync();

            var controller = new AutoPartsController(context);

            // Act
            var result = await controller.Index(null, null, 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AutoPartsFilterViewModel>(viewResult.ViewData.Model);
            Assert.Equal(2, model.AutoParts.Count());
        }

        [Fact]
        public async Task Create_Redirects_ToIndex_WhenModelIsValid()
        {
            // Arrange
            using var context = GetDatabaseContext();
            var controller = new AutoPartsController(context);
            var newPart = new AutoPart { Article = "3", Name = "Part3", PurchasePrice = 100, SalePrice = 200 };

            // Act
            var result = await controller.Create(newPart);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(1, context.AutoParts.Count());
        }

        [Fact]
        public async Task Create_ReturnsView_WhenModelIsInvalid()
        {
            // Arrange
            using var context = GetDatabaseContext();
            var controller = new AutoPartsController(context);
            controller.ModelState.AddModelError("Name", "Required"); // Имитация ошибки валидации
            var newPart = new AutoPart { Article = "3" }; // Неполные данные

            // Act
            var result = await controller.Create(newPart);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(newPart, viewResult.Model);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            using var context = GetDatabaseContext();
            var controller = new AutoPartsController(context);

            // Act
            var result = await controller.Edit(id: null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenPartDoesNotExist()
        {
            // Arrange
            using var context = GetDatabaseContext();
            var controller = new AutoPartsController(context);

            // Act
            var result = await controller.Edit(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_UpdatesPart_AndRedirects_WhenValid()
        {
            // Arrange
            using var context = GetDatabaseContext();
            var part = new AutoPart { Article = "Old", Name = "OldName", PurchasePrice = 10, SalePrice = 20 };
            context.AutoParts.Add(part);
            await context.SaveChangesAsync();

            var controller = new AutoPartsController(context);

            // Detach, чтобы избежать конфликта трекинга EF при обновлении
            context.Entry(part).State = EntityState.Detached;

            var updatedPart = new AutoPart { Id = part.Id, Article = "New", Name = "NewName", PurchasePrice = 10, SalePrice = 20 };

            // Act
            var result = await controller.Edit(part.Id, updatedPart);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            var partInDb = await context.AutoParts.FindAsync(part.Id);
            Assert.Equal("NewName", partInDb.Name);
        }

        [Fact]
        public async Task DeleteConfirmed_DeletesPart_AndRedirects()
        {
            // Arrange
            using var context = GetDatabaseContext();
            var part = new AutoPart { Article = "Del", Name = "DeleteMe", PurchasePrice = 10, SalePrice = 20 };
            context.AutoParts.Add(part);
            await context.SaveChangesAsync();

            var controller = new AutoPartsController(context);

            // Act
            var result = await controller.DeleteConfirmed(part.Id);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(0, context.AutoParts.Count());
        }
    }
}