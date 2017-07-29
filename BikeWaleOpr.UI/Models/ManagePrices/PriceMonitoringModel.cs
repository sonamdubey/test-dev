using Bikewale.Notifications;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Interface.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Models.ManagePrices
{
    /// <summary>
    /// Created By:
    /// </summary>
    public class PriceMonitoringModel
    {
        private readonly IBikeMakes _makesRepo = null;
        public PriceMonitoringModel(IBikeMakes makesRepo)
        {
            _makesRepo = makesRepo;
        }
        public IEnumerable<BikeMakeEntityBase> GetMakes(string requestType)
        {
            IEnumerable<BikeMakeEntityBase> makesList = null;
            try
            {
                if (requestType != string.Empty)
                {
                    makesList =  _makesRepo.GetMakes(requestType);
                    
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.Models.ManagePrices.PriceMonitoringModel.GetMakes_" + requestType);
            }
            return makesList; ;
        }
    }
}