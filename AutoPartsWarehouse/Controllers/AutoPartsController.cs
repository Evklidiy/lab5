using AutoPartsWarehouse.Data;
using AutoPartsWarehouse.Models;
using AutoPartsWarehouse.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsWarehouse.Controllers
{
    [Authorize] // Требует аутентификацию
    public class AutoPartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AutoPartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Просмотр с фильтрацией и пагинацией
        public async Task<IActionResult> Index(string searchName, string searchManufacturer, int page = 1)
        {
            int pageSize = 5; // Количество на странице

            // 1. Фильтрация
            IQueryable<AutoPart> source = _context.AutoParts;

            if (!string.IsNullOrEmpty(searchName))
            {
                source = source.Where(p => p.Name.Contains(searchName));
            }
            if (!string.IsNullOrEmpty(searchManufacturer))
            {
                source = source.Where(p => p.Manufacturer.Contains(searchManufacturer));
            }

            // 2. Пагинация
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // 3. Формирование ViewModel
            var viewModel = new AutoPartsFilterViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                AutoParts = items,
                SearchName = searchName,
                SearchManufacturer = searchManufacturer
            };

            return View(viewModel);
        }

        // Создание
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AutoPart autoPart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(autoPart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(autoPart);
        }

        // Редактирование
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var autoPart = await _context.AutoParts.FindAsync(id);
            if (autoPart == null) return NotFound();
            return View(autoPart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AutoPart autoPart)
        {
            if (id != autoPart.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(autoPart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(autoPart);
        }

        // Удаление
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var autoPart = await _context.AutoParts.FirstOrDefaultAsync(m => m.Id == id);
            if (autoPart == null) return NotFound();
            return View(autoPart);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var autoPart = await _context.AutoParts.FindAsync(id);
            if (autoPart != null)
            {
                _context.AutoParts.Remove(autoPart);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}