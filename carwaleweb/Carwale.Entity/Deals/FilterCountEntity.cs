using Carwale.Entity.Classified;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Deals
{
    [Serializable]
    public class FilterCountEntity
    {
        public List<StockMake> Makes { get; set; }
    }
}
