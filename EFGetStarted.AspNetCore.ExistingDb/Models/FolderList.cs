using System;
using System.Collections.Generic;

namespace EFGetStarted.AspNetCore.ExistingDb.Models
{
    public partial class FolderList
    {
        public FolderList()
        {
            PageList = new HashSet<PageList>();
        }

        public int ModuleId { get; set; }
        public int FolderId { get; set; }
        public int FolderSequence { get; set; }
        public string FolderDisplayName { get; set; }
        public DateTime? FolderDateTimeStart { get; set; }
        public bool Hidden { get; set; }
        public DateTime? ShowStartDate { get; set; }
        public DateTime? ShowEndDate { get; set; }
        public bool HasCustomAcl { get; set; }

        public ModuleList Module { get; set; }
        public ICollection<PageList> PageList { get; set; }
    }
}
