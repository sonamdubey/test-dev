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
using Bikewale.Entities.DealerLocator;

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
        public IEnumerable<NewBikeDealerEntityBase> GetNewBikeDealersList(int makeId, int cityId, EnumNewBikeDealerClient? clientId = null)
        {
            IEnumerable<NewBikeDealerEntityBase> objDealersList = null; 
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

        /// <summary>
        ///  To capture manufacturer lead against bikewale pricequote
        /// </summary>
        /// <param name="lead"></param>
        /// <returns></returns>
        public bool SaveManufacturerLead(ManufacturerLeadEntity lead)
        {
            bool status = false;
            status = dealerRepository.SaveManufacturerLead(lead);
            return status;
        }

        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On : 3rd Febrauary 2016
        /// Summary : Method to get list of all cities in which dealers having booking is available
        /// </summary>
        /// <returns></returns>
        public List<CityEntityBase> GetDealersBookingCitiesList()
        {
            string _apiUrl = "/api/DealerPriceQuote/getBikeBookingCities/";
            List<CityEntityBase> lstCity = null;
            try
            {
                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    lstCity = objClient.GetApiResponseSync<List<CityEntityBase>>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, lstCity);
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, System.Web.HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return lstCity;
        }
        
        /// <summary>
        /// Created By : Lucky Rathore on 21 March 2016
        /// Descritption : Implemented in DAL
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public Dealers GetDealerByMakeCity(uint cityId, uint makeId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 22 Mar 2016
        /// Description :   Calls the DAL
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<CityEntityBase> FetchDealerCitiesByMake(uint makeId)
        {
            try
            {
                return dealerRepository.FetchDealerCitiesByMake(makeId);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "FetchDealerCitiesByMake");
                objErr.SendMail();
                return null;
            }
        }
    }
}
