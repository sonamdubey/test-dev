using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Content;
using Bikewale.Notifications;
using Enyim.Caching;
using System;
using System.Collections.Generic;

namespace Bikewale.Cache.Content
{
    public class FeaturesCache : IFeatureCache
    {

        static readonly ILog _logger = LogManager.GetLogger(typeof(FeaturesCache));
        private ArticlePageDetails objFeature = null;
        private IEnumerable<ModelImage> objImg = null;

        private readonly IFeatures _features = null;
        private readonly ICacheManager _cache = null;
        public FeaturesCache(ICacheManager cache, IFeatures features)
        {
            _cache = cache;
            _features = features;
        }

        /// <summary>
        /// Author : Vivek Gupta on 18-07-2016
        /// Desc: this function moved from content/features/view.aspx.cs for caching and used for feature details
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public ArticlePageDetails GetFeatureDetailsViaGrpc(int basicId)
        {
            try
            {
                string cacheKey = String.Format("BW_ViewF_Feature_basicId_{0}", basicId);
                objFeature = _cache.GetFromCache<ArticlePageDetails>(cacheKey, new TimeSpan(0, 30, 0), () => _features.GetFeatureDetails(basicId));

            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }

            return objFeature;
        }



        /// <summary>
        /// Author : Vivek Gupta on 18-07-2016
        /// Desc: this function moved from content/features/view.aspx.cs for caching and used for photos
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public IEnumerable<ModelImage> BindPhotos(int basicId)
        {
            try
            {
                string cacheKey = String.Format("BW_ViewF_Photos_basicId_{0}", basicId);
                objImg = _cache.GetFromCache<IEnumerable<ModelImage>>(cacheKey, new TimeSpan(0, 30, 0), () => _features.Photos(basicId));

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                ErrorClass objErr = new ErrorClass(ex, "Exception : in BindPhotos");
                objErr.SendMail();
            }

            return objImg;
        }
    }
}
