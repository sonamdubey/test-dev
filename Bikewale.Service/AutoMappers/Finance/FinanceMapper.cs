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
            return Mapper.Map<CapitalFirstVoucherDTO, CapitalFirstVoucherEntityBase>(voucher);
        }
    }
}