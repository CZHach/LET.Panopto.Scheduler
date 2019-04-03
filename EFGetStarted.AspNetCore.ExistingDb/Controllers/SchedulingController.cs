using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SessionManagement;
using RemoteRecorderManagement;
using Microsoft.AspNetCore.Mvc;
using LET.Panopto.Scheduler.Models;
using LET.Panopto.Scheduler.Utilities;
using LET.Panopto.Scheduler.Scheduling;

namespace LET.Panopto.Scheduler.Controllers
{
    public class SchedulingController : Controller
    {
        private readonly NavEventsContext _context;
        public IScheduleCreationInitiator ScheduleCreationInitiator { get; }
        public IScheduleGenerator ScheduleGenerator { get; }
        public IConflictGenerator ConflictGenerator { get;  }
        public ISessionGenerator SessionGenerator { get; }

        public SchedulingController(
                NavEventsContext context,
                IScheduleGenerator scheduleGenerator,
                IScheduleCreationInitiator scheduleCreationInitiator,
                IConflictGenerator conflictGenerator,
                ISessionGenerator sessionGenerator
               )
        {
            _context = context;
            ScheduleGenerator = scheduleGenerator;
            ScheduleCreationInitiator = scheduleCreationInitiator;
            ConflictGenerator = conflictGenerator;
            SessionGenerator = sessionGenerator;
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleBlock(DateTime start, DateTime end)
        {
            List<GroupedEvents> groupedEventsList = await ScheduleGenerator.GenerateWeeklySchedule(start, end);

            List<bool> conflictsList = new List<bool>();
            List<ScheduleBlockResult> recordingResults = new List<ScheduleBlockResult>();

            foreach(var recordedSession in SessionGenerator.GenerateSession(groupedEventsList))
            {
                ScheduledRecordingResult result = await ScheduleCreationInitiator.ScheduleRecordings(recordedSession);
                conflictsList.Add(result.ConflictsExist);
                recordingResults = ConflictGenerator.GenerateConflicts(result);
            }

            if(conflictsList.All(x => x == true)) {
                return View(recordingResults);
            }

            recordingResults.Add(
                new ScheduleBlockResult
                {
                    SessionMessage = "No Conflicts. Sessions scheduled successfully."
                });

            return View(recordingResults);
        }
    }
}