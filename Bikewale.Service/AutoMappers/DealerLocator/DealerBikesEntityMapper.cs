using AutoMapper;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.DealerLocator;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.PriceQuote;
using Bikewale.DTO.PriceQuote.Area;
using Bikewale.DTO.Version;
using Bikewale.DTO.Widgets;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.DealerLocator
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created On : 22 March 2016
    /// Summary : Map DealerBikes and DealerBikesEntity
    /// </summary>
    public class DealerBikesEntityMapper
    {
        internal static DTO.DealerLocator.DealerBikes Convert(Entities.DealerLocator.DealerBikesEntity dealerBikes)
        {
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeVersionEntityBase, VersionBase>();
            Mapper.CreateMap<BikeVersionsListEntity, VersionBase>();
            Mapper.CreateMap<MinSpecsEntity, MinSpecs>();
            Mapper.CreateMap<MostPopularBikesBase, MostPopularBikes>();
            Mapper.CreateMap<DealerBikesEntity, DealerBikes>();

            Mapper.CreateMap<AreaEntityBase, PQAreaBase>();
            Mapper.CreateMap<NewBikeDealerBase, DealerBase>();
            Mapper.CreateMap<DealerPackageTypes, DealerPackageType>();
            Mapper.CreateMap<DealerDetailEntity, DealerDetail>();
            

            return Mapper.Map<DealerBikesEntity, DealerBikes>(dealerBikes);
        }
    }
}