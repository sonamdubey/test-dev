using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Accessories.Tyres
{
    [Serializable]
    public class TyreList
    {
        public int Count { get; set; }
        public List<TyreSummary> Tyres { get; set; }
        public bool LoadAdslot { get; set; }
    }
}
