using AutoMapper;
using Bikewale.DTO.Model;
using Bikewale.DTO.Series;
using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.Series
{
    public class SeriesListMapper
    {
        internal static IEnumerable<DTO.Model.ModelBase> Convert(List<Entities.BikeData.BikeModelEntityBase> objModelsList)
        {
            Mapper.CreateMap<BikeSeriesEntityBase, SeriesBase>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            return Mapper.Map<List<BikeModelEntityBase>, List<ModelBase>>(objModelsList);
        }

        internal static IEnumerable<ModelBase> Convert(List<BikeModelEntity> objModelsList)
        {
            Mapper.CreateMap<BikeSeriesEntityBase, SeriesBase>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            return Mapper.Map<List<BikeModelEntity>, List<ModelBase>>(objModelsList);
        }
    }
}