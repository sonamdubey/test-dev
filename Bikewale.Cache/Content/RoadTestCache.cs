using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Content;
using Enyim.Caching;
using System;
using System.Collections.Generic;

namespace Bikewale.Cache.Content
{
    /// <summary>
    /// Author : Vivek Gupta on 18-07-2016
    /// Desc: this class created for caching and used for feature details
    /// </summary>
    public class RoadTestCache : IRoadTestCache
    {
        static readonly ILog _logger = LogManager.GetLogger(typeof(RoadTestCache));
        private IEnumerable<ModelImage> objImg = null;

        private readonly IRoadTest _roadtest = null;
        private readonly ICacheManager _cache = null;
        public RoadTestCache(ICacheManager cache, IRoadTest roadtest)
        {
            _cache = cache;
            _roadtest = roadtest;
        }

        /// <summary>
        /// Author : Vivek Gupta on 18-07-2016
        /// Desc: this function moved from content/RoadTest/ViewRT.aspx.cs for caching and used for feature details
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public IEnumerable<ModelImage> BindPhotos(int basicId)
        {
            try
            {
                string cacheKey = String.Format("BW_RoadT_Photos_basicId_{0}", basicId);
                objImg = _cache.GetFromCache<IEnumerable<ModelImage>>(cacheKey, new TimeSpan(0, 30, 0), () => _roadtest.GetPhotos(basicId));
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }

            return objImg;
        }
    }
}
