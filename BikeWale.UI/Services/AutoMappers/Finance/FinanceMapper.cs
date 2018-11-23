using AutoMapper;
using Bikewale.DTO.Finance;
using Bikewale.DTO.Finance.BajajAuto;
using Bikewale.Entities.Finance.BajajAuto;
using Bikewale.Entities.Finance.CapitalFirst;

namespace Bikewale.Service.AutoMappers.Finance
{
    public class FinanceMapper
    {
        internal static CapitalFirstVoucherEntityBase Convert(CapitalFirstVoucherDTO voucher)
        {
           return Mapper.Map<CapitalFirstVoucherDTO, CapitalFirstVoucherEntityBase>(voucher);
        }

        internal static CapitalFirstLeadResponseDTO Convert(LeadResponseMessage entity)
        {
           return Mapper.Map<LeadResponseMessage, CapitalFirstLeadResponseDTO>(entity);
        }

        internal static BajajAutoLeadResponseDto Convert(LeadResponse leadResponse)
        {
            return Mapper.Map<LeadResponse, BajajAutoLeadResponseDto>(leadResponse);
        }
    }
}