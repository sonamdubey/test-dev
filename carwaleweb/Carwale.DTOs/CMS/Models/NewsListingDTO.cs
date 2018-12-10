using Carwale.DTOs.CMS.Articles;
using Carwale.DTOs.CMS.Media;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CMS
{
    public class NewsListingDTO
    {
        public List<int> ExpertCategories;
        public List<int> FeatureCategories;
        public List<int> NewsCategories;
        public List<Video> Videos;
        public List<TopSellingCarModel> TopSellingCars;
        public List<UpcomingCarModel> UpcomingCars;
        public CMSContentDTOV2 Page1;
        public MediaDTO MediaPage1;
        public Carwale.Entity.CMS.Articles.CMSContent PopularNews;
        public int pageNumber;
        public string category;
        public Car make = null;
        public Car model = null;
        public List<int> Pages;
        public List<ContentSegmentDTO> Segments;
        public bool HasNextPage;
        public bool HasPrevPage;
        public bool IsGallery;
    }
    public class Car {
        public string name;
        public string displayName;
        public int id;
    }
}
