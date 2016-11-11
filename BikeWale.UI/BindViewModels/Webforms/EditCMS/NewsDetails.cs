﻿using Bikewale.BAL.EditCMS;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Location;
using Bikewale.Entities.SEO;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Memcache;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Web;

namespace Bikewale.BindViewModels.Webforms.EditCMS
{
    /// <summary>
    /// Created By : Sushil Kumar on 10th Nov 2016
    /// Description : Common logic to bind news details page
    /// </summary>
    public class NewsDetails
    {

        public BikeMakeEntityBase TaggedMake;
        public ArticleDetails ArticleDetails { get; set; }
        public uint BasicId;
        public bool IsContentFound { get; set; }
        public bool IsPageNotFound { get; set; }
        public GlobalCityAreaEntity CityArea { get; set; }
        public PageMetaTags PageMetas { get; set; }

        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : Fetch required aticles list
        /// </summary>
        public NewsDetails()
        {
            CityArea = GlobalCityArea.GetGlobalCityArea();
            ProcessQueryString();
            if (!IsPageNotFound && BasicId > 0)
                GetNewsArticleDetails();
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 24 Sept 2014
        /// PopulateWhere to fetch news details from api asynchronously
        /// </summary>
        private void GetNewsArticleDetails()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>()
                            .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                            .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                    ArticleDetails = _cache.GetNewsDetails(BasicId);

                    if (ArticleDetails != null)
                    {
                        IsContentFound = true;
                        GetTaggedBikeList();
                        CreateMetaTags();
                    }

                }

            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"] + "Bikewale.BindViewModels.Webforms.EditCMS.GetNewsArticleDetails");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : Common logic to bind meta tags
        /// </summary>
        private void CreateMetaTags()
        {
            PageMetas = new PageMetaTags();

            try
            {
                PageMetas.Title = string.Format("{0} - BikeWale News", ArticleDetails.Title);
                PageMetas.ShareImage = Image.GetPathToShowImages(ArticleDetails.OriginalImgUrl, ArticleDetails.HostUrl, Bikewale.Utility.ImageSize._640x348);
                PageMetas.Description = string.Format("BikeWale coverage on {0}. Get the latest reviews and photos for {0} on BikeWale coverage.", ArticleDetails.Title);
                PageMetas.CanonicalUrl = string.Format("http://www.bikewale.com/news/{0}-{1}.html", ArticleDetails.BasicId, ArticleDetails.ArticleUrl);
                PageMetas.NextPageUrl = string.Format("/news/{0}-{1}.html", ArticleDetails.PrevArticle.BasicId, ArticleDetails.PrevArticle.ArticleUrl);
                PageMetas.PreviousPageUrl = string.Format("/news/{0}-{1}.html", ArticleDetails.NextArticle.BasicId, ArticleDetails.NextArticle.ArticleUrl);
                PageMetas.AlternateUrl = string.Format("http://www.bikewale.com/m/news/{0}-{1}.html", ArticleDetails.BasicId, ArticleDetails.ArticleUrl); ;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "Bikewale.BindViewModels.Webforms.EditCMS.CreateMetaTags");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : To get tagged bike along with article
        /// </summary>
        private void GetTaggedBikeList()
        {
            try
            {
                if (ArticleDetails != null && ArticleDetails.VehiclTagsList.Count > 0)
                {

                    var taggedMakeObj = ArticleDetails.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.MakeBase.MaskingName));
                    if (taggedMakeObj != null)
                    {
                        TaggedMake = taggedMakeObj.MakeBase;
                    }
                    else
                    {
                        TaggedMake = ArticleDetails.VehiclTagsList.FirstOrDefault().MakeBase;
                        FetchMakeDetails();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "Bikewale.BindViewModels.Webforms.EditCMS.GetTaggedBikeList");
                objErr.SendMail();
            }
        }


        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : Fetch make details from makeID
        /// </summary>
        private void FetchMakeDetails()
        {
            try
            {

                if (TaggedMake != null && TaggedMake.MakeId > 0)
                {

                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>();
                        var makesRepository = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();
                        TaggedMake = makesRepository.GetMakeDetails(TaggedMake.MakeId.ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "Bikewale.BindViewModels.Webforms.EditCMS.FetchMakeDetails");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : Process query string for article id
        /// </summary>
        private void ProcessQueryString()
        {
            var request = HttpContext.Current.Request;

            try
            {
                string _basicId = request.QueryString["id"];

                if (!string.IsNullOrEmpty(_basicId))
                {
                    //Check if basic id exists in mapped carwale basic id log **/
                    _basicId = BasicIdMapping.GetCWBasicId(_basicId);

                    //if id exists then redirect url to new basic id url
                    if (!_basicId.Equals(request["id"]))
                    {
                        string newUrl = string.Format("/news/{0}-{1}.html", _basicId, request["t"]);
                        Bikewale.Common.CommonOpn.RedirectPermanent(newUrl);
                    }
                    uint.TryParse(_basicId, out BasicId);
                }
                else
                {
                    IsPageNotFound = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "Bikewale.BindViewModels.Webforms.EditCMS.ProcessQueryString");
                objErr.SendMail();
            }
        }


    }
}