using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LET.Panopto.Scheduler.Models;

namespace LET.Panopto.Scheduler.Controllers
{
    public class FolderListsController : Controller
    {
        private readonly NavEventsContext _context;

        public FolderListsController(NavEventsContext context)
        {
            _context = context;
        }

        // GET: FolderLists
        public async Task<IActionResult> Index()
        {
            var navEventsContext = _context.FolderList.Include(f => f.Module);
            return View(await navEventsContext.ToListAsync());
        }

        // GET: FolderLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var folderList = await _context.FolderList
                .Include(f => f.Module)
                .FirstOrDefaultAsync(m => m.FolderId == id);
            if (folderList == null)
            {
                return NotFound();
            }

            return View(folderList);
        }

        // GET: FolderLists/Create
        public IActionResult Create()
        {
            ViewData["ModuleId"] = new SelectList(_context.ModuleList, "ModuleId", "ModuleDisplayName");
            return View();
        }

        // POST: FolderLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModuleId,FolderId,FolderSequence,FolderDisplayName,FolderDateTimeStart,Hidden,ShowStartDate,ShowEndDate,HasCustomAcl")] FolderList folderList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(folderList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModuleId"] = new SelectList(_context.ModuleList, "ModuleId", "ModuleDisplayName", folderList.ModuleId);
            return View(folderList);
        }

        // GET: FolderLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var folderList = await _context.FolderList.FindAsync(id);
            if (folderList == null)
            {
                return NotFound();
            }
            ViewData["ModuleId"] = new SelectList(_context.ModuleList, "ModuleId", "ModuleDisplayName", folderList.ModuleId);
            return View(folderList);
        }

        // POST: FolderLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModuleId,FolderId,FolderSequence,FolderDisplayName,FolderDateTimeStart,Hidden,ShowStartDate,ShowEndDate,HasCustomAcl")] FolderList folderList)
        {
            if (id != folderList.FolderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(folderList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FolderListExists(folderList.FolderId))
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
            ViewData["ModuleId"] = new SelectList(_context.ModuleList, "ModuleId", "ModuleDisplayName", folderList.ModuleId);
            return View(folderList);
        }

        // GET: FolderLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var folderList = await _context.FolderList
                .Include(f => f.Module)
                .FirstOrDefaultAsync(m => m.FolderId == id);
            if (folderList == null)
            {
                return NotFound();
            }

            return View(folderList);
        }

        // POST: FolderLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var folderList = await _context.FolderList.FindAsync(id);
            _context.FolderList.Remove(folderList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FolderListExists(int id)
        {
            return _context.FolderList.Any(e => e.FolderId == id);
        }
    }
}
