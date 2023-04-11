using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContactManager.Data;
using ContactManager.Models;
using Microsoft.AspNetCore.Authorization;

namespace ContactManager.Controllers
{
    
    public class ContactManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ContactManager
        [Authorize(Roles = "Admin, Manager, User")]
        public async Task<IActionResult> Index()
        {
              return _context.ContactManagerDB != null ? 
                          View(await _context.ContactManagerDB.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ContactManagerDB'  is null.");
        }

        // GET: ContactManager/Details/5
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ContactManagerDB == null)
            {
                return NotFound();
            }

            var contactManagerEntity = await _context.ContactManagerDB
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactManagerEntity == null)
            {
                return NotFound();
            }

            return View(contactManagerEntity);
        }

        // GET: ContactManager/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        // POST: ContactManager/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,City,Email,State,Zip")] ContactManagerEntity contactManagerEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactManagerEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactManagerEntity);
        }

        // GET: ContactManager/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ContactManagerDB == null)
            {
                return NotFound();
            }

            var contactManagerEntity = await _context.ContactManagerDB.FindAsync(id);
            if (contactManagerEntity == null)
            {
                return NotFound();
            }
            return View(contactManagerEntity);
        }
        [Authorize(Roles = "Admin")]
        // POST: ContactManager/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,City,Email,State,Zip")] ContactManagerEntity contactManagerEntity)
        {
            if (id != contactManagerEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactManagerEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactManagerEntityExists(contactManagerEntity.Id))
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
            return View(contactManagerEntity);
        }

        // GET: ContactManager/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ContactManagerDB == null)
            {
                return NotFound();
            }

            var contactManagerEntity = await _context.ContactManagerDB
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactManagerEntity == null)
            {
                return NotFound();
            }

            return View(contactManagerEntity);
        }

        // POST: ContactManager/Delete/5
        [Authorize(Roles = "Admin")]

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ContactManagerDB == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ContactManagerDB'  is null.");
            }
            var contactManagerEntity = await _context.ContactManagerDB.FindAsync(id);
            if (contactManagerEntity != null)
            {
                _context.ContactManagerDB.Remove(contactManagerEntity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactManagerEntityExists(int id)
        {
          return (_context.ContactManagerDB?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
