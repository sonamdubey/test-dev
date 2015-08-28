using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entity.BikeData
{
    [Serializable]
    public class BikeModelEntityBase
    {
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public string MaskingName { get; set; }
    }
}
