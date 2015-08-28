using AutoMapper;
using Bikewale.DTO.Model;
using Bikewale.DTO.Series;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.Series
{
    public class SeriesEntityToDTO
    {
        internal static IEnumerable<DTO.Model.ModelBase> ConvertModelList(List<Entities.BikeData.BikeModelEntityBase> objModelsList)
        {
            Mapper.CreateMap<BikeSeriesEntityBase, SeriesBase>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            return Mapper.Map<List<BikeModelEntityBase>, List<ModelBase>>(objModelsList);
        }

        internal static IEnumerable<ModelBase> ConvertModelList(List<BikeModelEntity> objModelsList)
        {
            Mapper.CreateMap<BikeSeriesEntityBase, SeriesBase>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            return Mapper.Map<List<BikeModelEntity>, List<ModelBase>>(objModelsList);
        }
    }
}