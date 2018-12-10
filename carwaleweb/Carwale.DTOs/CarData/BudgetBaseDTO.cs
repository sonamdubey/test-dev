using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.DTOs.Common;
using Newtonsoft.Json;

namespace Carwale.DTOs.CarData
{
	public class BudgetBaseDTO: MinMaxDTO
	{
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
	}
}
