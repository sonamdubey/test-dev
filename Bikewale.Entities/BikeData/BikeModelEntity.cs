using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    [Serializable]
    public class BikeModelEntity : BikeModelEntityBase
    {
        private BikeMakeEntityBase objmakeBase = new BikeMakeEntityBase();
        public BikeMakeEntityBase MakeBase { get { return objmakeBase; } set { objmakeBase = value; } }
        private BikeSeriesEntityBase objEntityBase = new BikeSeriesEntityBase();
        public BikeSeriesEntityBase ModelSeries { get { return objEntityBase; } set { objEntityBase = value; } }
        public bool New { get; set; }   //Added by suresh prajapati
        public bool Used { get; set; }  //Added by suresh prajapati
        public bool Futuristic { get; set; }
        public string SmallPicUrl { get; set; }
        public string LargePicUrl { get; set; }
        public string HostUrl { get; set; }
        public Int64 MinPrice { get; set; }
        public Int64 MaxPrice { get; set; }
        public double ReviewRate { get; set; }
        public int ReviewCount { get; set; }
        public string OriginalImagePath { get; set; }
    }
}
