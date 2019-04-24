using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LET.Panopto.Scheduler.Models
{
    public class Groups
    {
        public int GroupId { get; set; }
        public char Discriminator { get; set; }
        public string Title { get; set; }
        public int? ClassYear { get; set; }
    }
}
