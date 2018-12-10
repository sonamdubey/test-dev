using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Deals;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.CMS.UserReviews;
using Carwale.Entity.Dealers;
using Carwale.Entity.PriceQuote;
using Carwale.Entity.UserProfiling;
using System.Collections.Generic;

namespace Carwale.DTOs.NewCars
{
    /// <summary>
    /// Created By : Shalini Nair
    /// </summary>
    public class UpcomingCarsDTO
    {
		public string Url;
		public int PageNo;
		public int Sort;
		public int TotalPages;
		public int MakeId;
		public string MakeName;
		public string RedirectPath;
		public List<CarMakeEntityBase> CarMakes;
        public List<UpcomingCarModel> UpcomingCarModels;
		public Dictionary<int, string> CarSynopsis;
		public List<Carwale.Entity.CarData.LaunchedCarModel> NewLaunches;
		public PageMetaTags MetaData;
    }
}
