using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class ImagePopUpDTO
    {
        public string DealerName { get; set; }
        public string DealerMobileNo { get; set; }
        public string DealerDetailsHref { get; set; }
        public string CityAvailable { get; set; }
        public bool ShowDealerDetailsLink { get; set; }
        public bool AdAvailable { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string CityName { get; set; }
        public string LocateDealerLink { get; set; }
        public int NewCarDealersCount { get; set; }
        public int ModelId { get; set; }
        public int MakeId { get; set; }
        public int VersionId { get; set; }
        public bool IsNew { get; set; }
        public bool Futuristic { get; set; }
    }
}
