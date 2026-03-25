using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ComputerParts.Data;
using ComputerParts.Models;
using Microsoft.AspNetCore.Authorization;

namespace ComputerParts.Controllers
{
    public class HardwareComponentsController : Controller
    {
        private readonly ComputerPartsContext _context;

        public HardwareComponentsController(ComputerPartsContext context)
        {
            _context = context;
        }

        // Всички могат да гледат списъка
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            var components = from c in _context.HardwareComponent select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                bool isPrice = decimal.TryParse(searchString, out decimal searchPrice);

                components = components.Where(s => s.Name!.Contains(searchString)
                                               || s.Manufacturer!.Contains(searchString)
                                               || (isPrice && s.Price == searchPrice));
            }

            ViewData["PriceSortParm"] = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";

            switch (sortOrder)
            {
                case "price_desc":
                    components = components.OrderByDescending(s => s.Price);
                    break;
                default:
                    components = components.OrderBy(s => s.Price);
                    break;
            }

            return View(await components.ToListAsync());
        }

        // Всички могат да гледат детайлите
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hardwareComponent = await _context.HardwareComponent
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hardwareComponent == null)
            {
                return NotFound();
            }

            return View(hardwareComponent);
        }

        // --- САМО ЗА АДМИНИСТРАТОРИ ---

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Manufacturer,Type,Price,ImageUrl")] HardwareComponent hardwareComponent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hardwareComponent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hardwareComponent);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hardwareComponent = await _context.HardwareComponent.FindAsync(id);
            if (hardwareComponent == null)
            {
                return NotFound();
            }
            return View(hardwareComponent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Manufacturer,Type,Price,ImageUrl")] HardwareComponent hardwareComponent)
        {
            if (id != hardwareComponent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hardwareComponent);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Промените са записани успешно!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HardwareComponentExists(hardwareComponent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(hardwareComponent);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hardwareComponent = await _context.HardwareComponent
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hardwareComponent == null)
            {
                return NotFound();
            }

            return View(hardwareComponent);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hardwareComponent = await _context.HardwareComponent.FindAsync(id);
            if (hardwareComponent != null)
            {
                _context.HardwareComponent.Remove(hardwareComponent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HardwareComponentExists(int id)
        {
            return _context.HardwareComponent.Any(e => e.Id == id);
        }
    }
}