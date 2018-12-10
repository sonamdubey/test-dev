using AutoMapper;
using Carwale.BL.Dealers.Used;
using Carwale.BL.Stock;
using Carwale.Entity.Classified;
using Carwale.Entity.Classified.Search;
using Carwale.Entity.Enum;
using Carwale.Utility.Classified;
using System;

namespace Carwale.UI.Mappers.Classified
{
    public static class UsedSearchMappers
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<StockBaseEntity, StockBaseData>()
                .ForMember(d => d.SimilarCarsUrl, o => o.MapFrom(stock =>
                    string.IsNullOrEmpty(stock.StockRecommendationsUrl)
                    ? StockRecommendationsBL.GetSimilarCarsUrl(
                        stock.ProfileId,
                        Convert.ToInt32(stock.RootId),
                        Convert.ToInt32(stock.CityId),
                        stock.DeliveryCity,
                        Convert.ToInt32(stock.PriceNumeric),
                        Convert.ToInt32(stock.VersionSubSegmentID))
                    : stock.StockRecommendationsUrl))
                .ForMember(d => d.CertProgLogoUrl, o => o.MapFrom(stock => StockBL.GetLogoUrlForStock(stock.CwBasePackageId, stock.CertProgLogoUrl)))
                .ForMember(d => d.DealerCarsUrl, o => o.MapFrom(stock => stock.CwBasePackageId == CwBasePackageId.Franchise ? UsedDealerStocksBL.GetDealerOtherCarsUrl(stock.SellerName, stock.DealerId) : string.Empty));
        }
    }
}