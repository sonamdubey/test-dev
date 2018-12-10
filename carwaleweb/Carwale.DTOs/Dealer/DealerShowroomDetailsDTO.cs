using Carwale.DTO.Dealers;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Dealers;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.Dealers
{
    //Created on 18/02/16 by Vicky Lund, Used in new API which needed class without Serializable
    public class DealerShowroomDetailsDTO
    {
        [JsonProperty("dealerDetails")]
        public DealerDetails DealerDetails { get; set; }

        [JsonProperty("imageList")]
        public List<CarImageBaseDTO> ImageList { get; set; }

        [JsonProperty("modelDetails")]
        public List<CarModelSummaryDTO> ModelDetails { get; set; }
    }
}
