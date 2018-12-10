using Carwale.Entity;
using Carwale.Entity.Enum;
using Carwale.Entity.ViewModels;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Classified.PopularUsedCarsDetails;
using Carwale.Interfaces.CompareCars;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.Deals.Cache;
using Carwale.Interfaces.Home;
using Carwale.Interfaces.IPToLocation;
using Carwale.Interfaces.SponsoredCar;
using Carwale.Notifications;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Carwale.BL.NewCars
{
    public class HomePageAdapter : IServiceAdapter
    {       
        private readonly IIPToLocation _iPToLocation;
        private readonly ISponsoredCarCache _sponsoredCars;
        private readonly ICompareCarsBL _compRepo;       
        private readonly IPopularUCDetails _usedBl;
        public HomePageAdapter(ISponsoredCarCache sponsoredCars, ICompareCarsBL compRepo, IPopularUCDetails usedBl, IIPToLocation iPToLocation)
        {           
            _sponsoredCars = sponsoredCars;
            _compRepo = compRepo;           
            _usedBl = usedBl;
            _iPToLocation = iPToLocation;
        }

        public T Get<T>(string city)
        {
            return (T)Convert.ChangeType(GetHomePageAdaptor(city), typeof(T));
        }

        public T Get<T>()
        {
            throw new NotImplementedException();
        }

        private HomeModel GetHomePageAdaptor(string city)
        {
            HomeModel Model = new HomeModel();
            try
            {
                int cityId = -1;
                int.TryParse(city,out cityId);
                var carwaleDesktop = (int)Carwale.Entity.Enum.Platform.CarwaleDesktop;

                Model.IPToLocation = _iPToLocation.GetCity();               
                                          
                //home page banner
                var sponsorHomeBanner = _sponsoredCars.GetSponsoredCampaigns((int)Carwale.Entity.Enum.CampaignCategory.Banner, carwaleDesktop, (int)CategorySection.HomePageBanner);
                if (sponsorHomeBanner != null && sponsorHomeBanner.Count > 0)
                {
                    Model.SponsoredHomeBanner = sponsorHomeBanner.First();
                }

                //home page PQ banner
                var sponsorPQBanner = _sponsoredCars.GetSponsoredCampaigns((int)Carwale.Entity.Enum.CampaignCategory.Banner, carwaleDesktop, (int)CategorySection.PQWidget);
                if (sponsorPQBanner != null && sponsorPQBanner.Count > 0)
                {
                    Model.SponsoredPQBanner = sponsorPQBanner.First();
                }
               
                //compare car widget
                var page = new Pagination() { PageNo = 1, PageSize = 6 };              

                Model.HotComparisons = _compRepo.GetHotComaprisons(page,cityId,true);
                int widgetSource = (int)WidgetSource.HomePageCompareCarWidgetDesktop;
                if (Model.HotComparisons != null && Model.HotComparisons.Count > 0)
                    Model.HotComparisons.ForEach(x => x.WidgetPage = widgetSource);

                //popular used cars
                Model.UsedCity = _usedBl.FetchCityById(city);
                Model.PopularUsedCars = _usedBl.FillPopularUsedCarDetails(Model.UsedCity);

                //sponsored placeholder
                //To fetch sponsored campaign for autosuggest placeholder HomePageBanner  
                var newcarPlaceHolder = _sponsoredCars.GetSponsoredCampaigns((int)Carwale.Entity.Enum.CampaignCategory.CarSearchPlaceHolder, carwaleDesktop, (int)CategorySection.HomePageBanner);
                if (newcarPlaceHolder != null && newcarPlaceHolder.Count > 0) 
                {
                    Model.NewCarPlaceHolder = newcarPlaceHolder.First();
                }

                //To fetch sponsored campaign for autosuggest placeholder PQWidget  
                var pqPlaceHolder = _sponsoredCars.GetSponsoredCampaigns((int)Carwale.Entity.Enum.CampaignCategory.CarSearchPlaceHolder, carwaleDesktop, (int)CategorySection.PQWidget);
                if (pqPlaceHolder != null && pqPlaceHolder.Count > 0)
                {
                    Model.PQPlaceHolder = pqPlaceHolder.First();
                }

                //To fetch example data below search
                var searchExampleText = _sponsoredCars.GetSponsoredCampaigns((int)Carwale.Entity.Enum.CampaignCategory.SearchExampleText, carwaleDesktop, (int)CategorySection.HomePageBanner);
                if (searchExampleText != null && searchExampleText.Count > 0)
                {
                    Model.SearchExampleText = searchExampleText.First();
                }                
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "HomePageAdapter.GetHomePageAdaptor()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return Model;
        }
    }
}
