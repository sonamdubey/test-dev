using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity
{
    [Serializable]
    public class ContentFilters
    {
        public bool NoFilter { get; set; }
        public int Year { get; set; }
        public DateTime Date { get; set; }
        public int MakeId { get; set; }
        public int ModelId { get; set; }
    }    
}
