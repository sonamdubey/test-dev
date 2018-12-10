using Carwale.DTOs.CarData;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.ViewModels;
using Carwale.Entity.ViewModels.CarData;
using Carwale.Entity.UserProfiling;
using System;
using System.Collections.Generic;
using Carwale.Entity.CMS;

namespace Carwale.DTOs.NewCars
{
    /// <summary>
    /// Created By : Shalini on 02/12/14
    /// </summary>
    public class MakePageDTO_Mobile
    {
        public List<CarModelSummaryDTOV2> NewCarModels { get; set; }
        public CarMakeDescription CarMakeDescription { get; set; }
        public List<ArticleSummary> News { get; set; }
        public List<UpcomingCarModel> UpcomingModels { get; set; }
        public CarMakeEntityBase MakeDetails { get; set; }        
        public PageMetaTags MetaData { get; set; }
        public ImageCarousal Images { get; set; }
        public List<Video> MakeVideos { get; set; }
        public City CityDetails { get; set; }
    }

   
}
