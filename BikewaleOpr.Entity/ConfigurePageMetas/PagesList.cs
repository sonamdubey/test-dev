using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.ConfigurePageMetas
{
    public class PageEntity
    {
        public uint PageId { get; set; }
        public ushort PlatformId { get; set; }
        public string PageName { get; set; }

        public ushort GroupId { get; set; }
    }
}
