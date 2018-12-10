using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.DAL.Classified.CarValuation;
using Carwale.Entity.Classified.CarValuation;
using Carwale.Interfaces;
using Carwale.Interfaces.Classified.CarValuation;
using System.Configuration;

namespace Carwale.Cache.Classified
{
    public class ValuationCacheRepository : IValuationCacheRepository
    {
        private readonly ICacheManager _cacheProvider;
        private static readonly CTValuationClient _valuationClient = new CTValuationClient(ConfigurationManager.AppSettings["CTValuationApiUrl"]);
        private const string _keyPrefix = "valuation";

        public ValuationCacheRepository(ICacheManager cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public Valuation GetValuation(ValuationUrlParameters valuationUrlParameters)
        {
            return _cacheProvider.GetFromCache(string.Format("{0}_{1}_{2}_{3}_{4}{5}_{6}",
                _keyPrefix,
                valuationUrlParameters.Year,
                valuationUrlParameters.VersionId,
                valuationUrlParameters.CityId,
                valuationUrlParameters.Owners,
                valuationUrlParameters.Kilometers > 0 ? ("_" + valuationUrlParameters.Kilometers.ToString()) : "",
                valuationUrlParameters.IsSellingIndex?"1":"0"
                ),
                CacheRefreshTime.OneDayExpire(), () => _valuationClient.GetValuation(valuationUrlParameters));
        }
    }
}
