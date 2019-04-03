using LET.Panopto.Scheduler.Models;
using RemoteRecorderManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LET.Panopto.Scheduler.Scheduling
{
    public interface IConflictGenerator
    {
        List<ScheduleBlockResult> GenerateConflicts(ScheduledRecordingResult r);
    }

    public sealed class ConflictGenerator : IConflictGenerator
    {
        public  List<ScheduleBlockResult> GenerateConflicts(ScheduledRecordingResult r)
        {
            List<ScheduleBlockResult> recordingResults = new List<ScheduleBlockResult>();


            recordingResults = r.ConflictingSessions.Select(c => new ScheduleBlockResult
            {
                SessionID = c.SessionID,
                SessionName = c.SessionName,
                SessionStart = c.StartTime,
                SessionEnd = c.EndTime,
                SessionMessage = "CONFLICT FOUND",
                ConflictExists = true
            }).ToList();

            return recordingResults;
        }
    }
}
