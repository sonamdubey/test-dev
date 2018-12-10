using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Carwale.DTOs.Deals
{
   public class ProductButtonText_DTO
    {
       [JsonProperty("floatBtnText")]
       public string FloatingButtonText { get; set; }
       [JsonProperty("leadCaptureBtnText")]
       public string LeadCaptureText { get; set; }
       [JsonProperty("getEmiBtnText")]
       public string GetEmiButtonText { get; set; }
    }
}
