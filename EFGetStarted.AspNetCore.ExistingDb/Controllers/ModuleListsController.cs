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
    public class ModuleListsController : Controller
    {
        private readonly NavEventsContext _context;

        public ModuleListsController(NavEventsContext context)
        {
            _context = context;
        }

        // GET: ModuleLists
        public async Task<IActionResult> Index()
        {
            var eventDetails = await _context.ModuleList
                .Where(e => e.MediasiteCatalogId == null
                        && e.PublishingStatus == 1
                        && !e.IsPlaceholder).ToListAsync();

            return View(eventDetails);
        }

        // GET: ModuleLists
        public async Task<IActionResult> ClassOfGrouping(int classOf)
        {
            var eventDetails = await (
                                    from modules in _context.ModuleList
                                        join curricula in _context.ModuleCurricula on modules.ModuleId equals curricula.ModuleId
                                        join groups in _context.Groups on curricula.GroupId equals groups.GroupId
                                    into NewList
                                    from cg in NewList.DefaultIfEmpty()
                                        where modules.MediasiteCatalogId == null
                                        && modules.PublishingStatus == 1
                                        && !modules.IsPlaceholder
                                        && cg.ClassYear == classOf
                                    select new ModulesByClassYear
                                        {
                                            Id = modules.ModuleId,
                                            Title = modules.ModuleDisplayName,
                                            ClassYear = cg.ClassYear,
                                            MediasiteCatalogId = modules.MediasiteCatalogId
                                        }).ToListAsync();

            return View(eventDetails);
        }

        // GET: ModuleLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moduleList = await _context.ModuleList
                .FirstOrDefaultAsync(m => m.ModuleId == id);
            if (moduleList == null)
            {
                return NotFound();
            }

            return View(moduleList);
        }

        // GET: ModuleLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ModuleLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModuleId,ModuleDisplayName,Keywords,ToolType,ModuleDescription,ModuleStartDate,ModuleEndDate,OwnerId,OptionType,ShowStartDate,ShowEndDate,IsLocked,QuestionsPerPage,RandomizeQuestions,PullQuestionCount,AcademicYear,IsPlaceholder,BlogId,IsClerkship,IsMediaCollection,IsCloneReady,ClerkshipDurationInDays,DisciplineId,DepartmentId,AssessmentType,IsGated,AvailableQuestionAttempts,ShowsScoreAtEnd,ShowsFeedbackAtEnd,AllowsShowAllAnswers,PublishingStatus,IsShrouded,HasCustomAcl,CiYear,TimeAllotted,MediasiteCatalogId,AllowsMedia,EventCalendarId,SchedulerId,OmedCourseId,RecurringCourseId,ShowCompletionReport")] ModuleList moduleList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(moduleList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(moduleList);
        }

        // GET: ModuleLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moduleList = await _context.ModuleList.FindAsync(id);
            if (moduleList == null)
            {
                return NotFound();
            }
            return View(moduleList);
        }

        // POST: ModuleLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModuleId,ModuleDisplayName,Keywords,ToolType,ModuleDescription,ModuleStartDate,ModuleEndDate,OwnerId,OptionType,ShowStartDate,ShowEndDate,IsLocked,QuestionsPerPage,RandomizeQuestions,PullQuestionCount,AcademicYear,IsPlaceholder,BlogId,IsClerkship,IsMediaCollection,IsCloneReady,ClerkshipDurationInDays,DisciplineId,DepartmentId,AssessmentType,IsGated,AvailableQuestionAttempts,ShowsScoreAtEnd,ShowsFeedbackAtEnd,AllowsShowAllAnswers,PublishingStatus,IsShrouded,HasCustomAcl,CiYear,TimeAllotted,MediasiteCatalogId,AllowsMedia,EventCalendarId,SchedulerId,OmedCourseId,RecurringCourseId,ShowCompletionReport")] ModuleList moduleList)
        {
            if (id != moduleList.ModuleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(moduleList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleListExists(moduleList.ModuleId))
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
            return View(moduleList);
        }

        // GET: ModuleLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moduleList = await _context.ModuleList
                .FirstOrDefaultAsync(m => m.ModuleId == id);
            if (moduleList == null)
            {
                return NotFound();
            }

            return View(moduleList);
        }

        // POST: ModuleLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var moduleList = await _context.ModuleList.FindAsync(id);
            _context.ModuleList.Remove(moduleList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModuleListExists(int id)
        {
            return _context.ModuleList.Any(e => e.ModuleId == id);
        }
    }
}
