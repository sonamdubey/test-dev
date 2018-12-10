using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.PriceQuote
{
    public class EmiCalculatorDto
    {
        [JsonProperty("emiWidgetCommonData")]
        public EmiWidgetCommonDataDto EmiWidgetCommonData { get; set; }
        [JsonProperty("emiCalculatorData")]
        public List<EmiCalculatorModelDataDto> EmiCalculatorData { get; set; }

        public EmiCalculatorDto()
        {
            this.EmiCalculatorData = new List<EmiCalculatorModelDataDto>();
        }
    }
}
