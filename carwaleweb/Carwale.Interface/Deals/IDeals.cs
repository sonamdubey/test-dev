using Carwale.DTOs.CarData;
using Carwale.DTOs.Deals;
using Carwale.Entity.Deals;
using System.Collections.Generic;
using Carwale.Entity.Dealers;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.DTOs;
using Carwale.Entity;
using Carwale.Interfaces.Deals.Cache;

namespace Carwale.Interfaces.Deals
{
    public interface IDeals
    {
        IDealsCache dealsCache { get; }
        bool IsShowDeals(int cityId, bool noCitySelected = false);
        DiscountSummary GetDealsSummaryByModelandCity(int modelId, int cityId);
        Carwale.DTOs.Deals.DealsStockDTO GetAdvantageAdContent(int modelId, int cityId, byte subSegmentId, int versionId = 0, bool isVersionSpecific = false);
        BookingAndroid_DTO GetStockDetails(int stockId, int cityId);
        bool AutobizPushPaidLead(bool IsPaymentSuccess, string sourceIdForAutobiz, DealsStock dealsStock);
        List<DealsStock> GetRecommendedDeals(string recommendedModelIds, int recommendationCount, int cityId, string userHistory, int dealerId);
        List<DealsRecommendationDTO> GetDealsRecommendation(string recommendedModelIds, int recommendationCount, int cityId, string userHistory, int dealerId, string currentModel);
        DiscountSummaryDTO_Android GetDealsSummaryByModelandCity_Android(int modelId, int cityId);
        List<DealsSummaryDesktop_DTO> GetDealsByDiscount(int cityId, int carCount);
        List<CarModelsDTO> GetDealModelsByMakeAndCity(int cityId, int MakeId);
        List<DealsSummaryMobile_DTO> GetDealsByDiscountMobile(int cityId, int carCount);
        ProductDetailsDTO_Android GetProductDetails(int modelId, int versionId, int cityId);
        List<CarMakesDTO> GetDealMakesByCity(int cityId);
        List<City> GetAdvantageCities(int modelId = 0, int versionId = 0, int makeId = 0);
        Carwale.DTOs.CarData.DiscountSummaryDTO CarOverviewDiscountSummary(DiscountSummary discountSummary);
        List<DealsRecommendationDTO> GetRecommendationsBySubSegment(int modelId, int cityId = 1);
        bool AutoBizDealsLeadProcessApp(DealerInquiryDetails dealerInquiryDetails, bool IsPaymentSuccess);
        List<Carwale.DTOs.Deals.DealsStockDTO> GetAllVersionDeals(int modelId, int cityId);
        List<int> GetCitiesWithMoreModels(int minimumStockCount);
        AdvantageSearchResultsDTO GetDeals(Filters filter);
        int GetCarCountByCity(int cityId);
        List<DiscountSummaryDTO_Android> BestVersionDealsByModel(int modelId, int cityId);
        string RemoveAdvHistoryModels(string advHistory, string pqHistory);
        List<DealsRecommendationDTO> GetPQUserHistoryDeals(List<DealsRecommendationDTO> advHistory,int recommendationCount, string pqHistory, int cityId, int currentModel);
        List<DealsRecommendationDTO> GetBestSavingsCar(int recommendationCount, int modelId, int cityId);
        string RemoveCurrentModel(string userHistory, string currentModel);
        void PushLeadToAutobiz(DealsInquiryDetailDTO dealerInquiryDetails, bool isPaymentSuccess, int inquirySourceId);
        Carwale.DTOs.Deals.DealsStockDTO GetOfferOfWeekDetails(int modelId, int cityId);
        List<DealsRecommendationDTO> GetAdvUserHistoryDeals(string advHistory, int recommendationCount, int cityId, int currentModel);
        SimilarDealsRecommendationDTO GetSimilarDeals(int modelId, int cityId);
        List<DealsTestimonialDTO> GetDealsTestimonials(int cityId = 0);
        List<DealsRecommendationDTO> GetDealsSimilarCarsBySubSegment(int modelId, int cityId, int subsegment = 0);
        DealsPriceBreakupDTO GetDealsPriceBreakup(int stockId, int cityId);
        string GetMakeModelVersionColor();
        IEnumerable<CarVersionsDTO> GetAdvantageVersions(int modelId, int cityId);
        IEnumerable<CarColorDTO> GetAdvantageVersionColors(int versionId, int cityId);
        IEnumerable<int> GetAdvantageColorYears(int versionId, int colorId, int cityId);
        List<Tuple<int, int>> GetDealsByVersionList(List<Tuple<int, int>> modelVersionList, int cityId);
        List<KeyValuePair<string, string>> FillOfferList(List<DealsOfferEntity> offerList, int stockId=0);
        List<DealsOfferEntity> GetOffers(string stockIds, int cityId);
        List<Carwale.DTOs.Deals.DealsStockDTO> GetFilteredVersionDeals(int modelId, int versionId, Filters filter);
    }
}
