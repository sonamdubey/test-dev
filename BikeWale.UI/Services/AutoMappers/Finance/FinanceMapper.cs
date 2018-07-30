using AutoMapper;
using Bikewale.DTO.Finance;
using Bikewale.Entities.Finance.CapitalFirst;

namespace Bikewale.Service.AutoMappers.Finance
{
    public class FinanceMapper
    {
        internal static CapitalFirstVoucherEntityBase Convert(CapitalFirstVoucherDTO voucher)
        {
            Mapper.CreateMap<CapitalFirstVoucherDTO, CapitalFirstVoucherEntityBase>();
            Mapper.CreateMap<CapitalFirstVoucherStatusDTO, CapitalFirstVoucherStatus>();
            return Mapper.Map<CapitalFirstVoucherDTO, CapitalFirstVoucherEntityBase>(voucher);
        }

        internal static CapitalFirstLeadResponseDTO Convert(LeadResponseMessage entity)
        {
            Mapper.CreateMap<LeadResponseMessage, CapitalFirstLeadResponseDTO>();
            return Mapper.Map<LeadResponseMessage, CapitalFirstLeadResponseDTO>(entity);
        }
    }
}