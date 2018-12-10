using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.SellCarUsed
{
    public class ClassifiedCarColors
    {
        public int Id { get; set; }
        public string ColorName { get; set; }
        public string OrderName { get; set; }
        public string HexCode { get; set; }
        public bool IsExterior { get; set; }
    }
}
