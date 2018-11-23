using AutoMapper;
using Bikewale.DTO.MaskingNumber;
using Bikewale.Entities.MaskingNumber;

namespace Bikewale.Services.AutoMappers.MaskingNumber
{
    public class MaskingNumberMapper
    {
        /// <summary>
        /// Author  :   Kartik Rathod on 21 nov 2018
        /// Desc    :   mapper for MaskingNumberLeadEntity
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal static MaskingNumberLeadEntity Convert(MaskingNumberLeadInputDto input)
        {
            Mapper.CreateMap<MaskingNumberLeadInputDto, MaskingNumberLeadEntity>();
            return Mapper.Map<MaskingNumberLeadInputDto, MaskingNumberLeadEntity>(input); ;
        }
    }
}