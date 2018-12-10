using Carwale.Entity;
using Carwale.Entity.Enum;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Carwale.DTOs.CarData
{
	public class TopCarsByBodyTypeDto
	{
		public string BodyType { get; set; }
		public CarBodyStyle BodyStyleId { get; set; }
		public string CityName { get; set; }
		public string CityZone { get; set; }
		public string DomainName { get; set; }
		public string SEOText { get; set; }
		public JObject Schema { get; set; }
		public List<ModelDetailsDto> ModelList { get; set; }
		public List<BreadcrumbEntity> BreadcrumbEntitylist { get; set; }
        public string NextPageUrl { get; set; }
        public int TotalModels { get; set; }
        public int TotalVersions { get; set; }
        public bool IsFilterApplied { get; set; }
        public int FilterAppliedCount { get; set; }
	}
}
