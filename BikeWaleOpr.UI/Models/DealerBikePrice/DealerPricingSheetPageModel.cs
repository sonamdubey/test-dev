using AutoMapper;
using Bikewale.Notifications;
using BikewaleOpr.DTO.Dealers;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity;
using BikewaleOpr.Entity.ContractCampaign;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.Dealers;
using BikewaleOpr.Interface.Location;
using BikewaleOpr.Models.DealerPricing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BikewaleOpr.Models.DealerBikePrice
{
    public class DealerPricingSheetPageModel
    {
        private readonly ILocation location = null;
        private readonly IDealerPriceQuote dealerPriceQuote = null;
        private readonly IDealerPrice dealerPrice = null;
        private readonly IDealers dealersRepository = null;

        public DealerPricingSheetPageModel(
            ILocation locationObject, IDealerPriceQuote dealerPriceQuoteObject,
            IDealerPrice dealerPriceObject, IDealers dealersRepositoryObject)
        {
            location = locationObject;
            dealerPriceQuote = dealerPriceQuoteObject;
            dealerPrice = dealerPriceObject;
            dealersRepository = dealersRepositoryObject;
        }

        internal static IEnumerable<BikeMakeBase> Convert(IEnumerable<BikeMakeEntityBase> objMakes)
        {
            Mapper.CreateMap<BikeMakeEntityBase, BikeMakeBase>();
            return Mapper.Map<IEnumerable<BikeMakeEntityBase>, IEnumerable<BikeMakeBase>>(objMakes);
        }

        internal static IEnumerable<DealerBase> Convert(IEnumerable<DealerEntityBase> objDealers)
        {
            Mapper.CreateMap<DealerEntityBase, DealerBase>();
            return Mapper.Map<IEnumerable<DealerEntityBase>, IEnumerable<DealerBase>>(objDealers);
        }


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
            dealerPricingSheetInfo.MakeId = makeId;
            dealerPricingSheetInfo.DealerId = dealerId;

            try
            {
                dealerCities = location.GetDealerCities();
                dealerPricingSheetInfo.CopyPricingDealers.Cities = dealerCities;
                dealerPricingSheetInfo.ShowPricingCities.Cities = location.GetAllCities();
                dealerPricingSheetInfo.CopyPricingCities.States = location.GetStates();
                dealerPricingSheetInfo.AddCategoryType.PriceCategories = dealerPriceQuote.GetBikeCategoryItems("");
                dealerPricingSheetInfo.DealerOperationParams.DealerCities = dealerCities;
                dealerPricingSheetInfo.DealerOperationParams.Makes = dealersRepository.GetDealerMakesByCity((int)cityId);
                Byte[] StringBytes = System.Text.Encoding.UTF8.GetBytes(
                    Newtonsoft.Json.JsonConvert.SerializeObject(
                        DealerPricingSheetPageModel.Convert(
                            dealerPricingSheetInfo.DealerOperationParams.Makes)));
                dealerPricingSheetInfo.DealerOperationParams.MakesString = System.Convert.ToBase64String(StringBytes);
                dealerPricingSheetInfo.DealerOperationParams.Dealers = dealersRepository.GetDealersByMake(makeId, cityId);
                StringBytes = System.Text.Encoding.UTF8.GetBytes(
                    Newtonsoft.Json.JsonConvert.SerializeObject(
                        DealerPricingSheetPageModel.Convert(
                            dealerPricingSheetInfo.DealerOperationParams.Dealers)));
                dealerPricingSheetInfo.DealerOperationParams.DealersString = System.Convert.ToBase64String(StringBytes);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetPriceSheetAndLandingInfo");
            }

            return dealerPricingSheetInfo;
        }


        public DealerPricingSheetPageVM GetPriceSheetInformation(uint cityId, uint makeId, uint dealerId)
        {
            DealerPricingSheetPageVM dealerPricingSheetInfo = null;

            try
            {
                dealerPricingSheetInfo = GetPriceSheetAndLandingInfo(cityId, makeId, dealerId);
                dealerPricingSheetInfo.DealerPriceSheet.dealerVersionPricings = dealerPrice.GetDealerPriceQuotes(cityId, makeId, dealerId);
                dealerPricingSheetInfo.dealerVersionCategories = dealerPricingSheetInfo.DealerPriceSheet.dealerVersionPricings.First().Categories;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetPriceSheetInformation");
            }

            return dealerPricingSheetInfo;
        }

        public DealerPricingSheetPageVM GetPriceSheetInformation(uint cityId, uint makeId, uint dealerId, uint otherCityId)
        {
            DealerPricingSheetPageVM dealerPricingSheetInfo = null;

            try
            {
                dealerPricingSheetInfo = GetPriceSheetAndLandingInfo(cityId, makeId, dealerId);
                dealerPricingSheetInfo.DealerPriceSheet.dealerVersionPricings = dealerPrice.GetDealerPriceQuotes(otherCityId, makeId, dealerId);
                dealerPricingSheetInfo.dealerVersionCategories = dealerPricingSheetInfo.DealerPriceSheet.dealerVersionPricings.First().Categories;
                dealerPricingSheetInfo.OtherCityId = otherCityId;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetPriceSheetInformation");
            }

            return dealerPricingSheetInfo;
        }
    }
}