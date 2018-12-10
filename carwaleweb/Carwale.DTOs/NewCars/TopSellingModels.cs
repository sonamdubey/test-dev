using Carwale.Entity.CarData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class TopSellingModels
    {
        [JsonProperty("topSelling")]
        public List<TopSellingCarModel> TopSelling;
    }
}
