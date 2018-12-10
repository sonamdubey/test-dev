using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified
{
    public class CarPhoto
    {
        public int Id { get; set; }
        public int InquiryId { get; set; }
        public string ImageUrlFull { get; set; }
        public string ImageUrlThumb { get; set; }
        public string ImageUrlThumbSmall { get; set; }
        public string Description { get; set; }
        public bool IsMain { get; set; }
        public bool IsDealer { get; set; }
        public int StatusId { get; set; }
        public string DirectoryPath { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImgPath { get; set; }
        public string FileName { get; set; }
        public int IsReplicated { get; set; }
        public int ImageType { get; set; }
        public bool IsApproved { get; set; }
    }
}
