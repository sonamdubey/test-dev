using Bikewale.BAL.BikeData;
using Bikewale.BAL.Pager;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.SEO;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Pager;
using Bikewale.Notifications;
using Bikewale.Utility.GenericBikes;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Bikewale.BindViewModels.Webforms.GenericBikes
{
    /// <summary>
    /// Created By : Sushil Kumar on 22nd Dec 2016
    /// Description : View model for generic page
    /// Modified by : Aditi srivastava on 17 Jan 2017
    /// Description : Removed BL function call and 
    ///      added cache function for top bikes listing
    ///Modified By :- Subodh Jain 30 jan 2017
    ///Summary:- Shifted generic to bikemodel repository
    /// </summary>
    public class BestBikesListing
    {
        private readonly IBikeModelsCacheRepository<int> _objBestBikes = null;
        public IEnumerable<BestBikeEntityBase> objBestBikesList = null;

        public int FetchedRecordCount = 0;
        public ushort TotalCount { get; set; }
        public bool IsPageNotFound, IsPermanentRedirect;
        public string PageRedirectionUrl = string.Empty;
        public ushort BikeBodyStyleId = default(ushort);
        public PageMetaTags PageMetas = new PageMetaTags();
        public string PageMaskingName = string.Empty, PageName = string.Empty;
        public string PageContent { get; set; }
        public EnumBikeBodyStyles BodyStyleType = EnumBikeBodyStyles.AllBikes;

        /// <summary>
        /// Created By : Sushil Kumar on 22nd DEc 2016
        /// Description : REgister unity container for generic page 
        /// </summary>
        public BestBikesListing()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                         .RegisterType<ICacheManager, MemcacheManager>()
                         .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                         .RegisterType<IPager, Pager>()
                         .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>();
                _objBestBikes = container.Resolve<IBikeModelsCacheRepository<int>>();

            }

            ParseQueryString();
        }

        /// <summary>
        /// Created By : Sushil Kumar on 22nd Dec 2016
        /// Description : Parse query string to get bike type
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
                    BikeBodyStyleId = (ushort)BodyStyleType;
                    PageMaskingName = GenericBikesCategoriesMapping.BodyStyleByType(BodyStyleType);
                    PageName = new CultureInfo("en-US", false).TextInfo.ToTitleCase(PageMaskingName).Replace("-", " "); ;
                    SetPageMetas(PageName, PageMaskingName);
                    PageContent = SetPageContent(BodyStyleType);

                }
                else
                {
                    IsPageNotFound = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.GenericBikes.ParseQueryString");
                
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 22nd Dec 2016
        /// Description : Function to set page content type
        /// </summary>
        /// <param name="bodyType"></param>
        /// <returns></returns>
        private string SetPageContent(EnumBikeBodyStyles bodyType)
        {
            string content = string.Empty;
            switch (bodyType)
            {
                case EnumBikeBodyStyles.AllBikes:
                    content = "When you have more than 200 different motorcycle models, it gets difficult to choose the best bike. With an enormous amount of data being generated everyday by bike buyers in India on BikeWale, this list of best bikes truly reflects the popularity of bikes in India. We bring you information about ex-showroom price, colors, variants, monthly units sold, popularity and launch date of best bikes to help you pick the best one. Explore the list of best bikes in India to find a suitable bike for your needs.";
                    break;
                case EnumBikeBodyStyles.Cruiser:
                    content = "The lure of the open road, with the wind in your hair and the countryside passing you by… that is what cruisers are made for. There’s a cruiser for pockets of every size – which one will truly allow you kick back and enjoy the experience, though? BikeWale brings you a list of top 10 cruiser bikes in India. The list has been curated from the enormous amount of data being generated while researching cruisers on BikeWale. We bring you information about ex-showroom price, colors, variants, monthly units sold, popularity and launch date of the best cruisers to help you pick the best one. To buy the best cruisers, BikeWale recommends you to explore the list of best cruisers in India. Just remember: it’s all in the journey, so take your time!";
                    break;
                case EnumBikeBodyStyles.Mileage:
                    content = "Do you care about fuel efficiency? Well, we care about your choices. The ‘best mileage bikes in India’ is a list curated from online search data of more than 50 lakh users across India. Mileage is an important criterion a buyer keeps in mind while buying a bike. One also needs to consider other parameters like engine power, cost of maintenance etc. to make the best choice. We bring you information about ex-showroom price, colors, variants, monthly units sold, popularity and launch date of best mileage bikes to help you pick the best one. To buy the best mileage bikes, BikeWale recommends you to explore our list of best mileage bikes in India.";
                    break;
                case EnumBikeBodyStyles.Scooter:
                    content = "Whether you live in a metro or a small town, you will find lots of scooters around! Scooters in India have gained immense popularity in the last decade. With more than 10 brands and over 50 models, it gets really difficult to pick the best scooter. We have more than 50 lakh people researching scooters on BikeWale every month, so this list of best scooters in India is made out of our users’ choice and truly reflects the popularity of scooters. We bring you information about ex-showroom price, colors, variants, monthly units sold, popularity and launch date of best scooters to help you pick the best one. Have a look at the list of best scooters in India to find the most suitable scooter for you.";
                    break;
                case EnumBikeBodyStyles.Sports:
                    content = "All of us have dreamt of buying a sports bike at some point. The difficult question to answer has always been ‘which is the best sports bike?’. BikeWale brings you a list of top 10 sports bikes in India. The list has been curated from the enormous amount of data being generated while researching sports bikes on BikeWale. We bring you information about ex-showroom price, colors, variants, monthly units sold, popularity and launch date of the best sports bikes to help you pick the best one. To buy the best sports bikes, BikeWale recommends you to explore the list of best sports bikes in India.";
                    break;
                default:
                    content = "When you have more than 200 different motorcycle models, it gets difficult to choose the best bike. With an enormous amount of data being generated everyday by bike buyers in India on BikeWale, this list of best bikes truly reflects the popularity of bikes in India. We bring you information about ex-showroom price, colors, variants, monthly units sold, popularity and launch date of best bikes to help you pick the best one. Explore the list of best bikes in India to find a suitable bike for your needs.";
                    break;

            }
            return content;
        }


        /// <summary>
        /// Created by : Sushil Kumar on 22nd Dec 2016
        /// Desc: Fetch Bikes by body style and category
        /// Modified by : Aditi Srivastava on 17 Jan 2017
        /// Description : Used a different function for getting lust of top bikes
        /// Modified by : Sajal Gupta on 02-02-2017
        /// Description : Passed cityid to get used bikes count.     
        /// </summary>
        public void FetchBestBikesList(ushort topCount, uint? cityId = null)
        {
            try
            {
                if (_objBestBikes != null)
                {
                    objBestBikesList = _objBestBikes.GetBestBikesByCategory(BodyStyleType, cityId).Take(topCount);
                    if (objBestBikesList != null && objBestBikesList.Any())
                    {
                        FetchedRecordCount = objBestBikesList.Count();

                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("FetchBestBikesList{0} ", BodyStyleType));
                
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 22nd Dec 2016
        /// Description : Set page meta tags according to bike type
        /// </summary>
        private void SetPageMetas(string pgName, string pgMaskingName)
        {
            string formattedDate = Bikewale.Utility.FormatDate.GetFormatDate(DateTime.Now.AddMonths(-1).ToString(), "Y");
            PageMetas.Description = string.Format("BikeWale brings the list of best {0} in  India for {1}. Explore the top 10 {0} to buy the best bike of your  choice.", pgMaskingName, formattedDate);
            PageMetas.Title = string.Format("Best {0} in India - {1} | Top 10 {0} - BikeWale", pgName, formattedDate);
            PageMetas.CanonicalUrl = string.Format("https://www.bikewale.com/best-{0}-in-india/", pgMaskingName);
            PageMetas.AlternateUrl = string.Format("https://www.bikewale.com/m/best-{0}-in-india/", pgMaskingName);

        }


    }
}