using System;
using System.Collections.Generic;

namespace LET.Panopto.Scheduler.Models
{
    public partial class PageList
    {
        public int FolderId { get; set; }
        public int PageId { get; set; }
        public string PageDisplayName { get; set; }
        public int PageSequence { get; set; }
        public DateTime? PageStartTime { get; set; }
        public DateTime? PageEndTime { get; set; }
        public bool Hidden { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? ShowStartDate { get; set; }
        public DateTime? ShowEndDate { get; set; }
        public string PageKeywords { get; set; }
        public string BodyContent { get; set; }
        public int? LinkLibraryId { get; set; }
        public int? PageTypeId { get; set; }
        public bool HasCustomAcl { get; set; }
        public long? ObjectiveId { get; set; }

        public FolderList Folder { get; set; }
    }
}
