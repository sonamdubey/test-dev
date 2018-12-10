using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    [Serializable]
    public class CarVersionItemValue
    {
        public int CarVersionId { get; set; }

        public int ItemMasterId { get; set; }

        public string Value { get; set; }
    }
}
