using AutoMapper;
using BikewaleOpr.DTO.Dealers;
using BikewaleOpr.Entity.ContractCampaign;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.Location;
using BikewaleOpr.Models.DealerPricing;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;

namespace BikewaleOpr.Models.DealerEMI
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 18 Aug 2017
    /// Description :   Model for dealer EMI management page
    /// </summary>
    public class DealerEMIModel
    {
        private readonly ILocation _location = null;
        private readonly IDealers _dealersRepository = null;

        public DealerEMIModel(ILocation locationObject, IDealers dealersRepositoryObject)
        {
            _location = locationObject;
            _dealersRepository = dealersRepositoryObject;
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
        /// Description :   Fetches all the information for dealer collapsable and EMI grid.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public DealerEMIVM GetDealerEmiInfo(uint dealerId, uint cityId, uint makeId, string dealerName)
        {
            DealerEMIVM dealerEmiInfo = new DealerEMIVM();
            dealerEmiInfo.DealerOperationParams = new DealerOperationPricingVM();

            try
            {
                dealerEmiInfo.DealerName = dealerName;
                dealerEmiInfo.dealerEmiFormInfo = _dealersRepository.GetDealerLoanAmounts(dealerId);

                dealerEmiInfo.DealerOperationParams.DealerId = dealerId;
                dealerEmiInfo.DealerOperationParams.DealerCities = _location.GetDealerCities();
                dealerEmiInfo.DealerOperationParams.Makes = _dealersRepository.GetDealerMakesByCity((int)cityId);

                dealerEmiInfo.DealerOperationParams.MakesString = Bikewale.Utility.EncodingDecodingHelper.EncodeTo64(
                    Newtonsoft.Json.JsonConvert.SerializeObject(
                        Convert(
                            dealerEmiInfo.DealerOperationParams.Makes)));
                dealerEmiInfo.DealerOperationParams.Dealers = _dealersRepository.GetDealersByMake(makeId, cityId);

                dealerEmiInfo.DealerOperationParams.DealersString = Bikewale.Utility.EncodingDecodingHelper.EncodeTo64(
                    Newtonsoft.Json.JsonConvert.SerializeObject(
                        Convert(
                            dealerEmiInfo.DealerOperationParams.Dealers)));
                dealerEmiInfo.DealerOperationParams.CityId = cityId;
                dealerEmiInfo.DealerOperationParams.MakeId = makeId;

                dealerEmiInfo.PageTitle = "Configure EMI properties for " + dealerName;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex,
                    string.Format("GetDealerEmiInfo dealerId={0} cityId={1} makeId={2}", dealerId, cityId, makeId));
            }

            return dealerEmiInfo;
        }
    }
}
