using Carwale.DTOs.Deals;
using Carwale.Entity;
using Carwale.Entity.Dealers;
using Carwale.Entity.Deals;
using Carwale.Entity.Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Deals
{
    /// <summary>
    /// Created By : Anchal gupta
    /// Date : 26 Feb, 2016
    /// Description : method Declaration Related to GetDeals details 
    /// </summary>
    public interface IDealsRepository
    {
        DiscountSummary GetDealsMaxDiscount(int ModelId, int CityId);
        DealsStock GetAdvantageAdContent(int modelId, int cityId, byte subSegmentId, int versionId, bool isVersionSpecific);
        DealsStock GetStockDetails(int stockId, int cityId);
        Dictionary<int, DiscountSummary> BestVersionDealsByModel(int ModelId, int CityId);
        List<DealsStock> GetRecommendedDeals(string RecommendedModelIds, int RecommendationCount, int CityId, int DealerId);

        List<DealsStock> GetDealsByDiscount(int cityId, int carCount);
        List<MakeEntity> GetDealMakesByCity(int cityId);
        List<ModelEntity> GetDealModelsByMakeAndCity(int cityId, int makeId);
        ProductDetails GetProductDetails(int modelId, int versionId, int cityId);
        List<City> GetAdavantageCities(int modelId, int versionId, int makeId);
        List<DealsStock> GetRecommendationsBySubSegment(int modelId, int cityId);
        List<DealsStock> GetAllVersionsDeals(int modelId, int cityId);
        DealerInquiryDetails GetTransactionDetails(int transactionId);
        List<int> GetCitiesWithMoreModels(int minimumCount);
        AdvantageSearchResults GetDeals(Filters filter);
        bool UpdateCustomerInfo(int inquiryId, DealsInquiryDetail dealsInquiry);
        int GetCarCountByCity(int cityId);
        DealsPriceBreakupEntity GetDealsPriceBreakUp(int stockId, int cityId);
        DealsStock GetOfferOfWeekDetails(int modelId, int cityId);
        List<DealsTestimonialEntity> GetDealsTestimonials(int cityId = 0);
        List<DealsStock> GetDealsSimilarCarsBySubSegment(int modelId, int city, int subsegmentId = 0);
        DealsDealers GetDealerDetails(int dealerId);
        string GetDealsOfferList(string stockIds, int cityId);
        List<MakeModelVersionColor> GetMakeModelVersionColor();
        IEnumerable<VersionEntity> GetAdvantageVersions(int modelId, int cityId);
        IEnumerable<ColorEntity> GetAdvantageVersionColors(int versionId, int cityId);
        IEnumerable<int> GetAdvantageColorYears(int versionId, int colorId, int cityId);
        int GetDealsDealerId(int stockId, int cityId);
    }
}
