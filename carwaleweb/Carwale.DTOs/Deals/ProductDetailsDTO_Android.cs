using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.DTOs.CarData;
using Newtonsoft.Json;
namespace Carwale.DTOs.Deals
{
    public class ProductDetailsDTO_Android
    {
        [JsonProperty("make")]
        public CarMakesDTO Make {get; set; }
        [JsonProperty("model")]
        public CarModelsDTO Model {get; set; }
        [JsonProperty("version")]
        public List<Versions> Version { get; set; }
        [JsonProperty("carColorDetails")]
        public List<CarColorAndroid_DTO> CarColorsDetails { get; set; }
        [JsonProperty("currentVersionId")]
        public int CurrentVersionId { get; set; }
        [JsonProperty("bookingAmount")]
        public int BookingAmount { get; set; }
        [JsonProperty("tollFreeNumber")]
        public string TollFreeNumber { get; set; }
        [JsonProperty("carImage")]
        public CarImageBaseDTO CarImage { get; set; }
        [JsonProperty("testimonials")]
        public List<DealsTestimonialDTO> Testimonials { get; set; }
        [JsonProperty("buttonText")]
        public ProductButtonText_DTO ButtonText { get; set; }

   }
}
