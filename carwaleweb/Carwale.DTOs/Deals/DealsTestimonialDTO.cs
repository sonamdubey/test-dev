using Carwale.DTOs.CarData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Deals
{
    public class DealsTestimonialDTO
    {
        public CarMakesDTO Make { get; set; }
        public CarModelsDTO Model { get; set; }
        public City City { get; set; }
        public CarImageBaseDTO ImageDetails { get; set; }
        public string CustomerName { get; set; }
        public string Comments { get; set; }
    }
}