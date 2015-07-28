using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeWaleOpr.Entities
{
    [Serializable]
    public class MakeEntity : MakeEntityBase
    {
        public bool New { get; set; }
        public bool Used { get; set; }
        public bool Futuristic { get; set; }
    }
}
