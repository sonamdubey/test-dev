using System;
using System.Collections.Generic;
using System.Text;

namespace BikeWaleOpr.Entities
{
    [Serializable]
    public class PQ_QuotationEntity
    {
        public List<PQ_Price> PriceList { get; set; }

        public MakeEntityBase objMake { get; set; }
        public ModelEntityBase objModel { get; set; }
        public VersionEntityBase objVersion { get; set; }

        public string HostUrl { get; set; }
        public string LargePicUrl { get; set; }
        public string SmallPicUrl { get; set; }
    }
}
