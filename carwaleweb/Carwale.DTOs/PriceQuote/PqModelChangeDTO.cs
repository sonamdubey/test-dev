using Carwale.DTOs.Geolocation;
using Carwale.Entity.CarData;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.PriceQuote
{
    public class PqModelChangeDTO
    {
        public CityAreaDTO Location { get; set; }
        public List<SimilarCarModels> AlternateCarList { get; set; }

        public PqModelChangeDTO()
        {
            this.Location = new CityAreaDTO();
            this.AlternateCarList = new List<SimilarCarModels>();
        }
    }
}
