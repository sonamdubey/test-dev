using AutoMapper;
using Bikewale.Notifications;
using BikewaleOpr.DTO.Dealers;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ContractCampaign;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.Location;
using BikewaleOpr.Models.DealerFacility;
using BikewaleOpr.Models.DealerPricing;
using System;
using System.Collections.Generic;

namespace BikewaleOpr.Models.DealerFacilities
{
    /// <summary>
    /// Created by :  Snehal Dange on 05-08-2017
    /// Description : Model for Dealer Facility
    /// </summary>
    public class DealerFacilitiesModel
    {
        private readonly IDealers _dealerRepo;
        private readonly ILocation _location = null;

        /// <summary>
        /// Constuctor to initialize the dependencies
        /// </summary>
        /// <param name="dealer"></param>
        public DealerFacilitiesModel(IDealers dealer, ILocation locationObject)
        {
            _dealerRepo = dealer;
            _location = locationObject;
        }

        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
        /// Description :   Maps BikeMakeEntityBase and BikeMakeBase
        /// </summary>
        /// <param name="objMakes"></param>
        /// <returns></returns>
        private IEnumerable<BikeMakeBase> Convert(IEnumerable<BikewaleOpr.Entities.BikeData.BikeMakeEntityBase> objMakes)
        {
            Mapper.CreateMap<BikewaleOpr.Entities.BikeData.BikeMakeEntityBase, BikeMakeBase>();
            return Mapper.Map<IEnumerable<BikewaleOpr.Entities.BikeData.BikeMakeEntityBase>, IEnumerable<BikeMakeBase>>(objMakes);
        }
        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
        /// Description :   Maps DealerEntityBase and DealerBase
        /// </summary>
        /// <param name="objDealers"></param>
        /// <returns></returns>
        private IEnumerable<DealerBase> Convert(IEnumerable<DealerEntityBase> objDealers)
        {
            Mapper.CreateMap<DealerEntityBase, DealerBase>();
            return Mapper.Map<IEnumerable<DealerEntityBase>, IEnumerable<DealerBase>>(objDealers);
        }
        /// <summary>
        /// Created by :  Snehal Dange on 05-08-2017
        ///Description: Method to get all facilities of dealer on page load
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        private IEnumerable<FacilityEntity> GetDealerFacilitiesData(uint dealerId)
        {
            IEnumerable<FacilityEntity> objFacilities = null;
            try
            {
                if (dealerId > 0)
                {
                    objFacilities = _dealerRepo.GetDealerFacilities(dealerId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.Models.DealerFacilities.GetDealerFacilitiesData(), DealerId:{0}", dealerId));
            }
            return objFacilities;
        }


        /// <summary>
        /// Created by : Snehal Dange on 05-08-2017
        ///Description: Method to get model for Dealer Facility Page
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public ManageDealerFacilityVM GetData(uint dealerId, uint cityId, uint makeId, string dealerName)
        {
            ManageDealerFacilityVM viewModel = new ManageDealerFacilityVM();
            viewModel.DealerOperationParams = new DealerOperationPricingVM();

            try
            {
                if (dealerId > 0)
                {
                    viewModel.FacilityList = GetDealerFacilitiesData(dealerId);
                    viewModel.DealerId = dealerId;
                    viewModel.DealerOperationParams.DealerId = dealerId;
                    viewModel.DealerOperationParams.DealerCities = _location.GetDealerCities();
                    viewModel.DealerOperationParams.Makes = _dealerRepo.GetDealerMakesByCity((int)cityId);

                    viewModel.DealerOperationParams.MakesString = Bikewale.Utility.EncodingDecodingHelper.EncodeTo64(
                        Newtonsoft.Json.JsonConvert.SerializeObject(
                            Convert(
                                viewModel.DealerOperationParams.Makes)));
                    viewModel.DealerOperationParams.Dealers = _dealerRepo.GetDealersByMake(makeId, cityId);

                    viewModel.DealerOperationParams.DealersString = Bikewale.Utility.EncodingDecodingHelper.EncodeTo64(
                        Newtonsoft.Json.JsonConvert.SerializeObject(
                            Convert(
                                viewModel.DealerOperationParams.Dealers)));
                    viewModel.DealerOperationParams.CityId = cityId;
                    viewModel.DealerOperationParams.MakeId = makeId;
                    viewModel.PageTitle = "Manage Dealer Facilites of " + dealerName;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.Models.DealerFacilities.GetData() DealerId:{0}", dealerId));
            }
            return viewModel;
        }
    }
}