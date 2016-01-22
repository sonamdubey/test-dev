using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate on 22 Jan 2016
    /// Description :   Compare Bike Color Entity
    /// </summary>
    [Serializable]
    public class CompareBikeColor
    {
        public List<BikeColor> bikeColors { get; set; }
    }
}
