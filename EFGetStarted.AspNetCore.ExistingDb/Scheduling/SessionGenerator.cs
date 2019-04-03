using EFGetStarted.AspNetCore.ExistingDb.Models;
using RemoteRecorderManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EFGetStarted.AspNetCore.ExistingDb.Scheduling
{
    public interface ISessionGenerator
    {
        List<SchedulingEvent> GenerateSession(List<GroupedEvents> groupedEventsList);
    }
    public sealed class SessionGenerator : ISessionGenerator
    {
        public List<SchedulingEvent> GenerateSession(List<GroupedEvents> groupedEventsList)
        {

            List<SchedulingEvent> recordedSession = groupedEventsList.Select(s => new SchedulingEvent
            {
                SessionName = s.EventName,
                SessionStart = s.SessionStartDateTime.Value.AddMinutes(-5),
                SessionEnd = s.SessionEndDateTime.Value.AddMinutes(15),
                SessionFolderId = s.EventCourseId,
                SessionRecorderId = s.EventRecorderId,
                SessionCatalogId = s.EventCatalogId
            }).ToList();

            return recordedSession;
        }

    }
}
