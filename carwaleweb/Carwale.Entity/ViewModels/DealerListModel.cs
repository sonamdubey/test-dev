using Carwale.Entity.CarData;
using Carwale.Entity.Dealers;
using System.Collections.Generic;

namespace Carwale.Entity.ViewModels
{
    public class DealerListModel
    {
        public NewCarDealerEntiy Dealers { get; set; }
        public string mapData { get; set; }
        public int makeId { get; set; }
        public int cityId { get; set; }
		public string CityName { get; set; }
		public string CityMaskingName { get; set; }
		public List<CarModelSummary> ModelListWithDetails { get; set; }
        public List<BreadcrumbEntity> BreadcrumbEntitylist { get; set; }
    }
}
