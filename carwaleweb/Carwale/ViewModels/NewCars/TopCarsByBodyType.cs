using Carwale.DTOs.CarData;
using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carwale.UI.ViewModels.NewCars
{
    public class TopCarsByBodyType
    {
        public string BodyType { get; set; }
        public CarBodyStyle BodyStyleId { get; set; }
        public string CityName { get; set; }
        public string CityZone { get; set; }
        public string DomainName { get; set; }
        public string BudgetText { get; set; }
        public string BudgetLink { get; set; }
        public bool IsBudgetPage { get; set; }
        public List<ModelDetailsDto> TopCarsByBodyTypeList { get; set; }
    }
}