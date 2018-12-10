using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.CarData;

namespace Carwale.Entity.Deals
{
    [Serializable]
    public class ProductDetails
    {
        public MakeEntity Make { get; set; }
        public ModelEntity Model { get; set; }
        public List<CarVersionEntity> Version { get; set; }
        public List<ColorEntity> ModelColorsEntity { get; set; }
        public List<DealsStock> DealsStock { get; set; }
        public int CurrentVersionId { get; set; }
        public CarImageBase CarImage { get; set; }
        public List<DealsOfferEntity> OffersList { get; set; }
    }
}
