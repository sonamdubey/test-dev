using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.Dealers;
using Carwale.Entity.CarData;
using Carwale.DTOs.Dealer;

namespace Carwale.Interfaces.Dealers
{
    public interface INewCarDealers
    {
        NewCarDealerEntiy GetDealersList(int stateId, int cityId, int makeId, bool showDealerImage = true);
        List<CarMakeEntityBase> GetMakesByCity(int cityId);
        Tuple<List<DealerStateEntity>, List<PopularCitiesEntity>> GetCitiesByMake(int makeId);
        List<NewCarDealerCountByMake> GetMakesAndCount(string type);
        DealerShowroomDetails GetDealerDetails(int dealerId, int? pqCampaignId, int? pqCityId, int pqMakeId = 0);
        string CallSlugNumberByMakeId(int makeId);
        string CallSlugNumberByModelId(int modelId);
        List<DealerStateEntity> GetStatesAndCitiesByMake(int makeId);
        List<DealerModelListDTO> DealerModelListBl(int dealerId);
        NewCarDealerEntiy GetNCDealersList(int makeId, int modelId, int cityId);
        DealerDetails GetPremiumDealerDetails(int dealerId);
        DealerDetails NCDDetails(int dealerId, int campaignId, int makeId, int cityId);
        DealerDetails GetCampaignDealerDetails(int campaignType, int modelId, int dealerId, int campaignId, int cityId);
        List<DealerLocatorDTO> GetDealersByMakeModel(int makeId, int modelId, int cityId);
        SponsoredDealer GetCampaignDealerDetailsById(DealerInquiryDetails t);
        IEnumerable<NewCarDealersList> GetNcsDealers(int modelId, int cityId, int campaignId);
        NewCarDealerEntiy NewCarDealerListByCityMake(int makeId, int modelId, int cityId, bool withCampaign);
    }
}
