using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CMS
{
    public class PageListDTO
    {
        public List<int> Pages { get; set; }
        public int CurrentPageNo { get; set; }
    }
}
