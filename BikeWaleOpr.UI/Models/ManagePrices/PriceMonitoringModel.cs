using Bikewale.Notifications;
using BikewaleOpr.DALs.Location;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikePricing;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.BikePricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Models.ManagePrices
{
    /// <summary>
    /// Created By: Ashutosh Sharma on 31-07-2017
    /// Discription : Model for Price monitoring report page.
    /// </summary>
    public class PriceMonitoringModel
    {
        private readonly IBikeMakes _makesRepo = null;
        private readonly IShowroomPricesRepository _pricesRepo = null;

        public PriceMonitoringModel(IBikeMakes makesRepo, IShowroomPricesRepository pricesRepo)
        {
            _makesRepo = makesRepo;
            _pricesRepo = pricesRepo;
        }

        /// <summary>
        /// Created By: Ashutosh Sharma on 31-07-2017
        /// Discription: Method to return list of bike makes.
        /// </summary>
        /// <param name="requestType"></param>
        /// <returns>List of bike makes.</returns>
        public IEnumerable<BikeMakeEntityBase> GetMakes(string requestType)
        {
            IEnumerable<BikeMakeEntityBase> makesList = null;
            try
            {
                if (!string.IsNullOrEmpty(requestType))
                    makesList =  _makesRepo.GetMakes(requestType);
                    
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.Models.ManagePrices.PriceMonitoringModel.GetMakes_requestType:{0}",requestType));
            }
            return makesList;
        }

        /// <summary>
        /// Created By: Ashutosh Sharma on 09-08-2017
        /// Description
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Entities.StateEntityBase> GetStates()
        {
            IEnumerable<Entities.StateEntityBase> stateList = null;
            try
            {
                LocationRepository _locationRepo = new LocationRepository();
                stateList = _locationRepo.GetStates();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.Models.ManagePrices.PriceMonitoringModel.GetStates");
            }
            return stateList;
        }
        /// <summary>
        /// Created By: Ashutosh Sharma on 31-07-2017
        /// Description : Method to get price last updated details of bike versions in cities.
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public PriceMonitoringEntity GetPriceMonitoringDetails(uint makeId, uint modelId, uint stateId)
        {
            PriceMonitoringEntity priceMonitoring = null;
            try
            {
                priceMonitoring = _pricesRepo.GetPriceMonitoringDetails(makeId, modelId, stateId);
              
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.Models.ManagePrices.PriceMonitoringModel.GetPriceMonitoringDetails_makeId:{0}_modelId:{1}",makeId, modelId));
            }

            return priceMonitoring;
        }
    }
}