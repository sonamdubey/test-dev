using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    [Serializable]
    public class VizuryVersionData
    {
        public string carbodytype { get; set; }
        public string modelname { get; set; }
        public string brandname { get; set; }
        public string variantname { get; set; }
        public string fueltype { get; set; }
        public string modelurl { get; set; }
        public string versionurl { get; set; }
        public string imageurl { get; set; }
        public string productid { get; set; }
        public string arai_mileadge { get; set; }
        public string cc { get; set; }
        public string exshowroomprice { get; set; }
        public string colors { get; set; }
        public string largeimgurl { get; set; }
    }
}
