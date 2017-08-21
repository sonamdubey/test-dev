using AutoMapper;
using BikewaleOpr.DTO.Dealers;
using BikewaleOpr.Entity;
using BikewaleOpr.Entity.ContractCampaign;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.Location;
using BikewaleOpr.Models.DealerPricing;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;

namespace BikewaleOpr.Models.DealerBookingAmount
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 18 Aug 2017
    /// Description :   Model for dealer bookimg amount page
    /// </summary>
    public class DealerBookingAmountModel
    {
        private readonly ILocation _location = null;
        private readonly IDealers _dealersRepository = null;
        private readonly IManageBookingAmountPage _manageBookingAmountPageData = null;

        public DealerBookingAmountModel(ILocation locationObject, IDealers dealerRepositoryObject, IManageBookingAmountPage manageBookingAmountPageDataObject)
        {
            _location = locationObject;
            _dealersRepository = dealerRepositoryObject;
            _manageBookingAmountPageData = manageBookingAmountPageDataObject;
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
        /// Created By  :   Vishnu Teja Yalakuntla on 18 Aug 2017
        /// Description :   Populates the dealer booking amount pages view model
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <param name="makeId"></param>
        /// <param name="dealerName"></param>
        /// <returns></returns>
        public DealerBookingAmountVM GetDealerBookingAmountData(uint dealerId, uint cityId, uint makeId, string dealerName)
        {
            DealerBookingAmountVM dealerBookingAmountInfo = new DealerBookingAmountVM();
            dealerBookingAmountInfo.DealerOperationParams = new DealerOperationPricingVM();
            dealerBookingAmountInfo.DealerBookingAmountData = new ManageBookingAmountData();

            try
            {
                dealerBookingAmountInfo.DealerBookingAmountData = _manageBookingAmountPageData.GetManageBookingAmountData(dealerId);
                dealerBookingAmountInfo.DealerBookingAmountData.UpdateMessage = "";
                dealerBookingAmountInfo.DealerName = dealerName;
                dealerBookingAmountInfo.CityId = cityId;
                dealerBookingAmountInfo.MakeId = makeId;
                dealerBookingAmountInfo.DealerId = dealerId;


                dealerBookingAmountInfo.DealerOperationParams.DealerId = dealerId;
                dealerBookingAmountInfo.DealerOperationParams.DealerCities = _location.GetDealerCities();
                dealerBookingAmountInfo.DealerOperationParams.Makes = _dealersRepository.GetDealerMakesByCity((int)cityId);

                dealerBookingAmountInfo.DealerOperationParams.MakesString = Bikewale.Utility.EncodingDecodingHelper.EncodeTo64(
                    Newtonsoft.Json.JsonConvert.SerializeObject(
                        Convert(
                            dealerBookingAmountInfo.DealerOperationParams.Makes)));
                dealerBookingAmountInfo.DealerOperationParams.Dealers = _dealersRepository.GetDealersByMake(makeId, cityId);

                dealerBookingAmountInfo.DealerOperationParams.DealersString = Bikewale.Utility.EncodingDecodingHelper.EncodeTo64(
                    Newtonsoft.Json.JsonConvert.SerializeObject(
                        Convert(
                            dealerBookingAmountInfo.DealerOperationParams.Dealers)));
                dealerBookingAmountInfo.DealerOperationParams.CityId = cityId;
                dealerBookingAmountInfo.DealerOperationParams.MakeId = makeId;

                dealerBookingAmountInfo.PageTitle = "Manage Booking Amount of " + dealerName;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex,
                    string.Format("GetDealerBookingAmountData dealerId={0} cityId={1} makeId={2}", dealerId, cityId, makeId));
            }

            return dealerBookingAmountInfo;
        }
    }
}