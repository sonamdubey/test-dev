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
    public class DealerFacilitiesModel
    {
        private readonly IDealers _dealerRepo;
        public DealerFacilitiesModel(IDealers dealer)
        {
            _dealerRepo = dealer;
        }


        public List<FacilityEntity>  GetDealerFacilitiesData(uint dealerId)
        {
            List<FacilityEntity> objFacilities = null;
            try
            {
               
                objFacilities = _dealerRepo.GetDealerFacilities(dealerId);


            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.Models.DealerFacilities.GetDealerFacilitiesData");
            }
            return objFacilities;

        }

        public ManageDealerFacilityVM GetData(uint dealerId)
        {
            ManageDealerFacilityVM viewModel = new ManageDealerFacilityVM();
            try
            {
                if(dealerId>0)
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