using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFGetStarted.AspNetCore.ExistingDb.Models;

namespace EFGetStarted.AspNetCore.ExistingDb.Controllers
{
    public class PageListsController : Controller
    {
        private readonly NavEventsContext _context;

        public PageListsController(NavEventsContext context)
        {
            _context = context;
        }

        // GET: PageLists
        public async Task<IActionResult> Index()
        {
            var navEventsContext = _context.PageList.Include(p => p.Folder);
            return View(await navEventsContext.ToListAsync());
        }

        // GET: PageLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pageList = await _context.PageList
                .Include(p => p.Folder)
                .FirstOrDefaultAsync(m => m.PageId == id);
            if (pageList == null)
            {
                return NotFound();
            }

            return View(pageList);
        }

        // GET: PageLists/Create
        public IActionResult Create()
        {
            ViewData["FolderId"] = new SelectList(_context.FolderList, "FolderId", "FolderId");
            return View();
        }

        // POST: PageLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FolderId,PageId,PageDisplayName,PageSequence,PageStartTime,PageEndTime,Hidden,DateCreated,DateUpdated,ShowStartDate,ShowEndDate,PageKeywords,BodyContent,LinkLibraryId,PageTypeId,HasCustomAcl,ObjectiveId")] PageList pageList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pageList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FolderId"] = new SelectList(_context.FolderList, "FolderId", "FolderId", pageList.FolderId);
            return View(pageList);
        }

        // GET: PageLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pageList = await _context.PageList.FindAsync(id);
            if (pageList == null)
            {
                return NotFound();
            }
            ViewData["FolderId"] = new SelectList(_context.FolderList, "FolderId", "FolderId", pageList.FolderId);
            return View(pageList);
        }

        // POST: PageLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FolderId,PageId,PageDisplayName,PageSequence,PageStartTime,PageEndTime,Hidden,DateCreated,DateUpdated,ShowStartDate,ShowEndDate,PageKeywords,BodyContent,LinkLibraryId,PageTypeId,HasCustomAcl,ObjectiveId")] PageList pageList)
        {
            if (id != pageList.PageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pageList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PageListExists(pageList.PageId))
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
            ViewData["FolderId"] = new SelectList(_context.FolderList, "FolderId", "FolderId", pageList.FolderId);
            return View(pageList);
        }

        // GET: PageLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pageList = await _context.PageList
                .Include(p => p.Folder)
                .FirstOrDefaultAsync(m => m.PageId == id);
            if (pageList == null)
            {
                return NotFound();
            }

            return View(pageList);
        }

        // POST: PageLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pageList = await _context.PageList.FindAsync(id);
            _context.PageList.Remove(pageList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PageListExists(int id)
        {
            return _context.PageList.Any(e => e.PageId == id);
        }
    }
}
