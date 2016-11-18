
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
        public uint BasicId = 0;
        public String baseUrl = String.Empty, pageTitle = String.Empty, modelName = String.Empty, modelUrl = String.Empty;
        public String data = String.Empty, nextPageUrl = String.Empty, prevPageUrl = String.Empty, author = String.Empty, displayDate = String.Empty, canonicalUrl = String.Empty, title = String.Empty, description = String.Empty, keywords = String.Empty;
        public StringBuilder bikeTested;
        public ArticlePageDetails objTipsAndAdvice;
        public IEnumerable<ModelImage> objImg = null;
        public bool isContentFound = true, pageNotFound = false;
        private ICMSCacheContent _objDetailsBikeCarecache;
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
                _objDetailsBikeCarecache = container.Resolve<ICMSCacheContent>();
            }
            if (ProcessQueryString())
            {
                GetTipsAndAdviceDetails();
                CreateMetas();
            }




        }
        /// <summary>
        /// Created By:-Subodh Jain 12 Nov 2016
        /// Summary :- Detaile Page for TipsAndAdvice CreateMetas
        /// </summary>
        private void CreateMetas()
        {
            title = string.Format("{0} |Maintenance Tips from Bike Experts - BikeWale", pageTitle);
            description = string.Format("Read about {0}. Read through more bike care tips to learn more about your bike maintenance.", pageTitle);
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
                pageNotFound = true;


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


                objTipsAndAdvice = _objDetailsBikeCarecache.GetArticlesDetails(BasicId);

                if (objTipsAndAdvice != null)
                {
                    GetTipsAndAdviceData();
                    objImg = _objDetailsBikeCarecache.GetArticlePhotos(Convert.ToInt32(BasicId));
                }
                else
                {
                    isContentFound = false;
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

            canonicalUrl = string.Format("http://www.bikewale.com/bike-care/{0}-{1}.html", objTipsAndAdvice.ArticleUrl, BasicId.ToString());
            data = objTipsAndAdvice.Description;
            author = objTipsAndAdvice.AuthorName;
            pageTitle = objTipsAndAdvice.Title;
            displayDate = objTipsAndAdvice.DisplayDate.ToString();

        }
    }
}