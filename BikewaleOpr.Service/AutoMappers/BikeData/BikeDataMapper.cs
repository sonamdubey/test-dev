using AutoMapper;
using BikewaleOpr.DTO.BikeData;
using BikewaleOpr.Entity.BikeData;

namespace BikewaleOpr.Service.AutoMappers.BikeData
{
    /// <summary>
    /// Created by : Sajal Gupta on 10-03-2017
    /// Description: Class for auto mapping bike data
    /// </summary>
    public class BikeDataMapper
    {
        /// <summary>
        /// Created by : Sajal Gupta on 10-03-2017
        /// Description : MAp SynopsisData to SynopsisDataDto
        /// </summary>
        /// <param name="objSynopsis"></param>
        /// <returns></returns>
        internal static SynopsisDataDto Convert(SynopsisData objSynopsis)
        {
            Mapper.CreateMap<SynopsisData, SynopsisDataDto>();
            return Mapper.Map<SynopsisData, SynopsisDataDto>(objSynopsis);
        }

        /// <summary>
        /// Created by : Sajal Gupta on 10-03-2017
        /// Description : MAp SynopsisDataDto to SynopsisData
        /// </summary>
        /// <param name="objSynopsis"></param>
        /// <returns></returns>
        internal static SynopsisData Convert(SynopsisDataDto objSynopsis)
        {
            Mapper.CreateMap<SynopsisDataDto, SynopsisData>();
            return Mapper.Map<SynopsisDataDto, SynopsisData>(objSynopsis);
        }
    }
}