using System;
using System.Collections.Generic;
using System.Text;


namespace BikeWaleOpr.Entities
{   
    [Serializable]
    public class VersionEntity : VersionEntityBase
    {
        public MakeEntity objMake { get; set; }

        public ModelEntity objModel { get; set; }

        public bool New { get; set; }
        public bool Used { get; set; }
        public bool Futuristic { get; set; }
        public string HostUrl { get; set; }
        public string SmallPicUrl { get; set; }
        public string LargePicUrl { get; set; }
        public UInt64 Price { get; set; }
    }
}
