using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity
{
    public class Pagination
    {
        public ushort PageSize { get; set; }
        public ushort PageNo { get; set; }
        public bool IsFromApp { get; set; }
    }

}
