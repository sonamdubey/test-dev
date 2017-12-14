using AutoMapper;
using BikewaleOpr.DTO.Dealers;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity;
using BikewaleOpr.Entity.ContractCampaign;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.Dealers;
using BikewaleOpr.Interface.Location;
using BikewaleOpr.Models.DealerPricing;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BikewaleOpr.Models.DealerBikePrice
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
    /// Description :   Populates DealerPricingSheetVM
    /// </summary>
    public class DealerPricingSheetPageModel
    {
        private readonly ILocation _location = null;
        private readonly IDealerPriceQuote _dealerPriceQuote = null;
        private readonly IDealerPrice _dealerPrice = null;
        private readonly IDealers _dealersRepository = null;

        public DealerPricingSheetPageModel(
            ILocation locationObject, IDealerPriceQuote dealerPriceQuoteObject,
            IDealerPrice dealerPriceObject, IDealers dealersRepositoryObject)
        {
            _location = locationObject;
            _dealerPriceQuote = dealerPriceQuoteObject;
            _dealerPrice = dealerPriceObject;
            _dealersRepository = dealersRepositoryObject;
        }
        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
        /// Description :   Maps BikeMakeEntityBase and BikeMakeBase
        /// </summary>
        /// <param name="objMakes"></param>
        /// <returns></returns>
        private IEnumerable<BikeMakeBase> Convert(IEnumerable<BikeMakeEntityBase> objMakes)
        {
            Mapper.CreateMap<BikeMakeEntityBase, BikeMakeBase>();
            return Mapper.Map<IEnumerable<BikeMakeEntityBase>, IEnumerable<BikeMakeBase>>(objMakes);
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
        /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
        /// Description :   Populates all common view models needed for pricing page
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="makeId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        private DealerPricingSheetPageVM GetPriceSheetAndLandingInfo(uint cityId, uint makeId, uint dealerId)
        {
            IEnumerable<CityNameEntity> dealerCities = null;

            DealerPricingSheetPageVM dealerPricingSheetInfo = new DealerPricingSheetPageVM();
            dealerPricingSheetInfo.DealerPriceSheet = new DealerPriceSheetVM();
            dealerPricingSheetInfo.CopyPricingDealers = new DealerCopyPricingVM();
            dealerPricingSheetInfo.CopyPricingCities = new CityCopyPricingVM();
            dealerPricingSheetInfo.ShowPricingCities = new ShowPricingVM();
            dealerPricingSheetInfo.AddCategoryType = new AddCategoryVM();
            dealerPricingSheetInfo.DealerOperationParams = new DealerOperationPricingVM();
            dealerPricingSheetInfo.CityId = cityId;
            dealerPricingSheetInfo.DealerOperationParams.CityId = cityId;
            dealerPricingSheetInfo.DealerOperationParams.MakeId = makeId;
            dealerPricingSheetInfo.DealerOperationParams.DealerId = dealerId;

            try
            {
                dealerCities = _location.GetDealerCities();
                dealerPricingSheetInfo.EnteredBy = System.Convert.ToUInt32(CurrentUser.Id);
                dealerPricingSheetInfo.CopyPricingDealers.Cities = dealerCities;
                dealerPricingSheetInfo.ShowPricingCities.Cities = _location.GetAllCities();
                dealerPricingSheetInfo.CopyPricingCities.States = _location.GetStates();
                dealerPricingSheetInfo.AddCategoryType.PriceCategories = _dealerPriceQuote.GetBikeCategoryItems("");
                dealerPricingSheetInfo.DealerOperationParams.DealerCities = dealerCities;
                dealerPricingSheetInfo.DealerOperationParams.Makes = _dealersRepository.GetDealerMakesByCity((int)cityId);

                dealerPricingSheetInfo.DealerOperationParams.MakesString = Bikewale.Utility.EncodingDecodingHelper.EncodeTo64(
                    Newtonsoft.Json.JsonConvert.SerializeObject(
                        Convert(
                            dealerPricingSheetInfo.DealerOperationParams.Makes)));
                dealerPricingSheetInfo.DealerOperationParams.Dealers = _dealersRepository.GetDealersByMake(makeId, cityId);

                dealerPricingSheetInfo.DealerOperationParams.DealersString = Bikewale.Utility.EncodingDecodingHelper.EncodeTo64(
                    Newtonsoft.Json.JsonConvert.SerializeObject(
                        Convert(
                            dealerPricingSheetInfo.DealerOperationParams.Dealers)));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetPriceSheetAndLandingInfo");
            }

            return dealerPricingSheetInfo;
        }

        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
        /// Description :   Populates view models for dealer cities
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="makeId"></param>
        /// <param name="dealerId"></param>
        /// <param name="dealerName"></param>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public DealerPricingSheetPageVM GetPriceSheetInformation(uint cityId, uint makeId, uint dealerId, string dealerName, string cityName)
        {
            DealerPricingSheetPageVM dealerPricingSheetInfo = null;
            ICollection<string> categoryIdsString = new List<string>();

            try
            {
                dealerPricingSheetInfo = GetPriceSheetAndLandingInfo(cityId, makeId, dealerId);
                dealerPricingSheetInfo.DealerPriceSheet.dealerVersionPricings = _dealerPrice.GetDealerPriceQuotes(cityId, makeId, dealerId);
                dealerPricingSheetInfo.DealerVersionCategories = dealerPricingSheetInfo.DealerPriceSheet.dealerVersionPricings.First().Categories;
                foreach (var categories in dealerPricingSheetInfo.DealerVersionCategories)
                {
                    categoryIdsString.Add(categories.ItemCategoryId.ToString());
                }

                dealerPricingSheetInfo.SelectedCategoriesString = Bikewale.Utility.EncodingDecodingHelper.EncodeTo64(
                    Newtonsoft.Json.JsonConvert.SerializeObject(categoryIdsString)
                );
                dealerPricingSheetInfo.PageTitle = string.Format("{0} Pricing in {1}", dealerName, cityName);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetPriceSheetInformation");
            }

            return dealerPricingSheetInfo;
        }
        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
        /// Description :   Populates view models for other cities
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="makeId"></param>
        /// <param name="dealerId"></param>
        /// <param name="otherCityId"></param>
        /// <param name="dealerName"></param>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public DealerPricingSheetPageVM GetPriceSheetInformation(uint cityId, uint makeId, uint dealerId, uint otherCityId, string dealerName, string cityName)
        {
            DealerPricingSheetPageVM dealerPricingSheetInfo = null;
            List<string> categoryIdsString = new List<string>();

            try
            {
                dealerPricingSheetInfo = GetPriceSheetAndLandingInfo(cityId, makeId, dealerId);
                dealerPricingSheetInfo.DealerPriceSheet.dealerVersionPricings = _dealerPrice.GetDealerPriceQuotes(otherCityId, makeId, dealerId);
                dealerPricingSheetInfo.DealerVersionCategories = dealerPricingSheetInfo.DealerPriceSheet.dealerVersionPricings.First().Categories;
                foreach (var categories in dealerPricingSheetInfo.DealerVersionCategories)
                {
                    categoryIdsString.Add(categories.ItemCategoryId.ToString());
                }

                dealerPricingSheetInfo.SelectedCategoriesString = Bikewale.Utility.EncodingDecodingHelper.EncodeTo64(
                    Newtonsoft.Json.JsonConvert.SerializeObject(categoryIdsString)
                );
                dealerPricingSheetInfo.OtherCityId = otherCityId;
                dealerPricingSheetInfo.PageTitle = string.Format("{0} Pricing in {1}", dealerName, cityName);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetPriceSheetInformation");
            }

            return dealerPricingSheetInfo;
        }
    }
}