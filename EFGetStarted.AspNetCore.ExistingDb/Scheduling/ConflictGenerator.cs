using EFGetStarted.AspNetCore.ExistingDb.Models;
using RemoteRecorderManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EFGetStarted.AspNetCore.ExistingDb.Scheduling
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
                SessionMessage = "CONFLICT"
            }).ToList();

            return recordingResults;
        }
    }
}
