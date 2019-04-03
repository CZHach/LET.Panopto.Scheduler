using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LET.Panopto.Scheduler.Models
{
    public class ModulesByClassYear
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ClassYear { get; set; }
        public string MediasiteCatalogId { get; set; }
    }
}
