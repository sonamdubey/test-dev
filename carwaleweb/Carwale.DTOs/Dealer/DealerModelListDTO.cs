using Carwale.DTOs.CarData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Dealer
{
    public class DealerModelListDTO : CarMakesDTO
    {
        [JsonProperty("modelDetails")]
        public List<CarModelsDTO> ModelDetails;
    }
}
