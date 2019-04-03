using System;
using System.Collections.Generic;

namespace LET.Panopto.Scheduler.Models
{
    public partial class ModuleList
    {
        public ModuleList()
        {
            FolderList = new HashSet<FolderList>();
        }

        public int ModuleId { get; set; }
        public string ModuleDisplayName { get; set; }
        public int? AcademicYear { get; set; }
        public string MediasiteCatalogId { get; set; }
        public bool IsPlaceholder { get; set; }
        public byte PublishingStatus { get; set; }
        public ICollection<FolderList> FolderList { get; set; }
    }
}
