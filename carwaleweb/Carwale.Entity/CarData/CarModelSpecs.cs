using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    /// <summary>
    /// Created By : Shalini Nair
    /// </summary>
    [Serializable]
    public class CarModelSpecs
    {
        public int ItemId { get; set; }
        public string Item { get; set; }
        
        public string ItemValue { get; set; }
        public int ItemRank { get; set; }
        }
}
