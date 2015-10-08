using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.Dealer;
using Bikewale.Interfaces.Dealer;
using Bikewale.Entities.Location;
using Microsoft.Practices.Unity;
using Bikewale.Entities.BikeData;

namespace Bikewale.BAL.Dealer
{
    /// <summary>
    /// Created By : Ashwini Todkar on 4 June 2014
    /// </summary>
    public class Dealer : IDealer
    {
        private readonly IDealer dealerRepository = null;

        public Dealer()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IDealer, DealersRepository>();
                dealerRepository = container.Resolve<IDealer>();
            }
        }

        /// <summary>
        /// Get list of makes along with total dealers count for each make
        /// </summary>
        /// <returns></returns>
        public List<NewBikeDealersMakeEntity> GetDealersMakesList()
        {
            List<NewBikeDealersMakeEntity> objMakeList = null;

            objMakeList = dealerRepository.GetDealersMakesList();

            return objMakeList;          
        }

        /// <summary>
        /// Function to get the cities list with dealers count in the city along with states.
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public NewBikeDealersListEntity GetDealersCitiesListByMakeId(uint makeId)
        {
            NewBikeDealersListEntity objDealerList = null;
            
            objDealerList = dealerRepository.GetDealersCitiesListByMakeId(makeId);

            return objDealerList;
        }

        /// <summary>
        /// Get all dealers details list of a given make in the given city.
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public List<NewBikeDealerEntity> GetDealersList(uint makeId, uint cityId)
        {
            List<NewBikeDealerEntity> objDealersList = null;

            objDealersList = dealerRepository.GetDealersList(makeId, cityId);

            return objDealersList;          
        }

        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On : 7th October 2015
        /// Get list of all dealers with details for a given make and city.
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public NewBikeDealerEntityList GetNewBikeDealersList(int makeId, int cityId, EnumNewBikeDealerClient? clientId = null)
        {
            NewBikeDealerEntityList objDealersList = null; 
            objDealersList = dealerRepository.GetNewBikeDealersList(makeId, cityId,clientId);
            return objDealersList;
        }

        /// <summary>
        /// Function to get the list of makes available in the given city.
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public List<BikeMakeEntityBase> GetDealersMakeListByCityId(uint cityId)
        {
            List<BikeMakeEntityBase> objMakeList = null;

            objMakeList = dealerRepository.GetDealersMakeListByCityId(cityId);

            return objMakeList;
        }

        /// <summary>
        /// Function to get the list of cities where dealers are available
        /// </summary>
        /// <returns></returns>
        public List<CityEntityBase> GetDealersCitiesList()
        {
            List<CityEntityBase> objCitiesList = null;

            objCitiesList = dealerRepository.GetDealersCitiesList();

            return objCitiesList;          
        }
    }
}
