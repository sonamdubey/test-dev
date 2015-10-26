using Bikewale.Entities.Dealer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.Customer;

namespace Bikewale.Interfaces.Dealer
{
    /// <summary>
    /// Created By : Ashwini Todkar on 4 June 2014
    /// </summary>
    
    public interface IDealer
    {
        List<NewBikeDealersMakeEntity> GetDealersMakesList();
        NewBikeDealersListEntity GetDealersCitiesListByMakeId(uint makeId);
        List<NewBikeDealerEntity> GetDealersList(uint makeId, uint cityId);
        List<BikeMakeEntityBase> GetDealersMakeListByCityId(uint cityId);
        List<CityEntityBase> GetDealersCitiesList();
        //Added by sushil kumar on 7th Oct 2015
        IEnumerable<NewBikeDealerEntityBase> GetNewBikeDealersList(int makeId, int cityId, EnumNewBikeDealerClient? clientId = null);
        bool SaveManufacturerLead(ManufacturerLeadEntity customer);
    }
}
