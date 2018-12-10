using Carwale.Entity.Enum;
using System.Collections.Generic;

namespace Carwale.Entity.CarData
{
    public class SimilarCarVmRequest
    {
        public int ModelId { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string MaskingName { get; set; }
        public int CityId { get; set; }
        public WidgetSource WidgetSource { get; set; }
        public string PageName { get; set; }
        public bool IsMobile { get; set; }
        public bool IsFuturistic { get; set; }
        public string CwcCookie { get; set; }
        public int StateId { get; set; }
        public List<SimilarCarModels> SimilarCarModelList { get; set; }
    }
}
