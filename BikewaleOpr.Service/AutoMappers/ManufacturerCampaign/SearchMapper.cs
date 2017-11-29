using AutoMapper;
using Bikewale.ManufacturerCampaign.DTO.SearchCampaign;
using Bikewale.ManufacturerCampaign.Entities.SearchCampaign;
using System.Collections.Generic;

namespace BikewaleOpr.Service.AutoMappers.ManufacturerCampaign
{
    public class SearchMapper
    {
        internal static IEnumerable<ManufacturerCampaignDetailsDTO> Convert(IEnumerable<ManufacturerCampaignDetailsList> _objMfgList)
        {
            Mapper.CreateMap<ManufacturerCampaignDetailsList, ManufacturerCampaignDetailsDTO>();
            return Mapper.Map<IEnumerable<ManufacturerCampaignDetailsList>, IEnumerable<ManufacturerCampaignDetailsDTO>>(_objMfgList);
        }
    }
}