using AutoMapper;
using BikewaleOpr.DTO.Dealers;
using BikewaleOpr.Entity.Dealers;

namespace BikewaleOpr.Service.AutoMappers.Dealer
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 17 Aug 2017
    /// Description :   Maps between save dealer pricing reponses.
    /// </summary>
    public class SaveDealerPricingResponseMapper
    {
        internal static SaveDealerPricingResponseDTO Convert(UpdatePricingRulesResponseEntity savePricesResponse, bool isAvailabilitySaved)
        {
            Mapper.CreateMap<UpdatePricingRulesResponseEntity, SaveDealerPricingResponseDTO>().ForMember(dest => dest.IsPriceSaved, opt => opt.MapFrom(s => s.IsPriceSaved));
            Mapper.CreateMap<UpdatePricingRulesResponseEntity, SaveDealerPricingResponseDTO>().ForMember(dest => dest.RulesUpdatedModelNames, opt => opt.MapFrom(s => s.RulesUpdatedModelNames));
            var apiResponseData = Mapper.Map<UpdatePricingRulesResponseEntity, SaveDealerPricingResponseDTO>(savePricesResponse);
            apiResponseData.IsAvailabilitySaved = isAvailabilitySaved;
            return apiResponseData;
        }
    }
}
