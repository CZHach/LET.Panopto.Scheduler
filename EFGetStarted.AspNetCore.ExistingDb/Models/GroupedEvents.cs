using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LET.Panopto.Scheduler.Models
{
    public class GroupedEvents
    {
        public string EventCourseId { get; set; }
        public string EventCourseName { get; set; }
        public int ClassYear { get; set; }
        public DateTime? EventRecordDate { get; set; }
        public string EventName { get; set; }
        public string EventPlayDuration { get; set; }
        public DateTime? SessionStartDateTime { get; set; }
        public DateTime? SessionEndDateTime { get; set; }
        public Guid EventRecorderId { get; set; }
        public Guid EventCatalogId { get; set; }
        public bool ScheduleResult { get; set; }

    }
}
