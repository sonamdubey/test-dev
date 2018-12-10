using AutoMapper;
using Carwale.BL.Classified.CarValuation;
using Carwale.BL.Dealers.Used;
using Carwale.BL.Stock;
using Carwale.DTOs.Classified.CarDetails;
using Carwale.DTOs.Classified.Stock;
using Carwale.DTOs.Classified.Stock.Ios;
using Carwale.DTOs.Stock;
using Carwale.DTOs.Stock.Certification;
using Carwale.DTOs.Stock.Details;
using Carwale.DTOs.Stock.SimiliarCars;
using Carwale.Entity.Classified;
using Carwale.Entity.Classified.CarDetails;
using Carwale.Entity.Classified.CarValuation;
using Carwale.Entity.Enum;
using Carwale.Entity.Stock.Certification;
using Carwale.Entity.Stock.Finance;
using Carwale.Utility;
using Carwale.Utility.Classified;
using System;
using System.Configuration;
using System.Linq;

namespace Carwale.Service.Mappers
{
    public static class StockMappers
    {
        private static readonly string _hostUrl = ConfigurationManager.AppSettings["HostUrl"];
        private static readonly string _imageHostUrl = ConfigurationManager.AppSettings["CDNHostURL"];
        private static readonly string _rupeeSymbol = ConfigurationManager.AppSettings["rupeeSymbol"];
        private static readonly string _financeLinkText = ConfigurationManager.AppSettings["FinanceLinkText"];
        private const string _previewCarItemImagePath = "/cw/static/cw-apps/certificationpreview/";
        private const string _detailCarItemImagePath = "/cw/static/cw-apps/certificationdetail/";
        private static string _emiFormattedApp = "EMI {0}";
        private static readonly ushort _defaultImageQuality = Convert.ToUInt16(ConfigurationManager.AppSettings["DefaultImageQuality"] ?? "85");
        private static readonly string _apiHostUrl = ConfigurationManager.AppSettings["WebApiHostUrl"];

        public static void CreateMaps()
        {
            //Stock Details
            Mapper.CreateMap<CarDetailsEntity, StockApp>()
                .ForMember(dto => dto.ProfileId, map => map.MapFrom(src => src.BasicCarInfo.ProfileId))
                .ForMember(dto => dto.MakeName, map => map.MapFrom(src => src.BasicCarInfo.MakeName))
                .ForMember(dto => dto.ModelName, map => map.MapFrom(src => src.BasicCarInfo.ModelName))
                .ForMember(dto => dto.VersionName, map => map.MapFrom(src => src.BasicCarInfo.VersionName))
                .ForMember(dto => dto.HostUrl, map => map.UseValue(_imageHostUrl))
                .ForMember(dto => dto.OriginalImgPath, map => map.MapFrom(src => src.ImageList.ImageUrlAttributes.FirstOrDefault(url => url.IsMain).OriginalImgPath))
                .ForMember(dto => dto.Price, map => map.MapFrom(src => _rupeeSymbol + Format.FormatNumericCommaSep(src.BasicCarInfo.Price)))
                .ForMember(dto => dto.Year, map => map.MapFrom(src => src.BasicCarInfo.MakeYear.ToString("MMM yyyy")))
                .ForMember(dto => dto.Kms, map => map.MapFrom(src => src.BasicCarInfo.Kilometers))
                .ForMember(dto => dto.Fuel, map => map.MapFrom(src => !String.IsNullOrEmpty(src.BasicCarInfo.AdditionalFuel) ? src.BasicCarInfo.FuelName + "+" + src.BasicCarInfo.AdditionalFuel : src.BasicCarInfo.FuelName))
                .ForMember(dto => dto.GearBox, map => map.MapFrom(src => src.BasicCarInfo.TransmissionType))
                .ForMember(dto => dto.City, map => map.MapFrom(src => src.BasicCarInfo.CityName))
                .ForMember(dto => dto.AreaName, map => map.MapFrom(src => src.BasicCarInfo.AreaName))
                .ForMember(dto => dto.CertificationScore, map => map.MapFrom(src => StockCertificationBL.FormatCertificationScore(src.BasicCarInfo.CertificationScore)))
                .ForMember(dto => dto.ShareUrl, map => map.MapFrom(src => "https://" + _hostUrl + "/used/cars-in-" + Format.FormatSpecial(src.BasicCarInfo.CityName ?? String.Empty) + "/" + Format.FormatSpecial(src.BasicCarInfo.MakeName) + "-" + src.BasicCarInfo.MaskingName + "-" + src.BasicCarInfo.ProfileId + "/"))
                .ForMember(dto => dto.SectionsAvailable, map => map.MapFrom(src => StockBL.GetAvailableSections(src)))
                .ForMember(dto => dto.FinanceEmi, map => map.MapFrom(src => src.Finance.IsEligibleForFinance && src.Finance.Emi != null && src.Finance.Emi > 0 ? String.Format(_emiFormattedApp, Format.GetValueInINR(src.Finance.Emi)) : null))
                .ForMember(dto => dto.FinanceLinkText, map => map.MapFrom(src => src.Finance.IsEligibleForFinance ? _financeLinkText : null))
                .ForMember(dto => dto.FinanceUrl, map => map.MapFrom(src => src.Finance.IsEligibleForFinance ? (
                    StockFinanceBL.GetFinanceData(new Entity.Stock.Finance.FinanceUrlParameter
                    {
                        HostUrl = ConfigurationManager.AppSettings["CTFinanceMsite"],
                        ProfileId = src.BasicCarInfo.ProfileId,
                        MakeId = Convert.ToInt32(src.BasicCarInfo.MakeId),
                        ModelId = Convert.ToInt32(src.BasicCarInfo.ModelId),
                        MakeYear = src.BasicCarInfo.MakeYear.Year,
                        CityId = Convert.ToInt32(src.BasicCarInfo.CityId),
                        PriceNumeric = Convert.ToInt32(src.BasicCarInfo.PriceNumeric),
                        OwnerNumeric = Convert.ToInt16(src.BasicCarInfo.OwnerNumber),
                        MakeMonth = src.BasicCarInfo.MakeYear.Month
                    }).FinanceUrl
                ) : null))
                .ForMember(dto => dto.CityId, map => map.MapFrom(src => src.BasicCarInfo.CityId))
                .ForMember(dto => dto.MakeId, map => map.MapFrom(src => src.BasicCarInfo.MakeId))
                .ForMember(dto => dto.ModelId, map => map.MapFrom(src => src.BasicCarInfo.ModelId))
                .ForMember(dto => dto.ValuationUrl, map => map.MapFrom(src => "https://" + _hostUrl +
                            ValuationBL.GetValuationUrl(new ValuationUrlParameters
                            {
                                VersionId = Convert.ToInt32(src.BasicCarInfo.VersionId),
                                Year = Convert.ToInt16(src.BasicCarInfo.MakeYear.Year),
                                Owners = Convert.ToInt32(src.BasicCarInfo.OwnerNumber),
                                AskingPrice = Convert.ToInt32(src.BasicCarInfo.Price),
                                CityId = Convert.ToInt32(src.BasicCarInfo.CityId),
                                Kilometers = Convert.ToInt32(src.BasicCarInfo.KmNumeric),
                                ProfileId = src.BasicCarInfo != null ? src.BasicCarInfo.ProfileId : null
                            })
                    ))
                    .ForMember(dto => dto.ValuationText, map => map.UseValue(ConfigurationManager.AppSettings["ValuationText"]))
                    .ForMember(dto => dto.DealerRatingText, map => map.MapFrom(src => src.DealerInfo.RatingText))
                    .ForMember(dto => dto.IsChatAvailable, map => map.MapFrom(src => src.BasicCarInfo.IsChatAvailable));

            Mapper.CreateMap<BasicCarInfo, StockDTO>()
                .ForMember(dto => dto.IsDealer, map => map.MapFrom(src => src.SellerId == 1))
                .ForMember(dto => dto.Kilometers, map => map.MapFrom(src => src.KmNumeric))
                .ForMember(dto => dto.MfgDate, map => map.MapFrom(src => src.MakeYear))
                .ForMember(dto => dto.Owners, map => map.MapFrom(src => StockBL.FormatOwnerInfo(src.OwnerNumber ?? -1)))
                .ForMember(dto => dto.StockId, map => map.MapFrom(src => src.TCStockId))
                .ForMember(dto => dto.Fuel, map => map.MapFrom(src => src.FuelName))
                .ForMember(dto => dto.Transmission, map => map.MapFrom(src => src.TransmissionType))
                .ForMember(dto => dto.ImageCount, map => map.MapFrom(src => src.PhotoCount));

            Mapper.CreateMap<FeatureItems, FeatureItemApp>()
                .ForMember(dto => dto.Name, map => map.MapFrom(src => src.ItemName))
                .ForMember(dto => dto.IsAvailable, map => map.MapFrom(src => src.ItemValue));

            Mapper.CreateMap<Features, FeatureApp>()
                .ForMember(dto => dto.Items, map => map.MapFrom(src => src.FeatureItemList));

            Mapper.CreateMap<SpecItems, SpecificationItemApp>()
                .ForMember(dto => dto.Name, map => map.MapFrom(src => src.SpecName))
                .ForMember(dto => dto.Value, map => map.MapFrom(src => src.SpecValue + (String.IsNullOrEmpty(src.SpecUnit) ? "" : " " + src.SpecUnit)));

            Mapper.CreateMap<Specification, SpecificationApp>()
                .ForMember(dto => dto.Items, map => map.MapFrom(src => src.SpecificationList));

            Mapper.CreateMap<CarDetailsEntity, ExtraInfoApp>()
                .ForMember(dto => dto.OwnerComment, map => map.MapFrom(src => src.OwnerComments.SellerNote))
                .ForMember(dto => dto.ReasonForSelling, map => map.MapFrom(src => src.OwnerComments.ReasonForSell))
                .ForMember(dto => dto.Modifications, map => map.MapFrom(src => src.Modifications.Comments))
                .ForMember(dto => dto.Warranty, map => map.MapFrom(src => src.IndividualWarranty.WarrantyDescription));

            Mapper.CreateMap<CarDetailsEntity, CarConditionApp>()
                .ForMember(dto => dto.ImageHostUrl, map => map.UseValue(_imageHostUrl))
                .ForMember(dto => dto.OverallCondition, map => map.MapFrom(src => !String.IsNullOrEmpty(src.NonAbsureCarCondition.OverAll) ? src.NonAbsureCarCondition.OverAll : "Not Available"))
                .ForMember(dto => dto.CarCondition, map => map.MapFrom(src => StockBL.GetCarCondition(src.NonAbsureCarCondition)));

            //Similar Stock
            Mapper.CreateMap<StockBaseEntity, StockSummaryApp>()
                .ForMember(dto => dto.Price, map => map.MapFrom(src => _rupeeSymbol + src.Price))
                .ForMember(dto => dto.Year, map => map.MapFrom(src => src.MakeYear))
                .ForMember(dto => dto.Kms, map => map.MapFrom(src => Format.FormatNumericCommaSep(src.Km) + " km"))
                .ForMember(dto => dto.ModelName, map => map.MapFrom(src => Format.FilterModelName(src.ModelName)))
                .ForMember(dto => dto.City, map => map.MapFrom(src => src.CityName))
                .ForMember(dto => dto.CertificationScore, map => map.MapFrom(src => StockCertificationBL.FormatCertificationScore(src.CertificationScore)));

            Mapper.CreateMap<StockBaseEntity, StockSummaryDTO>()
                .ForMember(dto => dto.Price, map => map.MapFrom(src => Format.FormatFullPrice(src.Price, true)))
                .ForMember(dto => dto.Kms, map => map.MapFrom(src => Format.FormatNumericCommaSep(src.Km) + " km"))
                .ForMember(dto => dto.ModelName, map => map.MapFrom(src => Format.FilterModelName(src.ModelName)))
                .ForMember(dto => dto.Url, map => map.MapFrom(src => _hostUrl + src.Url));

            Mapper.CreateMap<StockBaseEntity, UsedCar>()
                .ForMember(dto => dto.Kms, map => map.MapFrom(stock => Format.FormatNumericCommaSep(stock.Km)))
                .ForMember(dto => dto.FormattedPrice, map => map.MapFrom(stock => Format.FormatFullPrice(stock.Price, true)))
                .ForMember(dto => dto.SmallPicUrl, map => map.MapFrom(stock => ImageSizes.CreateImageUrl(stock.HostUrl, ImageSizes._160X89, stock.OriginalImgPath, _defaultImageQuality)))
                .ForMember(dto => dto.LargePicUrl, map => map.MapFrom(stock => ImageSizes.CreateImageUrl(stock.HostUrl, ImageSizes._640X348, stock.OriginalImgPath, _defaultImageQuality)))
                .ForMember(dto => dto.UsedCarDetail, map => map.MapFrom(stock => _apiHostUrl + "UsedCarDetails/?car=" + stock.ProfileId))
                .ForMember(dto => dto.CertificationScore, map => map.MapFrom(stock => StockCertificationBL.FormatCertificationScore(stock.CertificationScore)));

            //Stock Certification
            Mapper.CreateMap<StockCertificationSubItemDetail, StockCertificationSubItemDetailApp>()
                .ForMember(dto => dto.IsAvailable, map => map.MapFrom(src => src.LegendId == 6));

            Mapper.CreateMap<StockCertificationSubItem, StockCertificationSubItemApp>()
                .ForMember(dto => dto.Score, map => map.UseValue(-1))
                .ForMember(dto => dto.ScoreColor, map => map.MapFrom(src => StockCertificationBL.GetColorFromLegendId(src.LegendId ?? 0)));

            Mapper.CreateMap<StockCertificationItem, StockCertificationItemApp>()
                .ForMember(dto => dto.PreviewImagePath, map => map.MapFrom(src => _previewCarItemImagePath + StockCertificationBL.GetCarItemImageName(src.CarItemId ?? 0)))
                .ForMember(dto => dto.DetailImagePath, map => map.MapFrom(src => _detailCarItemImagePath + StockCertificationBL.GetCarItemImageName(src.CarItemId ?? 0)))
                .AfterMap((src, dto) =>
                {
                    if (src.CarItemId == (int)CertificationCarItems.Tyres)
                    {
                        for (int i = 0; i < dto.SubItems.Count; i++)
                        {
                            string name = dto.SubItems[i].Name;
                            int tyreIndex = name.LastIndexOf(" Tyre", StringComparison.OrdinalIgnoreCase);
                            dto.SubItems[i].Name = tyreIndex > 0 ? name.Remove(tyreIndex) : name;
                            dto.SubItems[i].Score = StockCertificationBL.GetScoreFromLegendId(src.SubItems[i].LegendId ?? 0);
                        }
                    }
                });

            Mapper.CreateMap<StockCertification, StockCertificationApp>()
                .ForMember(dto => dto.MaxScore, map => map.UseValue(StockCertificationBL.MaxScore))
                .ForMember(dto => dto.ImageHostUrl, map => map.UseValue(_imageHostUrl));

            Mapper.CreateMap<StockBaseEntity, StockResultsIosBase>()
                    .ForMember(dest => dest.CertificationScore, opt => opt.MapFrom(src => StockCertificationBL.FormatCertificationScore(src.CertificationScore)))
                    .ForMember(dest => dest.FinanceEmi, opt => opt.MapFrom(src => src.IsEligibleForFinance && src.Emi > 0 ?
                                        String.Format(_emiFormattedApp, Format.GetValueInINR(src.Emi)) : null))
                    .ForMember(dest => dest.FinanceLinkText, opt => opt.MapFrom(src => src.IsEligibleForFinance ? _financeLinkText : null))
                    .ForMember(dest => dest.FinanceUrl, opt => opt.MapFrom(src => src.IsEligibleForFinance ?
                    (
                        StockFinanceBL.GetFinanceData(new FinanceUrlParameter
                        {
                            HostUrl = ConfigurationManager.AppSettings["CTFinanceMsite"],
                            ProfileId = src.ProfileId,
                            MakeId = Convert.ToInt32(src.MakeId),
                            ModelId = Convert.ToInt32(src.ModelId),
                            MakeYear = Convert.ToInt32(src.MakeYear),
                            CityId = Convert.ToInt32(src.CityId),
                            PriceNumeric = Convert.ToInt32(src.PriceNumeric),
                            OwnerNumeric = Convert.ToInt16(src.OwnerTypeId),
                            MakeMonth = src.MakeMonth
                        }).FinanceUrl
                    ) : null))
                    .ForMember(dest => dest.ValuationUrl, opt => opt.MapFrom(src => $@"https://{ _hostUrl }{
                        ValuationBL.GetValuationUrl(
                            new ValuationUrlParameters {
                                VersionId = Convert.ToInt32(src.VersionId),
                                Year = Convert.ToInt16(src.MakeYear),
                                Owners = Convert.ToInt32(src.OwnerTypeId),
                                AskingPrice = Convert.ToInt32(src.Price),
                                CityId = Convert.ToInt32(src.CityId),
                                Kilometers = Convert.ToInt32(src.Km),
                                ProfileId = src.ProfileId
                            }
                        ) }"))
                    .ForMember(dest => dest.ValuationText, opt => opt.UseValue(ConfigurationManager.AppSettings["ValuationText"]))
                    .ForMember(dest => dest.CertProgLogoUrl, opt => opt.MapFrom(src => StockBL.GetLogoUrlForStock(src.CwBasePackageId, src.CertProgLogoUrl)))
                    .ForMember(dest => dest.DealerCarsUrl, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.DealerCarsUrl)
                                                                                    ? null
                                                                                    : $"https://{ _hostUrl }{ UsedDealerStocksBL.GetDealerOtherCarsApiUrl(src.DealerId) }"))
                    .ForMember(dest => dest.StockRecommendationsUrl, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.StockRecommendationsUrl)
                                                                                            ? null
                                                                                            : $"https://{ _hostUrl }{ src.StockRecommendationsUrl }"));

            Mapper.CreateMap<StockBaseEntity, StockResultsAndroidBase>()
                    .ForMember(dest => dest.CertificationScore, opt => opt.MapFrom(src => StockCertificationBL.FormatCertificationScore(src.CertificationScore)))
                    .ForMember(dest => dest.FinanceEmi, opt => opt.MapFrom(src => src.IsEligibleForFinance && src.Emi > 0 ? String.Format(_emiFormattedApp, Format.GetValueInINR(src.Emi)) : null))
                    .ForMember(dest => dest.FinanceLinkText, opt => opt.MapFrom(src => src.IsEligibleForFinance ? _financeLinkText : null))
                    .ForMember(dest => dest.ValuationUrl, opt => opt.MapFrom(src => $@"https://{ _hostUrl }{ 
                        ValuationBL.GetValuationUrl(
                            new ValuationUrlParameters {
                                VersionId = Convert.ToInt32(src.VersionId),
                                Year = Convert.ToInt16(src.MakeYear),
                                Owners = Convert.ToInt32(src.OwnerTypeId),
                                AskingPrice = Convert.ToInt32(src.Price),
                                CityId = Convert.ToInt32(src.CityId),
                                Kilometers = Convert.ToInt32(src.Km),
                                ProfileId = src.ProfileId
                            }
                        ) }"))
                    .ForMember(dest => dest.ValuationText, opt => opt.UseValue(ConfigurationManager.AppSettings["ValuationText"]))
                    .ForMember(dest => dest.CertProgLogoUrl, opt => opt.MapFrom(src => StockBL.GetLogoUrlForStock(src.CwBasePackageId, src.CertProgLogoUrl)))
                    .ForMember(dest => dest.DealerCarsUrl, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.DealerCarsUrl) 
                                                                                    ? null 
                                                                                    : $"https://{ _hostUrl }{ UsedDealerStocksBL.GetDealerOtherCarsApiUrl(src.DealerId) }"))
                    .ForMember(dest => dest.StockRecommendationsUrl, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.StockRecommendationsUrl) 
                                                                                            ? null 
                                                                                            : $"https://{ _hostUrl }{ src.StockRecommendationsUrl }"));
        }
    }
}
