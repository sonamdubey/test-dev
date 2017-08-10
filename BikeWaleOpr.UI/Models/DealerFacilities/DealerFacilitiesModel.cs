using Bikewale.Notifications;
using BikewaleOpr.Entities;
using BikewaleOpr.Interface;
using BikewaleOpr.Models.DealerFacility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Models.DealerFacilities
{
    /// <summary>
    /// Created by :  Snehal Dange on 05-08-2017
    /// Description : Model for Dealer Facility
    /// </summary>
    public class DealerFacilitiesModel
    {
        private readonly IDealers _dealerRepo;

        /// <summary>
        /// Constuctor to initialize the dependencies
        /// </summary>
        /// <param name="dealer"></param>
        public DealerFacilitiesModel(IDealers dealer)
        {
            _dealerRepo = dealer;
        }


        /// <summary>
        /// Created by :  Snehal Dange on 05-08-2017
        ///Description: Method to get all facilities of dealer on page load
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public IEnumerable<FacilityEntity>  GetDealerFacilitiesData(uint dealerId)
        {
            IEnumerable<FacilityEntity> objFacilities = null;
            try
            {
                if(_dealerRepo !=null)
                {
                    objFacilities = _dealerRepo.GetDealerFacilities(dealerId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.Models.DealerFacilities.GetDealerFacilitiesData");
            }
            return objFacilities;
        }


        /// <summary>
        /// Created by : Snehal Dange on 05-08-2017
        ///Description: Method to get model for Dealer Facility Page
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public ManageDealerFacilityVM GetData(uint dealerId)
        {
            ManageDealerFacilityVM viewModel = new ManageDealerFacilityVM();
            try
            {
                if(dealerId>0 && _dealerRepo != null)
                {
                    viewModel.FacilityList = GetDealerFacilitiesData(dealerId);
                    viewModel.DealerId = dealerId;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.Models.DealerFacilities.GetData");
            }
            return viewModel;
        }
    }
}