using Bikewale.Entities.CMS;
using Bikewale.Interfaces.CMS;
using Bikewale.Notifications;
using System;
using System.Linq;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 23 Mar 2017
    /// Summary    : Model to get list of news article for partial view
    /// </summary>
    public class RecentNews
    {
        private readonly ICMSCacheContent _articles = null;
        private readonly uint _totalRecords;
        private readonly uint _makeId;
        private readonly uint _modelId;
        private readonly string _makeName;
        private readonly string _makeMasking;
        private readonly string _modelName;
        private readonly string _modelMasking;

        #region Constructor

        public RecentNews(ICMSCacheContent articles)
        {
            _articles = articles;
        }
        public RecentNews(uint totalRecords, ICMSCacheContent articles)
        {
            _totalRecords = totalRecords;
            _articles = articles;
        }
        public RecentNews(uint totalRecords, uint makeId, uint modelId, string makeName, string makeMasking, string modelName, string modelMasking)
        {
            _totalRecords = totalRecords;
            _makeId = makeId;
            _modelId = modelId;
            _makeName = makeName;
            _makeMasking = makeMasking;
            _modelName = modelName;
            _modelMasking = modelMasking;

        }
        #endregion

        #region Functions to get data
        /// <summary>
        /// Created by : Aditi Srivastava on 23 Mar 2017
        /// Summary    : To get list of news articles
        /// </summary>
        public RecentNewsVM GetData()
        {
            RecentNewsVM recentNews = new RecentNewsVM();
            try
            {
                recentNews.ArticlesList = _articles.GetMostRecentArticlesByIdList(Convert.ToString((int)EnumCMSContentType.News), _totalRecords, _makeId, _modelId);
                if (_makeId > 0)
                {
                    recentNews.MakeName = _makeName;
                    recentNews.MakeMasking = _makeMasking;
                }

                if (_modelId > 0)
                {
                    recentNews.ModelName = _modelName;
                    recentNews.ModelMasking = _modelMasking;
                }
                recentNews.FetchedCount = recentNews.ArticlesList.Count();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.News.RecentNews.GetData: TotalRecords {0},MakeId {1}, ModelId {2}", _totalRecords, _makeId, _modelId));
            }
            return recentNews;
        }
        #endregion
    }
}