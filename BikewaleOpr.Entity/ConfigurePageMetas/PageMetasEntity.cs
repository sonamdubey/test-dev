using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.ConfigurePageMetas
{
    public class PageMetasEntity
    {
        public uint PageMetaId { get; set; }
        public uint MakeId { get; set; }
        public uint ModelId { get; set; }
        public uint PageId { get; set; }
        public ushort PlatformId { get; set;}
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Heading { get; set; }
        public string Summary { get; set; }
        public int EnteredBy { get; set; }        

    }
}
