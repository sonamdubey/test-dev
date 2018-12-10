using Carwale.DTOs.CarData;
using Carwale.Entity;
using Carwale.Entity.ElasticEntities;
using Carwale.Entity.Enum;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.NewCars
{
	public interface ITopCarsBl
	{
        string GetSeoText(List<ModelDetailsDto> modelList, string bodyType, CarBodyStyle bodyStyle, string price = "");
		JObject CreatSchema(List<ModelDetailsDto> modelList, string bodyType);
		List<BreadcrumbEntity> GetBreadCrumb(string bodyType, string price = "");
        TopCarsByBodyTypeDto GetTopModels(NewCarSearchInputs inputs, bool isNcf);
	}
}
