using AutoMapper;
using Bikewale.ManufacturerCampaign.DTO.SearchCampaign;
using Bikewale.ManufacturerCampaign.Entities.SearchCampaign;
using BikewaleOpr.Entities.ContractCampaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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