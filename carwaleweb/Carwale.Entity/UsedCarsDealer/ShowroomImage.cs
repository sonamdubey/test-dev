using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.UsedCarsDealer
{
    public class ShowroomImage
    {
        public int Id { get; set; }
        public string LandingUrl { get; set; }

        public string Url { get; set; }

        public string HostUrl { get; set; }

        public string DirectoryPath { get; set; }

        public string LargeImage { get; set; }

        public int BannerImgSortingOrder { get; set; }

        public string OriginalImgPath { get; set; }
    }
}
