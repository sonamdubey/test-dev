using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS
{
    public class CarSynopsisEntity
    {
        public int ModelId { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string PageName { get; set; }
        public int Priority { get; set; }
    }
}
