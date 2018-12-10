using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.Enum;
using Carwale.Entity.CMS;

namespace Carwale.Entity.ViewModels.CarData
{
    public class VideosCarousel
    {
        public List<Video> Videos{get;set;}
        public string Title { get; set; }
        public string CarName { get; set; }
        public string ViewAllVideoUrl { get; set; }
        public ContentPages Page { get; set; }
        public string ModelName { get; set; }
       
    }
}
