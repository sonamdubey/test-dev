using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Carwale.DTOs.Search.Model;

namespace Carwale.DTOs.CarData
{
	public class AllFiltersDTO
	{
		[JsonProperty("budget")]
		public BudgetBaseDTO Budget { get; set; }
		[JsonProperty("emi")]
		public EmiBaseDTO Emi { get; set; }
		[JsonProperty("makes")]
        public MakesFilterDto Makes { get; set; }
		[JsonProperty("fuelType")]
        public FuelTypeFilterDto FuelType { get; set; }
		[JsonProperty("transmissionType")]
        public TransmissionTypeFilterDto TransmissionType { get; set; }
		[JsonProperty("bodyType")]
        public BodyTypeFilterDto BodyType { get; set; }
		[JsonIgnore, JsonProperty("seatingCapacity")]
        public SeatingCapacityFilterDto SeatingCapacity { get; set; }
		[JsonIgnore, JsonProperty("enginePower")]
        public EnginePowerFilterDto EnginePower { get; set; }
	}
}
