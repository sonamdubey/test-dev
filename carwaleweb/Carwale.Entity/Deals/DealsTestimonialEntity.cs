using Carwale.Entity.CarData;
using Carwale.Entity.Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Deals
{
    [Serializable]
    public class DealsTestimonialEntity
    {
        public MakeEntity Make { get; set; }
        public ModelEntity Model { get; set; }
        public City City { get; set; }
        public CarImageBase ImageDetails { get; set; }
        public string CustomerName { get; set; }
        public string Comments { get; set; }
    }
}