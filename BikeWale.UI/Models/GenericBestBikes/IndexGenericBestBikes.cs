
using Bikewale.Entities;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Notifications;
using Bikewale.Utility;
using Bikewale.Utility.GenericBikes;
using System;
using System.Collections.Generic;
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
        private readonly IBikeMakesCacheRepository _bikeMakes = null;
        private readonly ICMSCacheContent _objArticles = null;

        public ushort makeTopCount { get; set; }
        public bool IsMobile { get; set; }
        public StatusCodes status { get; set; }
        private EnumBikeBodyStyles BodyStyleType = EnumBikeBodyStyles.AllBikes;
        private string _modelIdList = string.Empty;

        /// <summary>
        /// Modified by sajal Gupta on 1-11-2017
        /// Description : Added objArticles
        /// </summary>
        /// <param name="objBestBikes"></param>
        /// <param name="bikeMakes"></param>
        /// <param name="objArticles"></param>
        public IndexGenericBestBikes(IBikeModelsCacheRepository<int> objBestBikes, IBikeMakesCacheRepository bikeMakes, ICMSCacheContent objArticles)
        {
            _objBestBikes = objBestBikes;
            _bikeMakes = bikeMakes;
            _objArticles = objArticles;
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
                ErrorClass.LogError(ex, "Bikewale.IndexGenericBestBikes.ParseQueryString");
            }
        }
        /// <summary>
        /// Created By :- Subodh Jain 18 May 2017
        /// Summary :- Generic Bike Model GetData;        
        /// Modified By :Sajal Gupta on 1-11-2017
        /// Description: Gneric bikes news widget
        /// Modified by : Snehal Dange on 28th Dec 2017
        /// Description: added ga pages 
        /// </summary>
        /// <param name="objUpcoming"></param>
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
                obj.BestBikes = new BestBikeWidgetModel(null, _objBestBikes).GetData();
                obj.BestBikes.CurrentPage = BodyStyleType;
                obj.Brands = new BrandWidgetModel(makeTopCount, _bikeMakes).GetData(BodyStyleType == EnumBikeBodyStyles.Scooter ? Entities.BikeData.EnumBikeType.Scooters : Entities.BikeData.EnumBikeType.New);
                obj.Page = Entities.Pages.GAPages.Best_Bikes;
                SetPageJSONLDSchema(obj);
                obj.News = new RecentNews(5, 0, _modelIdList, _objArticles).GetData();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.IndexGenericBestBikes.GetData");
            }

            return obj;

        }

        /// <summary>
        /// Created By  : Sushil Kumar on 15th Aug 2017
        /// Description : To load json schema for the list items
        /// </summary>
        /// <param name="obj"></param>
        private void SetPageJSONLDSchema(IndexBestBikesVM obj)
        {
            try
            {
                if (obj.objBestBikesList != null)
                {
                    ProductList objSchema = new ProductList();
                    objSchema.NumberOfItems = 10;
                    objSchema.Url = IsMobile ? obj.PageMetaTags.AlternateUrl : obj.PageMetaTags.CanonicalUrl;

                    if (IsMobile)
                        objSchema.CanonicalUrl = obj.PageMetaTags.CanonicalUrl;

                    objSchema.Name = obj.PageName;
                    var lstItems = new List<ProductListItem>();
                    uint itemNo = (uint)obj.objBestBikesList.Count();

                    foreach (var bike in obj.objBestBikesList)
                    {
                        var product = new Product();
                        product.Name = bike.BikeName;
                        product.Image = Bikewale.Utility.Image.GetPathToShowImages(bike.OriginalImagePath, bike.HostUrl, ImageSize._310x174);
                        product.Id = string.Format("{0}{1}", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, Bikewale.Utility.UrlFormatter.BikePageUrl(bike.Make.MaskingName, bike.Model.MaskingName));

                        product.Description = bike.SmallModelDescription;
                        product.AggregateOffer = new AggregateOffer()
                        {
                            HighPrice = bike.Price,
                            LowPrice = bike.Price
                        };
                        product.Url = string.Format("{0}#bike{1}", objSchema.Url, itemNo);
                        lstItems.Add(new ProductListItem()
                        {
                            Position = itemNo,
                            Item = product
                        });

                        _modelIdList = string.IsNullOrEmpty(_modelIdList) ? Convert.ToString(bike.Model.ModelId) : string.Format("{0},{1}", _modelIdList, bike.Model.ModelId);

                        itemNo--;
                    }
                    objSchema.ItemListElement = lstItems;
                    obj.PageMetaTags.SchemaJSON = Newtonsoft.Json.JsonConvert.SerializeObject(objSchema);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.IndexGenericBestBikes.SetPageJSONLDSchema");
            }
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

                case EnumBikeBodyStyles.Cruiser:
                    obj.Content = @"The lure of the open road, with the wind in your hair and the countryside passing you by… that is what cruisers are made for. There’s a cruiser for pockets of every size – which one will truly allow you kick back and enjoy the experience, though? BikeWale brings you a list of top 10 cruiser bikes in India. The list has been curated from the enormous amount of data being generated while researching cruisers on BikeWale. We bring you information about ex-showroom price, colors, variants, monthly units sold, popularity and launch date of the best cruisers to help you pick the best one. To buy the best cruisers, BikeWale recommends you to explore the list of best cruisers in India. Just remember: it’s all in the journey, so take your time!";
                    obj.pqSource = PQSourceEnum.Desktop_Generic_CruiserBikes;
                    obj.bannerImage = "https://imgd.aeplcdn.com/0x0/bw/static/landing-banners/d/cruiser-style-banner.jpg";
                    break;
                case EnumBikeBodyStyles.Mileage:
                    obj.Content = "Do you care about fuel efficiency? Well, we care about your choices. The ‘best mileage bikes in India’ is a list curated from online search data of more than 50 lakh users across India. Mileage is an important criterion a buyer keeps in mind while buying a bike. One also needs to consider other parameters like engine power, cost of maintenance etc. to make the best choice. We bring you information about ex-showroom price, colors, variants, monthly units sold, popularity and launch date of best mileage bikes to help you pick the best one. To buy the best mileage bikes, BikeWale recommends you to explore our list of best mileage bikes in India.";
                    obj.pqSource = PQSourceEnum.Desktop_Generic_MileageBikes;
                    obj.bannerImage = "https://imgd.aeplcdn.com/0x0/bw/static/landing-banners/d/mileage-bikes-banner.jpg";
                    break;
                case EnumBikeBodyStyles.Scooter:
                    obj.Content = @"Whether you live in a metro or a small town, you will find lots of scooters around! Scooters in India have gained immense popularity in the last decade. With more than 10 brands and over 50 models, it gets really difficult to pick the best scooter. We have more than 50 lakh people researching scooters on BikeWale every month, so this list of best scooters in India is made out of our users’ choice and truly reflects the popularity of scooters. We bring you information about ex-showroom price, colors, variants, monthly units sold, popularity and launch date of best scooters to help you pick the best one. Have a look at the list of best scooters in India to find the most suitable scooter for you.";
                    obj.pqSource = PQSourceEnum.Desktop_Generic_Scooters;
                    obj.bannerImage = "https://imgd.aeplcdn.com/0x0/bw/static/landing-banners/d/scooter-style-banner.jpg";
                    break;
                case EnumBikeBodyStyles.Sports:
                    obj.Content = @"All of us have dreamt of buying a sports bike at some point. The difficult question to answer has always been ‘which is the best sports bike?’. BikeWale brings you a list of top 10 sports bikes in India. The list has been curated from the enormous amount of data being generated while researching sports bikes on BikeWale. We bring you information about ex-showroom price, colors, variants, monthly units sold, popularity and launch date of the best sports bikes to help you pick the best one. To buy the best sports bikes, BikeWale recommends you to explore the list of best sports bikes in India.";
                    obj.pqSource = PQSourceEnum.Desktop_Generic_SportsBikes;
                    obj.bannerImage = "https://imgd.aeplcdn.com/0x0/bw/static/landing-banners/d/sports-style-banner.jpg";
                    break;
                default: //EnumBikeBodyStyles.AllBikes:
                    obj.Content = @"When you have more than 200 different motorcycle models, it gets difficult to choose the best bike. With an enormous amount of data being generated everyday by bike buyers in India on BikeWale, this list of best bikes truly reflects the popularity of bikes in India. We bring you information about ex-showroom price, colors, variants, monthly units sold, popularity and launch date of best bikes to help you pick the best one. Explore the list of best bikes in India to find a suitable bike for your needs.";
                    obj.pqSource = PQSourceEnum.Desktop_Generic_Bikes;
                    obj.bannerImage = "https://imgd.aeplcdn.com/0x0/bw/static/landing-banners/d/best-bikes-banner.jpg";
                    break;

            }
        }

        /// <summary>
        /// Created By :- Subodh Jain 18 May 2017
        /// Summary :- Generic Bike Model FetchBestBikesList;
        /// </summary>
        private void FetchBestBikesList(IndexBestBikesVM obj)
        {
            uint cityId = 0;
            try
            {
                GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();

                cityId = location != null ? location.CityId : 0;
                int topCount = 10;
                obj.objBestBikesList = _objBestBikes.GetBestBikesByCategory(BodyStyleType, cityId);
                if (obj.objBestBikesList != null)
                    obj.objBestBikesList = obj.objBestBikesList.Take(topCount);

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("FetchBestBikesList BodyStyle:{0} cityId:{1}", BodyStyleType, cityId));
            }
        }

        /// <summary>
        /// Modified By: Snehal Dange on 29th Nov 2017
        /// Description: Changed logic for current month on bestbike page
        /// </summary>
        /// <param name="obj"></param>
        private void SetPageMetas(IndexBestBikesVM obj)
        {
            try
            {
                string formattedDate = Bikewale.Utility.FormatDate.GetFormatDate(DateTime.Now.ToString(), "Y");
                obj.PageMetaTags.Description = string.Format("BikeWale brings the list of best {0} in  India for {1}. Explore the top 10 {0} to buy the best bike of your  choice.", obj.PageMaskingName, formattedDate);
                obj.PageMetaTags.Title = string.Format("Best {0} in India - {1} | Top 10 {0} - BikeWale", obj.PageName, formattedDate);
                obj.PageMetaTags.CanonicalUrl = string.Format("{0}/best-{1}-in-india/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, obj.PageMaskingName);
                obj.PageMetaTags.AlternateUrl = string.Format("{0}/m/best-{1}-in-india/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, obj.PageMaskingName);
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, string.Format("SetPageMetas{0} ", BodyStyleType));
            }

        }

    }
}