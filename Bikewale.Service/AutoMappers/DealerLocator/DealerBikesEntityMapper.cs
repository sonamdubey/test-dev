using AutoMapper;
using Bikewale.DTO.BikeData.v2;
using Bikewale.DTO.Dealer;
using Bikewale.DTO.DealerLocator;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.PriceQuote;
using Bikewale.DTO.PriceQuote.Area;
using Bikewale.DTO.PriceQuote.DealerPriceQuote;
using Bikewale.DTO.Version;
using Bikewale.DTO.Widgets.v2;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.PriceQuote;

namespace Bikewale.Service.AutoMappers.DealerLocator
{
    /// <summary>
    /// Modified By : Sushil Kumar on 21st March 2016
    /// Description : Changed minspecs and DealerBase DO refrence to v2 of each repectively.
    /// </summary>
    /// Created By : Lucky Rathore
    /// Created On : 22 March 2016
    /// Summary : Map DealerBikes and DealerBikesEntity
    /// </summary>
    public class DealerBikesEntityMapper
    {
        /// <summary>
        /// Created By : Lucky Rathore
        /// Created On : 22 March 2016
        /// Summary : Map DealerBikes and DealerBikesEntity
        /// </summary>
        /// <param name="dealerBikes"></param>
        /// <returns></returns>
        internal static DTO.DealerLocator.DealerBikes Convert(Entities.DealerLocator.DealerBikesEntity dealerBikes)
        {
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeVersionsListEntity, VersionBase>();
            Mapper.CreateMap<MinSpecsEntity, MinSpecs>();
            Mapper.CreateMap<MostPopularBikesBase, MostPopularBikes>();
            Mapper.CreateMap<DealerBikesEntity, DealerBikes>();

            Mapper.CreateMap<Bikewale.Entities.Location.AreaEntityBase, PQAreaBase>();
            Mapper.CreateMap<Bikewale.Entities.PriceQuote.NewBikeDealerBase, DealerBase>();
            Mapper.CreateMap<DealerPackageTypes, DealerPackageType>();
            Mapper.CreateMap<DealerDetailEntity, DealerDetail>();


            return Mapper.Map<DealerBikesEntity, DealerBikes>(dealerBikes);
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 20 May 2016
        /// Description :   Populates Dealer Bike list v2 DTO with dealer bikes entity
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        internal static System.Collections.Generic.IEnumerable<DTO.DealerLocator.v2.DealerBikeBase> ConvertV2(System.Collections.Generic.IEnumerable<MostPopularBikesBase> enumerable)
        {
            Mapper.CreateMap<Entities.BikeData.MostPopularBikesBase, DTO.DealerLocator.v2.DealerBikeBase>().ForMember(d => d.Bike, opt => opt.MapFrom(s => s.BikeName));
            Mapper.CreateMap<Entities.BikeData.MostPopularBikesBase, DTO.DealerLocator.v2.DealerBikeBase>().ForMember(d => d.VersionId, opt => opt.MapFrom(s => s.objVersion.VersionId));
            return Mapper.Map<System.Collections.Generic.IEnumerable<MostPopularBikesBase>, System.Collections.Generic.IEnumerable<DTO.DealerLocator.v2.DealerBikeBase>>(enumerable);
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 19th Dec 2017
        /// Summary : Convert dealer details and bike list model entity to dealer bike models dto
        /// </summary>
        /// <param name="dealerBikes"></param>
        /// <returns></returns>
        internal static DealerBikeModels Convert(DealerBikeModelsEntity dealerBikes)
        {
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeVersionsListEntity, VersionBase>();
            Mapper.CreateMap<MinSpecsEntity, MinSpecs>();
            Mapper.CreateMap<MostPopularBikesBase, MostPopularBikes>();
            Mapper.CreateMap<DealerBikeModelsEntity, DealerBikeModels>();

            return Mapper.Map<DealerBikeModelsEntity, DealerBikeModels>(dealerBikes);
        }

        internal static DTO.Dealer.DealerVersionPricesDTO Convert(DealerVersionPrices versionPrice)
        {
            Mapper.CreateMap<DealerVersionPrices, DealerVersionPricesDTO>();
            Mapper.CreateMap<PQ_Price, DPQ_Price>();
            return Mapper.Map<DealerVersionPrices, DealerVersionPricesDTO>(versionPrice);
        }
    }
}