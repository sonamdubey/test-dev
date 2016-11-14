
using Bikewale.BAL.EditCMS;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
namespace Bikewale.BindViewModels.Webforms.EditCMS
{
    /// <summary>
    /// Created By:-Subodh Jain 12 Nov 2016
    /// Summary :- Detaile Page for TipsAndAdvice
    /// </summary>
    public class DetailPageBikeCare
    {
        HttpContext page = HttpContext.Current;
        protected uint BasicId = 0;
        public String baseUrl = String.Empty, pageTitle = String.Empty, modelName = String.Empty, modelUrl = String.Empty;
        public String data = String.Empty, nextPageUrl = String.Empty, prevPageUrl = String.Empty, author = String.Empty, displayDate = String.Empty, canonicalUrl = String.Empty, title = String.Empty, description = String.Empty, keywords = String.Empty;
        public StringBuilder bikeTested;
        public ArticlePageDetails objTipsAndAdvice;
        public IEnumerable<ModelImage> objImg = null;
        private bool isContentFound = true;
        private ICMSCacheContent _cache;
        /// <summary>
        /// Created By:-Subodh Jain 12 Nov 2016
        /// Summary :- Detaile Page for TipsAndAdvice resolving interface
        /// </summary>
        public DetailPageBikeCare()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IArticles, Articles>()
                   .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                   .RegisterType<ICacheManager, MemcacheManager>();
                _cache = container.Resolve<ICMSCacheContent>();
            }
            if (ProcessQueryString())
            {
                GetTipsAndAdviceDetails();
            }
            else
            {

                page.Response.Redirect("/m/bike-care/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            CreateMetas();


        }
        /// <summary>
        /// Created By:-Subodh Jain 12 Nov 2016
        /// Summary :- Detaile Page for TipsAndAdvice CreateMetas
        /// </summary>
        private void CreateMetas()
        {
            title = string.Format("{0} |Maintenance Tips from Bike Experts - BikeWale", pageTitle);
            description = string.Format("BikeWale brings you tips to keep your bike in good shape. Read through this tip to learn more about your bike maintenance");
            keywords = string.Format("Bike maintenance, bike common issues, bike common problems, Maintaining bikes, bike care");

        }
        /// <summary>
        /// Created By:-Subodh Jain 12 Nov 2016
        /// Summary :- Detaile Page for TipsAndAdvice ProcessQueryString
        /// </summary>
        private bool ProcessQueryString()
        {
            bool isSuccess = true;
            if (!String.IsNullOrEmpty(page.Request.QueryString["id"]) && CommonOpn.CheckId(page.Request.QueryString["id"]))
            {

                BasicId = Convert.ToUInt32(page.Request.QueryString["id"]);
            }
            else
            {
                isSuccess = false;
            }

            return isSuccess;
        }
        /// <summary>
        /// Created By:-Subodh Jain 12 Nov 2016
        /// Summary :- to Get TipsAndAdvice Details 
        /// </summary>
        private void GetTipsAndAdviceDetails()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>()
                       .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                       .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                    objTipsAndAdvice = _cache.GetArticlesDetails(BasicId);

                    if (objTipsAndAdvice != null)
                    {
                        GetTipsAndAdviceData();
                        objImg = _cache.GetArticlePhotos(Convert.ToInt32(BasicId));
                    }
                    else
                    {
                        isContentFound = false;
                    }
                }
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "DetailPageBikeCare.GetTipsAndAdviceDetails");
                objErr.SendMail();
            }
            finally
            {
                if (!isContentFound)
                {
                    page.Response.Redirect("/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
        }
        /// <summary>
        /// Created By:-Subodh Jain 12 Nov 2016
        /// Summary :- to Get TipsAndAdvice data 
        /// </summary>
        private void GetTipsAndAdviceData()
        {
            baseUrl = "/m/bike-care/" + objTipsAndAdvice.ArticleUrl + '-' + BasicId.ToString() + "/";
            canonicalUrl = "http://www.bikewale.com/bike-care/" + objTipsAndAdvice.ArticleUrl + '-' + BasicId.ToString() + ".html";
            data = objTipsAndAdvice.Description;
            author = objTipsAndAdvice.AuthorName;
            pageTitle = objTipsAndAdvice.Title;
            displayDate = objTipsAndAdvice.DisplayDate.ToString();

            if (objTipsAndAdvice.VehiclTagsList != null && objTipsAndAdvice.VehiclTagsList.Count > 0)
            {
                if (objTipsAndAdvice.VehiclTagsList.Any(m => (m.MakeBase != null && !String.IsNullOrEmpty(m.MakeBase.MaskingName))))
                {
                    bikeTested = new StringBuilder();

                    bikeTested.Append("Bike Tested: ");

                    IEnumerable<int> ids = objTipsAndAdvice.VehiclTagsList
                           .Select(e => e.ModelBase.ModelId)
                           .Distinct();

                    foreach (var i in ids)
                    {
                        VehicleTag item = objTipsAndAdvice.VehiclTagsList.Where(e => e.ModelBase.ModelId == i).First();
                        if (!String.IsNullOrEmpty(item.MakeBase.MaskingName))
                        {
                            bikeTested.Append("<a title='" + item.MakeBase.MakeName + " " + item.ModelBase.ModelName + " Bikes' href='/m/" + item.MakeBase.MaskingName + "-bikes/" + item.ModelBase.MaskingName + "/'>" + item.ModelBase.ModelName + "</a>   ");
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Created By:-Subodh Jain 12 Nov 2016
        /// Summary :- to Get GetImageUrl
        /// </summary>
        protected string GetImageUrl(string hostUrl, string imagePath)
        {
            string imgUrl = String.Empty;
            imgUrl = Bikewale.Common.ImagingFunctions.GetPathToShowImages(imagePath, hostUrl);

            return imgUrl;
        }
    }
}