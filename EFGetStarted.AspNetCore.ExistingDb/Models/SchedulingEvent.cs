using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LET.Panopto.Scheduler.Models
{
    public class SchedulingEvent
    {
        public string SessionName { get; set; }
        public Guid SessionRecorderId { get; set; }
        public string SessionFolderId { get; set; }
        public DateTime SessionStart { get; set; }
        public DateTime SessionEnd { get; set; }
        public Guid SessionCatalogId { get; set; }
    }
}
