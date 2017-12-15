using AutoMapper;
using BikewaleOpr.DTO.BikeData;
using BikewaleOpr.DTO.Make;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikeData;

namespace BikewaleOpr.Service.AutoMappers.BikeData
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 12th Sep 2017
    /// </summary>
    public class BikeSeriesMapper
    {
        /// <summary>
        /// Created by : Vivek Singh Tomar on 12th Sep 2017
        /// Summary :  Map Bike Series List
        /// </summary>
        /// <param name="objSeries"></param>
        /// <returns></returns>
        internal static BikeSeriesDTO Convert(BikeSeriesEntity objSeries)
        {
            Mapper.CreateMap<BikeSeriesEntity, BikeSeriesDTO>()
                .ForMember(s => s.CreatedOn, c => c.MapFrom(m => m.CreatedOn.ToString("dd/MM/yyyy")))
                .ForMember(s => s.UpdatedOn, c => c.MapFrom(m => m.UpdatedOn.ToString("dd/MM/yyyy")));
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeBodyStyleEntity, BodyStyleBase>();
            return Mapper.Map<BikeSeriesEntity, BikeSeriesDTO>(objSeries);
        }
    }
}