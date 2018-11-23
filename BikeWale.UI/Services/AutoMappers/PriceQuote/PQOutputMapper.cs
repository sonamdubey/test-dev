using AutoMapper;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Series;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;

namespace Bikewale.Service.AutoMappers.PriceQuote
{
    public class PQOutputMapper
    {
        internal static DTO.PriceQuote.PQOutput Convert(Entities.BikeBooking.PQOutputEntity objPQOutput)
        {
          
            return Mapper.Map<PQOutputEntity, Bikewale.DTO.PriceQuote.PQOutput>(objPQOutput);
        }

        internal static DTO.PriceQuote.v2.PQOutput ConvertV2(Entities.BikeBooking.PQOutputEntity objPQOutput)
        {
            return Mapper.Map<PQOutputEntity, Bikewale.DTO.PriceQuote.v2.PQOutput>(objPQOutput);
        }

        internal static DTO.PriceQuote.v3.PQOutput ConvertV3(Entities.BikeBooking.v2.PQOutputEntity objPQOutput)
        {
           
            return Mapper.Map<Bikewale.Entities.BikeBooking.v2.PQOutputEntity, Bikewale.DTO.PriceQuote.v3.PQOutput>(objPQOutput);
        }

        internal static DTO.Model.ModelDetail Convert(Entities.BikeData.BikeModelEntity objModel)
        {
       
            return Mapper.Map<BikeModelEntity, ModelDetail>(objModel);
        }
    }
}