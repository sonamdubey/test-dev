using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    [Serializable]
    public class CarModelSummary : MakeModelIdsEntity
    {
        public string ModelImage { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string MaskingName { get; set; }
        public float ModelRating { get; set; }
        public int ReviewCount { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        //public bool Discontinued { get; set; }
    }
}
