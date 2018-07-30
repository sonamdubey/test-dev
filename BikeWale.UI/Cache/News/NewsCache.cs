using Bikewale.Entities.CMS.Articles;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.News;
using Enyim.Caching;
using System;

namespace Bikewale.Cache.News
{
    /// <summary>
    /// Author : Vivek Gupta
    /// Date : 19-07-2016
    /// Desc : GRPC caching for news
    /// </summary>
    public class NewsCache : INewsCache
    {
        static readonly ILog _logger = LogManager.GetLogger(typeof(NewsCache));
        private CMSContent objNews = null;

        private readonly INews _news = null;
        private readonly ICacheManager _cache = null;
        public NewsCache(ICacheManager cache, INews news)
        {
            _cache = cache;
            _news = news;
        }

        /// <summary>
        /// Author : Vivek Gupta
        /// Date : 19-07-2016
        /// Desc : GRPC caching for news
        /// </summary>
        /// <param name="_startIndex"></param>
        /// <param name="_endIndex"></param>
        /// <returns></returns>
        //public CMSContent GetNews(int _startIndex, int _endIndex, string contentTypeList, int modelid = 0)
        //{
        //    string cacheKey = string.Empty;
        //    try
        //    {
        //        if (modelid <= 0)
        //        {
        //            cacheKey = String.Format("BW_News_SI_{0}_EI_{1}_CL_{2}", _startIndex, _endIndex, contentTypeList.Replace(",", "_"));
        //        }
        //        else
        //        {
        //            cacheKey = String.Format("BW_News_SI_{0}_EI_{1}_CL_{2}_M_{3}", _startIndex, _endIndex, contentTypeList.Replace(",", "_"), modelid);
        //        }

        //        objNews = _cache.GetFromCache<CMSContent>(cacheKey, new TimeSpan(0, 30, 0), () => _news.GetNews(_startIndex, _endIndex, contentTypeList, modelid));
        //    }
        //    catch (Exception err)
        //    {
        //        _logger.Error(err.Message, err);
        //    }

        //    return objNews;
        //}
    }
}
