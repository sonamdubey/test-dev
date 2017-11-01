﻿using Bikewale.Entities.CMS;
using Bikewale.Entities.GenericBikes;
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
        private readonly string _title, _makeName, _makeMasking, _modelName, _modelMasking,_modelIdList;

        #region Constructor

        public RecentNews(uint totalRecords, ICMSCacheContent articles)
        {
            _totalRecords = totalRecords;
            _articles = articles;
        }

        public RecentNews(uint totalRecords, uint makeId, string makeName, string makeMasking, ICMSCacheContent articles)
        {
            _totalRecords = totalRecords;
            _makeId = makeId;
            _makeName = makeName;
            _makeMasking = makeMasking;
            _articles = articles;
        }

        public RecentNews(uint totalRecords, uint makeId, string makeName, string makeMasking, string title, ICMSCacheContent articles)
        {
            _totalRecords = totalRecords;
            _makeId = makeId;
            _makeName = makeName;
            _makeMasking = makeMasking;
            _title = title;
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

        public RecentNews(uint totalRecords, uint makeId, uint modelId, string makeName, string makeMasking, string modelName, string modelMasking, string title, ICMSCacheContent articles)
        {
            _totalRecords = totalRecords;
            _makeId = makeId;
            _modelId = modelId;
            _makeName = makeName;
            _makeMasking = makeMasking;
            _modelName = modelName;
            _modelMasking = modelMasking;
            _title = title;
            _articles = articles;

        }

        public RecentNews(uint totalRecords, uint makeId, string modelId,ICMSCacheContent articles)
        {
            _totalRecords = totalRecords;
            _makeId = makeId;
            _modelIdList = modelId;
            _articles = articles;



        }
        #endregion
        public bool IsScooter { get; set; }
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
                if(_modelIdList.Trim().Length>0)
                {
                    recentNews.ArticlesList = _articles.GetMostRecentArticlesByIdList(Convert.ToString((int)EnumCMSContentType.News), _totalRecords, _makeId, _modelIdList);
                }
                else
                {
                    if (IsScooter)
                    {
                    string bodyStyleId = ((int)EnumBikeBodyStyles.Scooter).ToString();
                    recentNews.ArticlesList = _articles.GetMostRecentArticlesByIdList(Convert.ToString((int)EnumCMSContentType.News), _totalRecords, bodyStyleId, _makeId, _modelId);
                    }
                    else
                        recentNews.ArticlesList = _articles.GetMostRecentArticlesByIdList(Convert.ToString((int)EnumCMSContentType.News), _totalRecords, _makeId, _modelId);

                }


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

                if (recentNews.ArticlesList != null)
                    recentNews.FetchedCount = recentNews.ArticlesList.Count();

                recentNews.Title = _title;
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