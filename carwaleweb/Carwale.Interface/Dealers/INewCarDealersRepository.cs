using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.Dealers;
using Carwale.Entity.CarData;
using Carwale.Entity;
using Carwale.Entity.Campaigns;

namespace Carwale.Interfaces.Dealers
{
    public interface INewCarDealersRepository
    {
        List<CarMakeEntityBase> GetMakesByCity(int cityId);
        List<DealerStateEntity> GetCitiesByMake(int makeId);
        List<PopularCitiesEntity> GetPopularCitiesByMake(int makeId);
        List<NewCarDealerCountByMake> GetCarCountByMakesAndType(string type);
        List<MakeModelEntity> GetDealerModels(int dealerId);
        IEnumerable<NewCarDealersList> GetNCSDealers(int modelId, int cityId);
        DealerMicrositeImage GetDealerMicrositeImages(int dealerId);
        List<ClientCampaignMapping> GetClientCampaignMapping();
        IEnumerable<NewCarDealer> GetDealerListByCityMake(int makeId, int cityId);
    }
}
