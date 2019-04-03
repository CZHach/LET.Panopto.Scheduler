using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFGetStarted.AspNetCore.ExistingDb.Models;
using EFGetStarted.AspNetCore.ExistingDb.Utilities;
using EFGetStarted.AspNetCore.ExistingDb.Scheduling;
using Newtonsoft.Json;

namespace EFGetStarted.AspNetCore.ExistingDb.Controllers
{
   
    public class EventSessionsController : Controller
    {
        private readonly NavEventsContext _context;
        ScheduleGenerator sg;
        ScheduleCreationInitiator sc;

        public EventSessionsController(NavEventsContext context)
        {
            _context = context;
            sg = new ScheduleGenerator(context);
        }

        public async Task<IActionResult> Index(DateTime? start, DateTime? end)
        {
            start = DatetimeGenerator.GetWeekStart();
            end = DatetimeGenerator.GetWeekEnd();

            List<GroupedEvents> playersGroupList = await sg.GenerateWeeklySchedule(start, end);
            ViewData["RangeStart"] = start;
            ViewData["RangeEnd"] = end;
            return View(playersGroupList);
        }

        [HttpPost]
        public async Task<IActionResult> SearchByDate(DateTime? start, DateTime? end)
        {
            List<GroupedEvents> playersGroupList = await sg.GenerateWeeklySchedule(start, end);
            ViewData["RangeStart"] = start;
            ViewData["RangeEnd"] = end;
            return View(playersGroupList);
        }

        // GET: EventSessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventSession = await _context.EventSession
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventSession == null)
            {
                return NotFound();
            }

            return View(eventSession);
        }

        // GET: EventSessions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EventSessions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SessionCourseName,SessionEventName,SessionDate,SessionStartDateTime,SessionEndDateTime")] EventSession eventSession)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventSession);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventSession);
        }

        // GET: EventSessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventSession = await _context.EventSession.FindAsync(id);
            if (eventSession == null)
            {
                return NotFound();
            }
            return View(eventSession);
        }

        // POST: EventSessions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SessionCourseName,SessionEventName,SessionDate,SessionStartDateTime,SessionEndDateTime")] EventSession eventSession)
        {
            if (id != eventSession.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventSession);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventSessionExists(eventSession.Id))
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
            return View(eventSession);
        }

        // GET: EventSessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventSession = await _context.EventSession
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventSession == null)
            {
                return NotFound();
            }

            return View(eventSession);
        }

        // POST: EventSessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventSession = await _context.EventSession.FindAsync(id);
            _context.EventSession.Remove(eventSession);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventSessionExists(int id)
        {
            return _context.EventSession.Any(e => e.Id == id);
        }
    }
}
