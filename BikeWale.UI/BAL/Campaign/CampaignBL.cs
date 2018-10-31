using AutoMapper;
using Bikewale.DTO.Campaign;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote.v2;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.Campaign;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.Notifications;
using BikeWale.Entities.AutoBiz;
using System;
using System.Linq;

namespace Bikewale.BAL.Campaign
{
	/// <summary>
	/// Author  : Kartik Rathod on 12 sept 2018
	/// </summary>
	public class CampaignBL : ICampaignBL
	{
		private readonly IDealerPriceQuote _dealerPriceQuote;
		private readonly ICityCacheRepository _cityCache;
		private readonly IPriceQuoteCache _objPqCache;
		public CampaignBL(IDealerPriceQuote dealerPriceQuote, ICityCacheRepository cityCache, IPriceQuoteCache pqCache)
		{
			_dealerPriceQuote = dealerPriceQuote;
			_cityCache = cityCache;
			_objPqCache = pqCache;
			Mapper.CreateMap<NewBikeDealers, DealerCampaignDto>().ForMember(output => output.Area, opt => opt.MapFrom(input => input.objArea.AreaName)).ForMember(output => output.AreaId, opt => opt.MapFrom(input => input.objArea.AreaId)).ForMember(output => output.City, opt => opt.MapFrom(input => input.objCity.CityName)).ForMember(output => output.CityId, opt => opt.MapFrom(input => input.objCity.CityId));
			Mapper.CreateMap<ManufacturerCampaignLeadConfiguration, ManufacturerLeadCampaignDto>().ForMember(output => output.PopupSuccessMessage, opt => opt.MapFrom(input => string.Format(input.PopupSuccessMessage, input.Organization))).ForMember(output => output.LeadsButtonTextMobile, opt => opt.MapFrom(input => string.Format(input.LeadsButtonTextMobile, input.Organization)));
            Mapper.CreateMap<ManufacturerCampaignEMIConfiguration, ManufacturerEmiCampaignDto>().ForMember(output => output.PopupSuccessMessage, opt => opt.MapFrom(input => string.Format(input.PopupSuccessMessage, input.Organization)));
            
		}

		/// <summary>
		/// Author  : Kartik Rathod on 12 sept 2018
		/// Desc    : get campaign details for Ds ANd Es campaings
		/// </summary>
		/// <param name="cityId"></param>
		/// <param name="areaId"></param>
		/// <param name="modelId"></param>
		/// <returns></returns>
		public ESDSCampaignDto GetCampaignLocationWise(uint cityId, uint areaId, uint modelId)
		{
			ESDSCampaignDto objESDSCampaign = new ESDSCampaignDto();
			try
			{
				if (cityId > 0 && modelId > 0)
				{
					uint versionId = 0;
					bool isDealerSubscriptionRequired = false;
					uint defaultVersionId = 0;
					DealerInfo objDealerDetail = null;

					isDealerSubscriptionRequired = CheckModelForDsCampaign(cityId, areaId, modelId);

					if (isDealerSubscriptionRequired)
					{
						objDealerDetail = _dealerPriceQuote.GetDefaultVersionAndSubscriptionDealer(modelId, cityId, areaId, versionId, isDealerSubscriptionRequired, out defaultVersionId);
					}

					if (objDealerDetail != null && objDealerDetail.IsDealerAvailable && objDealerDetail.DealerId > 0)
					{
						objESDSCampaign.DealerCampaign = GetDsCampaign(cityId, areaId, defaultVersionId, objDealerDetail.DealerId);
					}
					else // if dealer campaign is not present then fetch ManufacturerCampaign
					{
						objESDSCampaign.ManufacturerCampaign = GetEsCampaign(modelId, cityId);
					}
				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, string.Format("Exception : Bikewale.Service.Controllers.CampaignBL.GetLocationWiseCampaign() cityId({0}),areaId({1}, modelId({2}))", cityId, areaId, modelId));
			}
			return objESDSCampaign;
		}

		/// <summary>
		/// Author  : Kartik Rathod on 12 sept 2018
		/// Desc    : Get Es Campaign
		/// </summary>
		/// <param name="modelId"></param>
		/// <param name="cityId"></param>
		/// <returns>ManufacturerLeadCampaignDTO</returns>
		private ManufacturerCampaignDto GetEsCampaign(uint modelId, uint cityId)
		{
			ManufacturerCampaignDto objESCampaign = null;
			try
			{
				bool isManufacturerDealer = false;

				ManufacturerCampaignEntity objManufacturer = null;
				_dealerPriceQuote.GetManufacturerCampaignDealer(modelId, cityId, ManufacturerCampaignServingPages.Mobile_Model_Page, out objManufacturer, out isManufacturerDealer);

				if (objManufacturer != null )
				{
					objESCampaign = ConvertToManufacturerCampaign(objManufacturer);
				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, string.Format("Exception : Bikewale.Service.Controllers.CampaignBL.GetESCampaign() cityId({0}), modelId({1}))", cityId, modelId));
			}
			return objESCampaign;
		}

		/// <summary>
		/// Author  : Kartik Rathod on 12 sept 2018
		/// Desc    : Get DS Campaign 
		/// </summary>
		/// <param name="cityId"></param>
		/// <param name="areaId"></param>
		/// <param name="versionId"></param>
		/// <param name="dealerId"></param>
		/// <returns>DealerCampaignDTO</returns>
		private DealerCampaignDto GetDsCampaign(uint cityId, uint areaId, uint versionId, uint dealerId)
		{
			DealerCampaignDto objDSCampaign = null;
			try
			{
				PQParameterEntity objParams = new PQParameterEntity
				{
					DealerId = dealerId,
					VersionId = versionId,
					CityId = cityId,
					AreaId = areaId
				};
				DetailedDealerQuotationEntity objDealerQuotation = _objPqCache.GetDealerPriceQuoteByPackageV2(objParams);

				if (objDealerQuotation != null && objDealerQuotation.PrimaryDealer != null)
				{
					objDSCampaign = ConvertToDealerCampaign(objDealerQuotation.PrimaryDealer.DealerDetails);
				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, string.Format("Exception : Bikewale.Service.Controllers.CampaignBL.GetDSCampaign() cityId({0}),areaId({1}, versionId({2}))", cityId, areaId, versionId));
			}
			return objDSCampaign;
		}

		/// <summary>
		/// Author  : Kartik Rathod on 12 sept 2018
		/// Desc    : CHeck if ds campign is present for model city and area combination
		/// </summary>
		/// <param name="cityId"></param>
		/// <param name="areaId"></param>
		/// <param name="modelId"></param>
		/// <returns>bool</returns>
		private bool CheckModelForDsCampaign(uint cityId, uint areaId, uint modelId)
		{
			bool isPresent = false;
			try
			{
				var cities = _cityCache.GetPriceQuoteCities(modelId);

				if (cities != null)
				{
					CityEntityBase selectedCity = cities.FirstOrDefault(m => m.CityId == cityId);
					isPresent = selectedCity != null && ((selectedCity.HasAreas && areaId > 0) || !selectedCity.HasAreas);
				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, string.Format("Exception : Bikewale.Service.Controllers.CampaignBL.GetLocationWiseCampaign() cityId({0}),areaId({1}, modelId({2}))", cityId, areaId, modelId));
			}
			return isPresent;
		}

		/// <summary>
		/// Author  : Kartik Rathod on 12 sept 2018
		/// Desc    : convertor
		/// </summary>
		/// <param name="objPrimary">NewBikeDealers</param>
		/// <returns>DealerCampaignDTO</returns>
		private DealerCampaignDto ConvertToDealerCampaign(NewBikeDealers objPrimary)
		{

			return Mapper.Map<NewBikeDealers, DealerCampaignDto>(objPrimary);
		}

		/// <summary>
		/// Author  : Kartik Rathod on 12 sept 2018
		/// Desc    : convertor
		/// </summary>
		/// <param name="objManufacturer">ManufacturerCampaignLeadConfiguration</param>
		/// <returns>ManufacturerLeadCampaignDTO</returns>
        private ManufacturerCampaignDto ConvertToManufacturerCampaign(ManufacturerCampaignEntity objManufacturer)
		{
            ManufacturerCampaignDto objManufacturerDto = new ManufacturerCampaignDto();
            objManufacturerDto.LeadCampaign = objManufacturer.LeadCampaign != null ? Mapper.Map<ManufacturerCampaignLeadConfiguration, ManufacturerLeadCampaignDto>(objManufacturer.LeadCampaign) : null;
            objManufacturerDto.EmiCampaign = objManufacturer.EMICampaign != null ? Mapper.Map<ManufacturerCampaignEMIConfiguration, ManufacturerEmiCampaignDto>(objManufacturer.EMICampaign) : null;
            return objManufacturerDto;
		}
	}
}