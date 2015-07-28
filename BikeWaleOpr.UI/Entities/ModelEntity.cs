using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeWaleOpr.Entities
{
    [Serializable]
    public class ModelEntity : ModelEntityBase
    {
        public MakeEntity objMake { get; set; }

        public bool New { get; set; } 
        public bool Used { get; set; }
        public bool Futuristic { get; set; }
        public string SmallPicUrl { get; set; }
        public string LargePicUrl { get; set; }
        public string HostUrl { get; set; }
        public UInt64 MinPrice { get; set; }
        public UInt64 MaxPrice { get; set; }
    }
}
