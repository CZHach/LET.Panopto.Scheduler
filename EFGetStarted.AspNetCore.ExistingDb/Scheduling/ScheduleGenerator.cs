using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using EFGetStarted.AspNetCore.ExistingDb.Models;
using EFGetStarted.AspNetCore.ExistingDb.Utilities;

namespace EFGetStarted.AspNetCore.ExistingDb.Scheduling
{
    public interface IScheduleGenerator
    {
        Task<List<GroupedEvents>> GenerateWeeklySchedule(DateTime? start, DateTime? end);
    }

    public sealed class ScheduleGenerator : IScheduleGenerator
    {
        private readonly NavEventsContext _context;

        public ScheduleGenerator(NavEventsContext context)
        {
            _context = context;
        }

        public async Task<List<GroupedEvents>>  GenerateWeeklySchedule(DateTime? start, DateTime? end)
        {
            var eventCourses = await _context.ModuleList
                         .Where(m => m.MediasiteCatalogId != null
                                 && m.PublishingStatus == 1
                                 && m.AcademicYear > DateTime.Now.Year - 2
                                 && !m.IsPlaceholder).ToListAsync();

            var eventPages = await _context.PageList
                .Where(p => p.PageTypeId == 668
                && p.PageStartTime != null
                && p.PageEndTime != null)
                .OrderBy(p => p.PageStartTime).ToListAsync();

            var eventFolders = await _context.FolderList
                .Where(f => f.FolderDateTimeStart >= start
                && f.FolderDateTimeStart <= end).ToListAsync();

            var queryToEventSession = from pl in eventPages
                join fl in eventFolders on pl.FolderId equals fl.FolderId
                join ml in eventCourses on fl.ModuleId equals ml.ModuleId
                select new EventSession
                {
                    Id = pl.PageId,
                    CourseId = ml.ModuleId,
                    SessionCourseName = ml.ModuleDisplayName,
                    SessionEventName = pl.PageDisplayName,
                    SessionDate = fl.FolderDateTimeStart,
                    SessionStartDateTime = new DateTime(
                        fl.FolderDateTimeStart.Value.Year, 
                        fl.FolderDateTimeStart.Value.Month, 
                        fl.FolderDateTimeStart.Value.Day, 
                        pl.PageStartTime.Value.Hour, 
                        pl.PageStartTime.Value.Minute, 
                        pl.PageStartTime.Value.Second),
                    SessionEndDateTime = new DateTime(
                        fl.FolderDateTimeStart.Value.Year,
                        fl.FolderDateTimeStart.Value.Month,
                        fl.FolderDateTimeStart.Value.Day,
                        pl.PageEndTime.Value.Hour,
                        pl.PageEndTime.Value.Minute,
                        pl.PageEndTime.Value.Second),
                    CatalogId = new Guid(ml.MediasiteCatalogId)
                };

            // order by moduleDisplayName, folderDateTimeStart, PageStartTime
            var orderedResults = queryToEventSession
                .OrderBy(n => n.CourseId)
                .ThenBy(d => d.SessionDate)
                .ThenBy(tsa => tsa.SessionStartDateTime.Value.TimeOfDay)
                .ToList();

            var ts = TimeSpan.FromMinutes(30);

            // Add a SelectMany after the first grouping to break up each group into sub-groups by the time based condition

            // the SelectMany takes each group on a given SessionDate, and sub-groups them into runs where each member 
            // is less than 35 minutes from the next. Because of SelectMany, the sub-groups are all promoted to be the groups of the final result. 
            // So now you have groups where each one contains a run of sessions where there are less than 35 minutes 
            // from SessionEndDateTime to next SessionStartDateTime. Note that a run will end at the end of the day regardless, 
            // so if you might have runs that go across midnight, you would need to change the grouping.

            // Note: If it is possible for sessions starting at the same time to have 
            // different durations(i.e.end times) then you need to add a ThenBy(tsa => tsa.SessionEndTime) to your orderedResults sorting.

            var groupedEventsList = orderedResults.GroupBy(x => new { x.SessionDate, x.CourseId })
                         .SelectMany(g => g.GroupByPairsWhile((p, c) => c.SessionStartDateTime.Value.TimeOfDay - p.SessionEndDateTime.Value.TimeOfDay <= TimeSpan.FromMinutes(35)))
                         .Select(group => new GroupedEvents
                         {
                             EventCourseName = group.Select(g => g.SessionCourseName).FirstOrDefault(),
                             EventCourseId = group.Select(g => g.CourseId).FirstOrDefault().ToString(),
                             EventRecordDate = group.Select(g => g.SessionDate).FirstOrDefault(),
                             EventName = String.Join("; ", group.Select(g => g.SessionEventName).ToArray()),
                             EventPlayDuration = group.Select(g => g.SessionStartDateTime.Value.TimeOfDay).First() + " - " + group.Select(g => g.SessionEndDateTime.Value.TimeOfDay).Last(),
                             SessionStartDateTime = group.Select(g => g.SessionStartDateTime).First(),
                             SessionEndDateTime = group.Select(g => g.SessionEndDateTime).Last(),
                             EventCatalogId = group.Select(g => g.CatalogId).FirstOrDefault()
                         })
                         .ToList();

            return groupedEventsList;
        }
    }
}
