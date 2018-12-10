using AEPLCore.Cache;
using Carwale.DTOs.Deals;
using Carwale.Entity;
using Carwale.Entity.Deals;
using Carwale.Entity.Geolocation;
using System.Collections.Generic;

namespace Carwale.Interfaces.Deals.Cache
{
    public interface IDealsCache
    {
        DiscountSummary GetDealsMaxDiscount(int modelId, int cityId);
        Dictionary<int, DiscountSummary> BestVersionDealsByModel(int ModelId, int CityId);
        DealsStock GetAdvantageAdContent(int modelId, int cityId, byte subSegmentId, int versionId = 0, bool isVersionSpecific = false);
        List<MakeEntity> GetDealMakesByCity(int cityId);
        List<DealsStock> GetDealsByDiscount(int cityId, int carCount);
        List<ModelEntity> GetDealModelsByMakeAndCity(int cityId, int makeId);
        ProductDetails GetProductDetails(int modelId, int versionId, int cityId);
        List<City> GetAdvantageCities(int modelId, int versionId, int makeId);
        List<DealsStock> GetRecommendationsBySubSegment(int modelId, int cityId);
        List<DealsStock> GetAllVersionDeals(int modelId, int cityId);
        List<int> GetCitiesWithMoreModels(int minimumStockCount);
        int GetCarCountByCity (int cityId);
        DealsPriceBreakupEntity GetDealsPriceBreakUp(int stockId, int cityId);
        DealsStock GetOfferOfWeekDetails(int modelId, int cityId);
        List<DealsTestimonialEntity> GetDealsTestimonials(int cityId = 0);
        string GetDealsOfferList(string stockIds, int cityId);
        DealsDealers GetDealerDetails(int dealerId);
        List<DealsStock> GetDealsSimilarCarsBySubSegment(int modelId, int cityId, int subsegmentId);
        IEnumerable<VersionEntity> GetAdvantageVersions(int modelId, int cityId);
        IEnumerable<ColorEntity> GetAdvantageVersionColors(int versionId, int cityId);
        IEnumerable<int> GetAdvantageColorYears(int versionId, int colorId, int cityId);
        Dictionary<string, DealsStock> GetDealsByVersionList(Dictionary<string, MultiGetCallback<DealsStock>> advantageAdCallback);
        Dictionary<int, DealsStock> GetAdvantageAdContentV1(List<int> modelIdList, int cityId);
        int GetDealsDealerId(int stockId, int cityId) ;

    }
}
