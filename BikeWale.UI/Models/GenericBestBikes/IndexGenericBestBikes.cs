
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Utility;
using Bikewale.Utility.GenericBikes;
using System;
using System.Globalization;
using System.Linq;
using System.Web;
namespace Bikewale.Models
{
    /// <summary>
    /// Created By :- Subodh Jain 18 May 2017
    /// Summary :- Generic Bike Model;
    /// </summary>
    public class IndexGenericBestBikes
    {
        private readonly IBikeModelsCacheRepository<int> _objBestBikes = null;
        private readonly IBikeMakes<BikeMakeEntity, int> _bikeMakes = null;


        public ushort makeTopCount { get; set; }

        public StatusCodes status;
        public EnumBikeBodyStyles BodyStyleType = EnumBikeBodyStyles.AllBikes;
        public IndexGenericBestBikes(IBikeModelsCacheRepository<int> objBestBikes, IBikeMakes<BikeMakeEntity, int> bikeMakes)
        {
            _objBestBikes = objBestBikes;
            _bikeMakes = bikeMakes;
            ParseQueryString();
        }

        /// <summary>
        /// Created By :- Subodh Jain 18 May 2017
        /// Summary :- Generic Bike Model ParseQueryString;
        /// </summary>
        private void ParseQueryString()
        {
            var request = HttpContext.Current.Request;
            string biketype = request.QueryString["biketype"];
            try
            {

                if (!string.IsNullOrEmpty(biketype))
                {
                    BodyStyleType = GenericBikesCategoriesMapping.GetBodyStyleByBikeType(biketype);
                    status = StatusCodes.ContentFound;
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.IndexGenericBestBikes.ParseQueryString");
            }
        }
        /// <summary>
        /// Created By :- Subodh Jain 18 May 2017
        /// Summary :- Generic Bike Model GetData;
        /// </summary>
        public IndexBestBikesVM GetData()
        {
            IndexBestBikesVM obj = new IndexBestBikesVM();
            try
            {
                obj.PageMaskingName = GenericBikesCategoriesMapping.BodyStyleByType(BodyStyleType);
                obj.PageName = new CultureInfo("en-US", false).TextInfo.ToTitleCase(obj.PageMaskingName).Replace("-", " "); ;
                SetPageMetas(obj);
                SetPageContent(obj);
                SetbannerImagePos(obj);
                FetchBestBikesList(obj);
                obj.BestBikes.CurrentPage = BodyStyleType;
                obj.Brands = new BrandWidgetModel(makeTopCount, _bikeMakes).GetData(BodyStyleType == EnumBikeBodyStyles.Scooter ? Entities.BikeData.EnumBikeType.Scooters : Entities.BikeData.EnumBikeType.New);
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "Bikewale.IndexGenericBestBikes.GetData");
            }
            return obj;

        }

        /// <summary>
        /// Created By :- Subodh Jain 18 May 2017
        /// Summary :- Generic Bike Model SetbannerImagePos;
        /// </summary>
        private void SetbannerImagePos(IndexBestBikesVM obj)
        {
            switch (BodyStyleType)
            {
                case EnumBikeBodyStyles.AllBikes:
                case EnumBikeBodyStyles.Sports:
                    obj.bannerImagePos = "center-pos";
                    break;
                case EnumBikeBodyStyles.Scooter:
                case EnumBikeBodyStyles.Mileage:
                    obj.bannerImagePos = "center-right-pos";
                    break;
                case EnumBikeBodyStyles.Cruiser:
                    obj.bannerImagePos = "left-center-pos";
                    break;
                default:
                    obj.bannerImagePos = "center-pos";
                    break;
            }

        }
        /// <summary>
        /// Created By :- Subodh Jain 18 May 2017
        /// Summary :- Generic Bike Model SetPageContent;
        /// </summary>
        private void SetPageContent(IndexBestBikesVM obj)
        {

            switch (BodyStyleType)
            {
                case EnumBikeBodyStyles.AllBikes:
                    obj.Content = "When you have more than 200 different motorcycle models, it gets difficult to choose the best bike. With an enormous amount of data being generated everyday by bike buyers in India on BikeWale, this list of best bikes truly reflects the popularity of bikes in India. We bring you information about ex-showroom price, colors, variants, monthly units sold, popularity and launch date of best bikes to help you pick the best one. Explore the list of best bikes in India to find a suitable bike for your needs.";
                    obj.pqSource = PQSourceEnum.Desktop_Generic_Bikes;
                    obj.bannerImage = "https://imgd2.aeplcdn.com/0x0/bw/static/landing-banners/d/best-bikes-banner.jpg";
                    break;
                case EnumBikeBodyStyles.Cruiser:
                    obj.Content = "The lure of the open road, with the wind in your hair and the countryside passing you by… that is what cruisers are made for. There’s a cruiser for pockets of every size – which one will truly allow you kick back and enjoy the experience, though? BikeWale brings you a list of top 10 cruiser bikes in India. The list has been curated from the enormous amount of data being generated while researching cruisers on BikeWale. We bring you information about ex-showroom price, colors, variants, monthly units sold, popularity and launch date of the best cruisers to help you pick the best one. To buy the best cruisers, BikeWale recommends you to explore the list of best cruisers in India. Just remember: it’s all in the journey, so take your time!";
                    obj.pqSource = PQSourceEnum.Desktop_Generic_CruiserBikes;
                    obj.bannerImage = "https://imgd3.aeplcdn.com/0x0/bw/static/landing-banners/d/cruiser-style-banner.jpg";
                    break;
                case EnumBikeBodyStyles.Mileage:
                    obj.Content = "Do you care about fuel efficiency? Well, we care about your choices. The ‘best mileage bikes in India’ is a list curated from online search data of more than 50 lakh users across India. Mileage is an important criterion a buyer keeps in mind while buying a bike. One also needs to consider other parameters like engine power, cost of maintenance etc. to make the best choice. We bring you information about ex-showroom price, colors, variants, monthly units sold, popularity and launch date of best mileage bikes to help you pick the best one. To buy the best mileage bikes, BikeWale recommends you to explore our list of best mileage bikes in India.";
                    obj.pqSource = PQSourceEnum.Desktop_Generic_MileageBikes;
                    obj.bannerImage = "https://imgd1.aeplcdn.com/0x0/bw/static/landing-banners/d/mileage-bikes-banner.jpg";
                    break;
                case EnumBikeBodyStyles.Scooter:
                    obj.Content = "Whether you live in a metro or a small town, you will find lots of scooters around! Scooters in India have gained immense popularity in the last decade. With more than 10 brands and over 50 models, it gets really difficult to pick the best scooter. We have more than 50 lakh people researching scooters on BikeWale every month, so this list of best scooters in India is made out of our users’ choice and truly reflects the popularity of scooters. We bring you information about ex-showroom price, colors, variants, monthly units sold, popularity and launch date of best scooters to help you pick the best one. Have a look at the list of best scooters in India to find the most suitable scooter for you.";
                    break;
                case EnumBikeBodyStyles.Sports:
                    obj.Content = "All of us have dreamt of buying a sports bike at some point. The difficult question to answer has always been ‘which is the best sports bike?’. BikeWale brings you a list of top 10 sports bikes in India. The list has been curated from the enormous amount of data being generated while researching sports bikes on BikeWale. We bring you information about ex-showroom price, colors, variants, monthly units sold, popularity and launch date of the best sports bikes to help you pick the best one. To buy the best sports bikes, BikeWale recommends you to explore the list of best sports bikes in India.";
                    obj.pqSource = PQSourceEnum.Desktop_Generic_SportsBikes;
                    obj.bannerImage = "https://imgd3.aeplcdn.com/0x0/bw/static/landing-banners/d/sports-style-banner.jpg";
                    break;
                default:
                    obj.Content = "When you have more than 200 different motorcycle models, it gets difficult to choose the best bike. With an enormous amount of data being generated everyday by bike buyers in India on BikeWale, this list of best bikes truly reflects the popularity of bikes in India. We bring you information about ex-showroom price, colors, variants, monthly units sold, popularity and launch date of best bikes to help you pick the best one. Explore the list of best bikes in India to find a suitable bike for your needs.";
                    obj.pqSource = PQSourceEnum.Desktop_Generic_Bikes;
                    obj.bannerImage = "https://imgd2.aeplcdn.com/0x0/bw/static/landing-banners/d/best-bikes-banner.jpg";
                    break;

            }
        }

        /// <summary>
        /// Created By :- Subodh Jain 18 May 2017
        /// Summary :- Generic Bike Model FetchBestBikesList;
        /// </summary>
        private void FetchBestBikesList(IndexBestBikesVM obj)
        {

            try
            {
                GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();

                uint cityId = location != null ? location.CityId : 0;
                int topCount = 10;
                obj.objBestBikesList = _objBestBikes.GetBestBikesByCategory(BodyStyleType, cityId).Take(topCount);

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("FetchBestBikesList{0} ", BodyStyleType));
            }
        }

        private void SetPageMetas(IndexBestBikesVM obj)
        {
            try
            {
                string formattedDate = Bikewale.Utility.FormatDate.GetFormatDate(DateTime.Now.AddMonths(-1).ToString(), "Y");
                obj.PageMetaTags.Description = string.Format("BikeWale brings the list of best {0} in  India for {1}. Explore the top 10 {0} to buy the best bike of your  choice.", obj.PageMaskingName, formattedDate);
                obj.PageMetaTags.Title = string.Format("Best {0} in India - {1} | Top 10 {0} - BikeWale", obj.PageName, formattedDate);
                obj.PageMetaTags.CanonicalUrl = string.Format("https://www.bikewale.com/best-{0}-in-india/", obj.PageMaskingName);
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("SetPageMetas{0} ", BodyStyleType));
            }

        }


    }
}