using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.SellCarUsed
{
    public class CustomerSellInquiryVehicleData
    {
        public string CarName { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string VersionName { get; set; }
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public int VersionId { get; set; }
        public DateTime MakeYear { get; set; }
        public string City { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public int PinCode { get; set; }
        public int Price { get; set; }
        public int Kilometers { get; set; }
        public string HostURL { get; set; }
        public string OriginalImgPath { get; set; }
        public int Owners { get; set; }
    }
}
