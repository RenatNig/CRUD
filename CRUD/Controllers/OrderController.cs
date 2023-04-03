using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUD.Data;
using CRUD.Data.Abstract;
using CRUD.Models;
using CRUD.ViewModels;

namespace CRUD.Controllers
{
    public class OrderController : Controller
    {
        //private readonly EFDbContext _context;
        private EFDbContext _context;

        public OrderController(EFDbContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index(string sortOrder, DateTime? startDate, DateTime? endDate, string? selectedNumber, int? selectedProvider)
        {
            ViewData["NumSortPar"] = String.IsNullOrEmpty(sortOrder) ? "num_desc" : "";
            ViewData["DateSortPar"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["StartDate"] = startDate;
            ViewData["EndDate"] = endDate;

            ViewData["Numbers"] = new SelectList(_context.Order
                .GroupBy(c=>c.Number)
                .Select(s => s.First()), "Id", "Number");
            
            ViewData["Providers"] = new SelectList(_context.Provider
                .GroupBy(c=>c.Name)
                .Select(s => s.First()), "Id", "Name");
            
            ViewData["Items"] = new SelectList(_context.OrderItem
                .GroupBy(c=>c.Name)
                .Select(s => s.First()), "Id", "Name");
            
            ViewData["Units"] = new SelectList(_context.OrderItem
                .GroupBy(c=>c.Unit)
                .Select(s => s.First()), "Id", "Unit");
            
            IQueryable<Order> orders = _context.Order
                .Include(o => o.Provider);
            
            if (startDate != null && endDate != null)
            {
                orders = orders.Where(s => s.Date < endDate 
                && s.Date > startDate);
            }
            
            if (selectedNumber != null)
            {
                orders = orders.Where(s => s.Number == selectedNumber);
            }
            
            if (selectedProvider != null)
            {
                orders = orders.Where(s => s.ProviderId == selectedProvider);
            }
            
            switch (sortOrder)
            {
                case "num_desc":
                    orders = orders.OrderByDescending(s => s.Number);
                    break;
                case "Date":
                    orders = orders.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    orders = orders.OrderByDescending(s => s.Date);
                    break;
                default:
                    orders = orders.OrderBy(s => s.Number);
                    break;
            }
            return View(await orders.AsNoTracking().ToListAsync());
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            OrderItemsViewModel orderAndItems = new OrderItemsViewModel()
            {
                Order = _context.Order
                    .Include(o => o.Provider)
                    .FirstOrDefault(m => m.Id == id),
                OrderItems = _context.OrderItem
                    .Where(s=>s.OrderId == id).ToList()
            };
            //orderAndItems.OrderItems = orderAndItems.OrderItems.Where(s => s.OrderId == id);
            if (orderAndItems.Order == null)
            {
                return NotFound();
            }

            return View(orderAndItems);
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            ViewData["Providers"] = new SelectList(_context.Provider, "Id", "Name");
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            var orderCheck = _context.Order
                .Any(n => n.Number == order.Number && n.ProviderId == order.ProviderId);
            
            if (ModelState.IsValid)
            {
                if (!orderCheck)
                {
                    _context.Add(order);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(nameof(order.Number), "У данного поставщика уже есть заказ с таким номером");
                    ViewData["Providers"] = new SelectList(_context.Provider, "Id", "Name");
                    return View(order);
                }
            }
            return View(order);
        }
        
        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["Providers"] = new SelectList(_context.Provider, "Id", "Name");
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            OrderItemsViewModel orderAndItems = new OrderItemsViewModel()
            {
                Order = _context.Order
                    .FirstOrDefault(m => m.Id == id),
                OrderItems = _context.OrderItem
                    .Where(s=>s.OrderId == id).ToList()
            };
            if (orderAndItems.Order == null)
            {
                return NotFound();
            }
            return View(orderAndItems);
        }
        
        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderItemsViewModel orderAndItems)
        {
            if (id != orderAndItems.Order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderAndItems.Order);
                    await _context.SaveChangesAsync();

                    foreach (var o in orderAndItems.OrderItems)
                    {
                        _context.Update(o);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(orderAndItems.Order.Id))
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
            return RedirectToAction(nameof(Index));
            //return View(orderAndItems);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = _context.Order
                .FirstOrDefault(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order == null)
            {
                return Problem("Entity set 'CRUDContext.Order'  is null.");
            }
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return (_context.Order?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
