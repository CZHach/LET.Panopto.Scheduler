using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LET.Panopto.Scheduler.Models
{
    public class EventSession
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string SessionCourseName { get; set; }
        public string SessionEventName { get; set; }
        public DateTime? SessionDate { get; set; }
        public DateTime? SessionStartDateTime { get; set; }
        public DateTime? SessionEndDateTime { get; set; }
        public string SessionDuration { get; set; }
        public Guid CatalogId { get; set; }
    }
}
