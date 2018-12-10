using Carwale.Entity.CarData;
using System.Collections.Generic;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Dealers;
using Carwale.Entity;
using System;

namespace Carwale.Entity.PriceQuote
{   
    [Serializable]
    public class PQCarDetails
    {
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public int VersionId { get; set; }

        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string VersionName { get; set; }
        public string MaskingName { get; set; }
      
        public string LargePic { get; set; }
        public string SmallPic { get; set; }
        public string XtraLargePic { get; set; }
        public string OriginalImgPath { get; set; }
        public string HostURL { get; set; }

        public byte SubSegmentId { get; set; }
    }
}
