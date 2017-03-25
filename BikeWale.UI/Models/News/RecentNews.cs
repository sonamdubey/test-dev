using Bikewale.Entities.CMS;
using Bikewale.Interfaces.CMS;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models.News
{
    /// <summary>
    /// Created by : Aditi Srivastava on 23 Mar 2017
    /// Summary    : Model to get list of news article for partial view
    /// </summary>
    public class RecentNews
    {
        private readonly ICMSCacheContent _articles = null;

        #region Constructor
        public RecentNews(ICMSCacheContent articles)
        {
            _articles = articles;
        }
        #endregion

        #region Functions to get data
        /// <summary>
        /// Created by : Aditi Srivastava on 23 Mar 2017
        /// Summary    : To get list of news articles
        /// </summary>
        public RecentNewsVM GetData(uint totalRecords, uint makeId, uint modelId, string makeName, string makeMasking, string modelName, string modelMasking)
        {
            RecentNewsVM recentNews = new RecentNewsVM();
            try
            {
                recentNews.ArticlesList = _articles.GetMostRecentArticlesByIdList(Convert.ToString((int)EnumCMSContentType.News), totalRecords, makeId, modelId);
                if (makeId > 0)
                {
                    recentNews.MakeName = makeName;
                    recentNews.MakeMasking = makeMasking;
                }

                if (modelId > 0)
                {
                    recentNews.ModelName = modelName;
                    recentNews.ModelMasking = modelMasking;
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.News.RecentNews.GetData: TotalRecords {0},MakeId {1}, ModelId {2}", totalRecords, makeId, modelId));
            }
            return recentNews;
        }
        #endregion
    }
}