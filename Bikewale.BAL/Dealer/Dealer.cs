using Bikewale.BAL.ApiGateway.Adapters.BikeData;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.ApiGateway.Entities.BikeData;
using Bikewale.DTO.MobileVerification;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Entities.MobileVerification;
using Bikewale.Interfaces.Dealer;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.BAL.Dealer
{
    /// <summary>
    /// Created By : Ashwini Todkar on 4 June 2014
    /// </summary>
    public class Dealer : IDealer
    {
        private readonly IDealerRepository _dealerRepository;
        private readonly IDealerCacheRepository _dealerCacheRepository;
        private readonly IApiGatewayCaller _apiGatewayCaller;

        public Dealer(IDealerRepository dealerRepository, IDealerCacheRepository dealerCacheRepository, IApiGatewayCaller apiGatewayCaller)
        {
            _dealerRepository = dealerRepository;
            _dealerCacheRepository = dealerCacheRepository;
            _apiGatewayCaller = apiGatewayCaller;
        }

        /// <summary>
        /// Get list of makes along with total dealers count for each make
        /// </summary>
        /// <returns></returns>
        public List<NewBikeDealersMakeEntity> GetDealersMakesList()
        {
            List<NewBikeDealersMakeEntity> objMakeList = null;

            objMakeList = _dealerRepository.GetDealersMakesList();

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

            objDealerList = _dealerRepository.GetDealersCitiesListByMakeId(makeId);

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

            objDealersList = _dealerRepository.GetDealersList(makeId, cityId);

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
            objDealersList = _dealerRepository.GetNewBikeDealersList(makeId, cityId, clientId);
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

            objMakeList = _dealerRepository.GetDealersMakeListByCityId(cityId);

            return objMakeList;
        }

        /// <summary>
        /// Function to get the list of cities where dealers are available
        /// </summary>
        /// <returns></returns>
        public List<CityEntityBase> GetDealersCitiesList()
        {
            List<CityEntityBase> objCitiesList = null;

            objCitiesList = _dealerRepository.GetDealersCitiesList();

            return objCitiesList;
        }

        /// <summary>
        ///  To capture manufacturer lead against bikewale pricequote
        /// </summary>
        /// <param name="lead"></param>
        /// <returns></returns>
        [Obsolete("Not being used", true)]
        public bool SaveManufacturerLead(ManufacturerLeadEntity lead)
        {
            bool status = false;
            status = _dealerRepository.SaveManufacturerLead(lead);
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
            List<CityEntityBase> lstCity = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealerPriceQuote, Bikewale.DAL.AutoBiz.DealerPriceQuoteRepository>();
                    Bikewale.Interfaces.AutoBiz.IDealerPriceQuote objPriceQuote = container.Resolve<Bikewale.DAL.AutoBiz.DealerPriceQuoteRepository>();
                    lstCity = objPriceQuote.GetBikeBookingCities(null);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, System.Web.HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return lstCity;
        }

        /// <summary>
        /// Created By : Lucky Rathore on 21 March 2016
        /// Descritption : Implemented in DAL
        /// Modified by :   Sumit Kate on 20 May 2016
        /// Description :   Called the DAL function rather than throwing NotImplementedException
        /// Modified by :   Sumit Kate on 19 Jun 2016
        /// Description :   Added Optional parameter(inherited from Interface)
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public DealersEntity GetDealerByMakeCity(uint cityId, uint makeId, uint modelId = 0)
        {
            try
            {
                return _dealerRepository.GetDealerByMakeCity(cityId, makeId, modelId);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetDealerByMakeCity");

                return null;
            }
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
                return _dealerRepository.FetchDealerCitiesByMake(makeId);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "FetchDealerCitiesByMake");

                return null;
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 25th March 2016
        /// Description : Calls DAL method to get dealer's bikes and details
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public DealerBikesEntity GetDealerDetailsAndBikes(uint dealerId, uint campaignId)
        {
            try
            {
                return _dealerRepository.GetDealerDetailsAndBikes(dealerId, campaignId);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetDealerDetailsAndBikes");

                return null;
            }
        }

        /// <summary>
        /// Created By : Sajal Gupta on 26/09/2016
        /// Description : Calls DAL method to get dealer's bikes and details on the basis of dealerId and makeId.
        /// Modified By : Rajan Chauhan on 16 Apr 2018
        /// Description : Added method to add minSpecs to dealerBikes
        /// </summary>
        public DealerBikesEntity GetDealerDetailsAndBikesByDealerAndMake(uint dealerId, int makeId)
        {
            DealerBikesEntity dealerBikes = null;
            try
            {
                dealerBikes = _dealerCacheRepository.GetDealerDetailsAndBikesByDealerAndMake(dealerId, makeId);
                IEnumerable<MostPopularBikesBase> bikesList = dealerBikes != null ? dealerBikes.Models : null;
                if (bikesList != null && bikesList.Any())
                {
                    var specItemList = new List<EnumSpecsFeaturesItems>{
                        EnumSpecsFeaturesItems.Displacement,
                        EnumSpecsFeaturesItems.FuelEfficiencyOverall,
                        EnumSpecsFeaturesItems.MaxPowerBhp,
                        EnumSpecsFeaturesItems.KerbWeight
                    };
                    GetVersionSpecsSummaryByItemIdAdapter adapt = new GetVersionSpecsSummaryByItemIdAdapter();
                    VersionsDataByItemIds_Input specItemInput = new VersionsDataByItemIds_Input
                    {
                        Versions = bikesList.Select(m => m.objVersion.VersionId),
                        Items = specItemList
                    };
                    adapt.AddApiGatewayCall(_apiGatewayCaller, specItemInput);
                    _apiGatewayCaller.Call();
                    IEnumerable<VersionMinSpecsEntity> specsResponseList = adapt.Output;
                    if (specsResponseList != null)
                    {
                        var specsEnumerator = specsResponseList.GetEnumerator();
                        var bikesEnumerator = bikesList.GetEnumerator();
                        while (bikesEnumerator.MoveNext())
                        {
                            if (!bikesEnumerator.Current.objVersion.VersionId.Equals(0) && specsEnumerator.MoveNext())
                            {
                                bikesEnumerator.Current.MinSpecsList = specsEnumerator.Current.MinSpecsList;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetDealerDetailsAndBikes");
            }
            return dealerBikes;
        }
        /// <summary>
        /// Craeted by  :   Sumit Kate on 21 Jun 2016
        /// Description :   Get Popular City Dealer Count
        /// Modified by :  Subodh Jain on 21 Dec 2016
        /// Description :   Merge Dealer and service center for make and model page
        /// <param name="makeId"></param>
        /// <returns></returns>
        public PopularDealerServiceCenter GetPopularCityDealer(uint makeId, uint topCount)
        {
            try
            {
                return _dealerRepository.GetPopularCityDealer(makeId, topCount);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("GetPopularCityDealer(makeId : {0})", makeId));

                return null;
            }
        }
        /// <summary>
        /// Created by  :   Sumit Kate on 18 Aug 2016
        /// Description :   Store Manufacturer Lead response
        /// </summary>
        /// <param name="pqId"></param>
        /// <param name="custEmail"></param>
        /// <param name="mobile"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public bool UpdateManufaturerLead(uint pqId, string custEmail, string mobile, string response)
        {
            try
            {
                return _dealerRepository.UpdateManufaturerLead(pqId, custEmail, mobile, response);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("UpdateManufaturerLead({0}, {1}, {2}, {3})", pqId, custEmail, mobile, response));

                return false;
            }
        }

        /// <summary>
        /// Created by  :   Sajal Gupta on 19-12-2016
        /// Description :   Fetch dealers count for nearby city.
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<NearByCityDealerCountEntity> FetchNearByCityDealersCount(uint makeId, uint cityId)
        {
            IEnumerable<NearByCityDealerCountEntity> objDealerCountList = null;
            try
            {
                if (makeId > 0 && cityId > 0)
                {
                    objDealerCountList = _dealerRepository.FetchNearByCityDealersCount(makeId, cityId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("exception in BAL layer for FetchNearByCityDealersCount {0}, {1}", makeId, cityId));

            }
            return objDealerCountList;
        }
        /// <summary>
        /// Created By : Subodh Jain on 20 Dec 2016
        /// Summary    : To bind dealers data by brand
        /// </summary>
        public IEnumerable<DealerBrandEntity> GetDealerByBrandList()
        {
            try
            {
                return _dealerRepository.GetDealerByBrandList();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetDealerByBrandList");

                return null;
            }
        }


        /// <summary>
        /// Created By : Snehal Dange on 18th Jan 2018
        /// Description: BAL layer Function for sending dealer showroom sms data from DAL.
        /// Modifier : Kartik Rathod on 30 apl 2018 changed input para of DealerShowroomDetailsSMS method
        /// </summary>
        public EnumSMSStatus GetDealerShowroomSMSData(MobileSmsVerification objData)
        {
            try
            {
                SMSData objSMSData = _dealerRepository.GetDealerShowroomSMSData(objData);

                if (objSMSData != null)
                {
                    if (objSMSData.SMSStatus == EnumSMSStatus.Success)
                    {
                        SMSTypes.DealerShowroomDetailsSMS(objSMSData, objData);
                        return EnumSMSStatus.Success;
                    }
                    else if (objSMSData.SMSStatus == EnumSMSStatus.Daily_Limit_Exceeded)
                    {
                        return EnumSMSStatus.Daily_Limit_Exceeded;
                    }
                    else
                    {
                        return EnumSMSStatus.Invalid;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Dealer.GetDealerSMSData : {0}, mobileNumber : {1}", objData.Id, objData.MobileNumber));

            }
            return 0;
        }
    }
}
