using Carwale.Entity;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.Dealers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Dealers
{
    public interface INewCarDealersCache
    {
        List<CarMakeEntityBase> GetMakesByCity(int cityId);
        List<DealerStateEntity> GetCitiesByMake(int makeId);
        List<PopularCitiesEntity> GetPopularCitiesByMake(int makeId);
        List<NewCarDealerCountByMake> NewCarDealerCountMake(string type);
        List<MakeModelEntity> GetDealerModels(int dealerId);
        IEnumerable<NewCarDealersList> GetNCSDealers(int modelId, int cityId);
        void StoreDealersList(string cacheKey, NewCarDealerEntiy newCarDealers);
        void StoreDealerDetails(string cacheKey, DealerDetails dealerDetails);
        List<ClientCampaignMapping> GetClientCampaignMapping();
        IEnumerable<NewCarDealer> GetDealerListByCityMake(int makeId, int cityId);
    }
}
