using Bikewale.BAL.EditCMS;
using Bikewale.BAL.GenericBikes;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Cache.GenericBikes;
using Bikewale.DAL.NewBikeSearch;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.SEO;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.GenericBikes;
using Bikewale.Interfaces.NewBikeSearch;
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
    public class BestBikesListing
    {
        private readonly IBestBikes objBestBikesBal = null;

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
        /// Created by  :   Sushil Kumar on 22 Dec 2016
        /// Description :   
        /// </summary>
        public BestBikesListing()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBestBikes, BestBikesBL>()
                    .RegisterType<IBestBikesCacheRepository, BestBikesCacheRepository>()
                    .RegisterType<ISearchResult, SearchResult>()
                    .RegisterType<ICacheManager, MemcacheManager>()
                    .RegisterType<IArticles, Articles>()
                    .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                    .RegisterType<IProcessFilter, ProcessFilter>();
                objBestBikesBal = container.Resolve<IBestBikes>();
            }

            ParseQueryString();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ParseQueryString()
        {
            var request = HttpContext.Current.Request;
            string _biketype = request.QueryString["biketype"];
            try
            {

                if (!string.IsNullOrEmpty(_biketype))
                {
                    BodyStyleType = GenericBikesCategoriesMapping.GetBodyStyleByBikeType(_biketype);
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BindViewModels.Webforms.GenericBikes.ProcessQueryString");
                //objErr.SendMail();
            }
        }

        /// <summary>
        /// 
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
        /// </summary>
        public void FetchBestBikesList(ushort topCount)
        {
            try
            {
                if (objBestBikesBal != null)
                {
                    objBestBikesList = objBestBikesBal.BestBikesByType(BodyStyleType).Take(topCount);
                    if (objBestBikesList != null && objBestBikesList.Count() > 0)
                    {
                        FetchedRecordCount = objBestBikesList.Count();

                    }
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("FetchBestBikesList{0} ", BodyStyleType));
                //objErr.SendMail();
            }
        }

        private void SetPageMetas(string pgName, string pgMaskingName)
        {
            string formattedDate = Bikewale.Utility.FormatDate.GetFormatDate(DateTime.Now.ToString(), "Y");
            PageMetas.Description = string.Format("BikeWale brings the list of best {0} in  India for {1}. Explore the top 10 {0} to buy the best bike of your  choice.", pgMaskingName, formattedDate);
            PageMetas.Title = string.Format("Best {0} in India - {1} | Top 10 Bikes - BikeWale", pgName, formattedDate);
            PageMetas.CanonicalUrl = string.Format("https://www.bikewale.com/best-{0}-in-india/", pgMaskingName);
            PageMetas.AlternateUrl = string.Format("https://www.bikewale.com/m/best-{0}-in-india/", pgMaskingName);

        }


    }
}