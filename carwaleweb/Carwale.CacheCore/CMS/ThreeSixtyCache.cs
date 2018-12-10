using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity;
using Carwale.Entity.CMS;
using Carwale.Entity.Enum;
using Carwale.Interfaces;
using Carwale.Interfaces.CMS;
using System.Collections.Generic;

namespace Carwale.Cache.CMS
{
    public class ThreeSixtyCache : IThreeSixtyCache {

        private readonly IThreeSixtyDal _dalThreeSixty;
        private readonly ICacheManager _cachecore;
        private readonly string _hotspotDetailsCacheKey = "hotspots_details_{0}_{1}";
        private readonly string _hotspotCacheKey = "hotspots_{0}_{1}";

        public ThreeSixtyCache(ICacheManager cachecore,IThreeSixtyDal dalThreeSixty)
        {
            _cachecore = cachecore;
            _dalThreeSixty = dalThreeSixty;
        }

        public HotspotData GetHotspots(int modelId, ThreeSixtyViewCategory type)
        {
            return _cachecore.GetFromCache(string.Format(_hotspotCacheKey, modelId, (int)type),
                        CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"),
                            () => _dalThreeSixty.GetHotspots(modelId,type));
        }

        public Dictionary<string, List<Hotspot>> GetHotspotDetails(int modelId, ThreeSixtyViewCategory category)
        {
            return _cachecore.GetFromCache(string.Format(_hotspotDetailsCacheKey, modelId, (int)category),
                        CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"),
                            () => _dalThreeSixty.GetHotspotDetails(modelId, category));
        }

        public Dictionary<string, Dictionary<string, List<Hotspot>>> MultiGetHotspots(int modelId)
        {
            var hotspotsCallback = new Dictionary<string, MultiGetCallback<Dictionary<string, List<Hotspot>>>>();
            string exteriorHotspotsCacheKey = string.Format(_hotspotDetailsCacheKey, modelId, (int)ThreeSixtyViewCategory.Closed);
            string openHotspotsCacheKey = string.Format(_hotspotDetailsCacheKey, modelId, (int)ThreeSixtyViewCategory.Open);
            string interiorHotspotsCacheKey = string.Format(_hotspotDetailsCacheKey, modelId, (int)ThreeSixtyViewCategory.Interior);

            hotspotsCallback.Add(exteriorHotspotsCacheKey,
                        new MultiGetCallback<Dictionary<string, List<Hotspot>>>()
                        { CacheDuration = CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"), DbCallback = () => _dalThreeSixty.GetHotspotDetails(modelId, ThreeSixtyViewCategory.Closed) });
            hotspotsCallback.Add(openHotspotsCacheKey,
                        new MultiGetCallback<Dictionary<string, List<Hotspot>>>()
                        { CacheDuration = CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"), DbCallback = () => _dalThreeSixty.GetHotspotDetails(modelId, ThreeSixtyViewCategory.Open) });
            hotspotsCallback.Add(interiorHotspotsCacheKey,
                        new MultiGetCallback<Dictionary<string, List<Hotspot>>>()
                        { CacheDuration = CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"), DbCallback = () => _dalThreeSixty.GetHotspotDetails(modelId, ThreeSixtyViewCategory.Interior) });

            return _cachecore.MultiGetFromCache(hotspotsCallback);
        }

        public Dictionary<string, HotspotData> MultiGetXmlVersions(int modelId)
        {
            var hotspotsCallback = new Dictionary<string, MultiGetCallback<HotspotData>>();
            string exteriorXmlCacheKey = string.Format(_hotspotCacheKey, modelId, (int)ThreeSixtyViewCategory.Closed);
            string openXmlCacheKey = string.Format(_hotspotCacheKey, modelId, (int)ThreeSixtyViewCategory.Open);
            string interiorXmlCacheKey = string.Format(_hotspotCacheKey, modelId, (int)ThreeSixtyViewCategory.Interior);

            hotspotsCallback.Add(exteriorXmlCacheKey,
                        new MultiGetCallback<HotspotData>() 
                        { CacheDuration = CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"), DbCallback = () => _dalThreeSixty.GetHotspots(modelId, ThreeSixtyViewCategory.Closed) });
            hotspotsCallback.Add(openXmlCacheKey,
                        new MultiGetCallback<HotspotData>()
                        { CacheDuration = CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"), DbCallback = () => _dalThreeSixty.GetHotspots(modelId, ThreeSixtyViewCategory.Open) });
            hotspotsCallback.Add(interiorXmlCacheKey,
                        new MultiGetCallback<HotspotData>() 
                        { CacheDuration = CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"), DbCallback = () => _dalThreeSixty.GetHotspots(modelId, ThreeSixtyViewCategory.Interior) });

            return _cachecore.MultiGetFromCache(hotspotsCallback);
        }
    }
}
