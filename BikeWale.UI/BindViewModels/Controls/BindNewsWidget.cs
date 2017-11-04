using Bikewale.BAL.EditCMS;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Controls
{
    public class BindNewsWidget
    {
        public uint TotalRecords { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }

        /// <summary>
        /// Written By : Sangram Nandkhile on 20 May 2016
        /// Summary : Function to get the data from the carwale apis. This data is cached on the bikewale.
        /// </summary>
        /// <param name="rptr"></param>
        public ArticleSummary BindNews(Repeater rptr, int Takecount)
        {
            FetchedRecordsCount = 0;
            ArticleSummary firstArticle = null;
            try
            {

                IEnumerable<ArticleSummary> _objArticleList = null;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>()
                            .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                            .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent _articles = container.Resolve<ICMSCacheContent>();

                    _objArticleList = _articles.GetMostRecentArticlesByIdList(Convert.ToString((int)EnumCMSContentType.News), Convert.ToUInt32(TotalRecords), Convert.ToUInt32(MakeId), Convert.ToUInt32(ModelId));
                }

                if (_objArticleList != null && _objArticleList.Any())
                {
                    FetchedRecordsCount = _objArticleList.Count();
                    rptr.DataSource = _objArticleList;
                    rptr.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            return firstArticle;
        }
    }
}