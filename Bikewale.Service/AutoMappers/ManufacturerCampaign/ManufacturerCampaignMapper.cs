using AutoMapper;
using Bikewale.DTO.ManufacturerCampaign;
using Bikewale.ManufacturerCampaign.Entities;

namespace Bikewale.Service.AutoMappers.ManufacturerCampaign
{
    public class ManufacturerCampaignMapper
    {
        internal static ManufacturerCampaignDTO Convert(ManufacturerCampaignEntity objModel)
        {
            Mapper.CreateMap<ManufacturerCampaignLeadConfiguration, ManufacturerCampaignLeadConfigurationDTO>();
            Mapper.CreateMap<ManufacturerCampaignEMIConfiguration, ManufacturerCampaignEMIConfigurationDTO>();
            Mapper.CreateMap<ManufacturerCampaignEntity, ManufacturerCampaignDTO>();
            return Mapper.Map<ManufacturerCampaignEntity, ManufacturerCampaignDTO>(objModel);
        }
    }
}