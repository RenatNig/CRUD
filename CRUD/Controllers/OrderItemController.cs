using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUD.Data;
using CRUD.Models;

namespace CRUD.Controllers
{
    public class OrderItemController : Controller
    {
        private readonly EFDbContext _context;

        public OrderItemController(EFDbContext context)
        {
            _context = context;
        }

        // GET: OrderItem
        public async Task<IActionResult> Index()
        {
            var eFDbContext = _context.OrderItem.Include(o => o.Order);
            return View(await eFDbContext.ToListAsync());
        }

        // GET: OrderItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrderItem == null)
            {
                return NotFound();
            } 

            var orderItem = await _context.OrderItem
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // GET: OrderItem/Create
        public IActionResult Create(int orderId)
        {
            //ViewData["OrderId"] = orderId;
            OrderItem item = new OrderItem();
            item.OrderId = orderId;
            return View(item);
        }

        // POST: OrderItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderItem orderItem)
        {
            var checkOrder = _context.Order
                .Any(n => n.Id == orderItem.OrderId && n.Number == orderItem.Name);

            if (ModelState.IsValid)
           {
               if (!checkOrder)
               {
                   _context.Add(orderItem);
                   await _context.SaveChangesAsync();
                   return RedirectToAction("Edit", "Order", new {id = orderItem.OrderId});
               }
               else
               {
                   var order = _context.Order
                       .First(n => n.Id == orderItem.OrderId && n.Number == orderItem.Name);

                   ModelState.AddModelError(nameof(orderItem.Name),
                       "Наименование товара не может быть равно номеру заказа (" + order.Number + ")");
                   ViewData["Providers"] = new SelectList(_context.Provider, "Id", "Name");
                   //ViewData["OrderId"] = ;
                   return View(orderItem);
               }
           }
            return View(orderItem);
        }

        // GET: OrderItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrderItem == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItem.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "Id", orderItem.OrderId);
            return View(orderItem);
        }

        // POST: OrderItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderId,Name,Quantity,Unit")] OrderItem orderItem)
        {
            if (id != orderItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderItemExists(orderItem.Id))
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
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "Id", orderItem.OrderId);
            return View(orderItem);
        }

        // GET: OrderItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrderItem == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItem
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // POST: OrderItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrderItem == null)
            {
                return Problem("Entity set 'EFDbContext.OrderItem'  is null.");
            }
            var orderItem = await _context.OrderItem.FindAsync(id);
            if (orderItem != null)
            {
                _context.OrderItem.Remove(orderItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit","Order", new { id = orderItem.OrderId });
        }

        private bool OrderItemExists(int id)
        {
          return (_context.OrderItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
