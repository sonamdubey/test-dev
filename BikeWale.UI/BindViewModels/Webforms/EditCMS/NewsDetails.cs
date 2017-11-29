using Bikewale.BAL.EditCMS;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Location;
using Bikewale.Entities.SEO;
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
    /// Modified By : Sushil Kumar on 2nd Jan 2016
    /// Description : Modified maka and model ffetch logic to get details from model and make helper;
    ///                 Also removed send mail statements as log are maintained in graylog
    /// </summary>
    public class NewsDetails
    {
        public BikeMakeEntityBase TaggedMake;
        public BikeModelEntityBase TaggedModel;
        public ArticleDetails ArticleDetails { get; set; }
        public uint BasicId;
        public bool IsContentFound, IsPageNotFound, IsPermanentRedirect;
        public GlobalCityAreaEntity CityArea { get; set; }
        public PageMetaTags PageMetas { get; set; }
        public string MappedCWId { get; set; }
        private readonly ICMSCacheContent _cache = null;
        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : Fetch required aticles list
        /// Modified By : Sushil Kumar on 16th Nov 2016
        /// Description : Handle thread abort 
        /// </summary>
        public NewsDetails()
        {
            CityArea = GlobalCityArea.GetGlobalCityArea();
            if (ProcessQueryString())
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>()
                             .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                             .RegisterType<ICacheManager, MemcacheManager>();
                    _cache = container.Resolve<ICMSCacheContent>();
                }
            }
        }
        /// <summary>
        /// Written By : Ashwini Todkar on 24 Sept 2014
        /// PopulateWhere to fetch news details from api asynchronously
        /// </summary>
        public void GetNewsArticleDetails()
        {
            try
            {
                ArticleDetails = _cache.GetNewsDetails(BasicId);

                if (ArticleDetails != null)
                {
                    IsContentFound = true;
                    GetTaggedBikeListByMake();
                    GetTaggedBikeListByModel();
                    CreateMetaTags();
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err,"Bikewale.BindViewModels.Webforms.EditCMS.GetNewsArticleDetails");
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
                PageMetas.CanonicalUrl = string.Format("https://www.bikewale.com/news/{0}-{1}.html", ArticleDetails.BasicId, ArticleDetails.ArticleUrl);
                PageMetas.NextPageUrl = string.Format("/news/{0}-{1}.html", ArticleDetails.NextArticle.BasicId, ArticleDetails.NextArticle.ArticleUrl);
                PageMetas.PreviousPageUrl = string.Format("/news/{0}-{1}.html", ArticleDetails.PrevArticle.BasicId, ArticleDetails.PrevArticle.ArticleUrl);
                PageMetas.AlternateUrl = string.Format("https://www.bikewale.com/m/news/{0}-{1}.html", ArticleDetails.BasicId, ArticleDetails.ArticleUrl);
                PageMetas.AmpUrl = String.Format("{0}/m/news/{1}-{2}/amp/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, ArticleDetails.ArticleUrl, ArticleDetails.BasicId);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"] + "Bikewale.BindViewModels.Webforms.EditCMS.CreateMetaTags");
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : To get tagged bike along with article by make
        /// </summary>
        private void GetTaggedBikeListByMake()
        {
            try
            {
                if (ArticleDetails != null && ArticleDetails.VehiclTagsList != null && ArticleDetails.VehiclTagsList.Count > 0)
                {

                    var taggedMakeObj = ArticleDetails.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.MakeBase.MaskingName));
                    if (taggedMakeObj != null)
                    {
                        TaggedMake = taggedMakeObj.MakeBase;
                    }
                    else
                    {
                        TaggedMake = ArticleDetails.VehiclTagsList.FirstOrDefault().MakeBase;
                        if (TaggedMake != null)
                            TaggedMake = new Bikewale.Common.MakeHelper().GetMakeNameByMakeId((uint)TaggedMake.MakeId);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex,"Bikewale.BindViewModels.Webforms.EditCMS.GetTaggedBikeList");
            }
        }


        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : To get tagged bike along with article by model
        /// </summary>
        private void GetTaggedBikeListByModel()
        {
            try
            {
                if (ArticleDetails != null && ArticleDetails.VehiclTagsList != null && ArticleDetails.VehiclTagsList.Count > 0)
                {

                    var taggedModelObj = ArticleDetails.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.ModelBase.MaskingName));
                    if (taggedModelObj != null)
                    {
                        TaggedModel = taggedModelObj.ModelBase;
                    }
                    else
                    {
                        TaggedModel = ArticleDetails.VehiclTagsList.FirstOrDefault().ModelBase;
                        if (TaggedModel != null)
                            TaggedModel = new Bikewale.Common.ModelHelper().GetModelDataById((uint)TaggedModel.ModelId);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.EditCMS.GetTaggedBikeList");
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : Process query string for article id
        /// Modified By : Sushil Kumar on 16th Nov 2016
        /// Description : Handle thread abort by passing processquerystring status
        /// </summary>
        private bool ProcessQueryString()
        {
            var request = HttpContext.Current.Request;
            string qsBasicId = request.QueryString["id"];
            try
            {
                qsBasicId = BasicIdMapping.GetCWBasicId(qsBasicId);
                if (!qsBasicId.Equals(request.QueryString["id"]))
                {
                    IsPermanentRedirect = true;
                    MappedCWId = qsBasicId;
                    return false;
                }
                IsPageNotFound = !(uint.TryParse(qsBasicId, out BasicId) && BasicId > 0);
                return !IsPageNotFound;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.EditCMS.ProcessQueryString");
                return false;
            }
        }


    }
}