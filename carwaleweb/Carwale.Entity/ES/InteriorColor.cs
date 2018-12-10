using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.ES
{
    [Serializable]
    public class InteriorColor : ColorEntity
    {
        public int ExtColorId { get; set; }
        public int CarCount { get; set; }
    }
}
