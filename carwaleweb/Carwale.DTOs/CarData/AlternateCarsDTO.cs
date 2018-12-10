using Carwale.DTOs.Geolocation;
using System.Collections.Generic;

namespace Carwale.DTOs.CarData
{
    public class AlternateCarsDTO
    {
        public List<SimilarCarModelsDtoV3> SimilarModelList { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public PQCustLocationDTO Location { get; set; }
        public int WidgetCardCount { get; set; }
    }
}
