using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
    public class CarPhotos
    {
        public int? Id { get; set; }
        public int? SourceId { get; set; }
        public int? SellerType { get; set; }
        public int? StockId { get; set; }
        public string Description { get; set; }
        public bool? IsMain { get; set; }
        public string HostUrl { get; set; }
        public bool IsReplicated { get; set; }
        public String Title{ get; set; }
        public int? TC_CarPhotoId { get; set; }
        public bool IsApproved { get; set; }
        public string OriginalImgPath { get; set; }
        public int ImageType { get; set; }
        public bool IsActive { get; set; }
    }
}
