using Carwale.BL.Stock;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified;
using Carwale.Entity.Classified.CarValuation;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Classified.CarValuation;
using Carwale.Interfaces.Geolocation;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace Carwale.BL.Classified.CarValuation
{
    public class ValuationBL : IValuationBL
    {
        private readonly ICarVersionCacheRepository _carVersionCacheRepo;
        private readonly IGeoCitiesCacheRepository _geoCitiesCacheRepo;
        private readonly IValuationCacheRepository _valuationCacheRepo;
        private static readonly string _hostUrl = ConfigurationManager.AppSettings["HostUrl"];

        public ValuationBL(ICarVersionCacheRepository carVersionCacheRepo, IGeoCitiesCacheRepository geoCitiesCacheRepo, IValuationCacheRepository valuationCacheRepo)
        {
            _carVersionCacheRepo = carVersionCacheRepo;
            _geoCitiesCacheRepo = geoCitiesCacheRepo;
            _valuationCacheRepo = valuationCacheRepo;
        }

        private ValuationReport FormValuationReport(short year, int city, CarVersionDetails versionDetails)
        {
            return new ValuationReport
            {
                Make = versionDetails.MakeName,
                Model = versionDetails.ModelName,
                Version = versionDetails.VersionName,
                Year = year,
                City = _geoCitiesCacheRepo.GetCityNameById(city.ToString()),
                Fuel = versionDetails.FuelType,
                CityId = city
            };
        }
        public ValuationReport GetValuationReport(short year, int versionId, int city, int? kms, UsedCarOwnerTypes owner = UsedCarOwnerTypes.NA, bool isSellingIndex = true)
        {
            ValuationReport report = null;
            if (year > 0 && versionId > 0 && city > 0)
            {
                CarVersionDetails versionDetails = _carVersionCacheRepo.GetVersionDetailsById(versionId);
                if (versionDetails != null)
                {
                    report = FormValuationReport(year, city, versionDetails);
                    ValuationUrlParameters valuationParams = FormValuationUrlParameters(year, versionId, city, owner, versionDetails, isSellingIndex);
                    if (kms != null)
                    {
                        valuationParams.Kilometers = kms.Value;
                    }
                    report.Valuation = _valuationCacheRepo.GetValuation(valuationParams);
                }
            }
            return report;
        }

        private ValuationUrlParameters FormValuationUrlParameters(short year, int versionId, int city, UsedCarOwnerTypes owner, CarVersionDetails versionDetails, bool isSellingIndex)
        {
            return new ValuationUrlParameters
            {
                MakeId = versionDetails.MakeId,
                ModelId = versionDetails.ModelId,
                VersionId = versionId,
                Year = year,
                CityId = city,
                Owners = GetValidOwnerType(owner),
                IsSellingIndex = isSellingIndex
            };
        }
        private int GetValidOwnerType(UsedCarOwnerTypes owner)
        {
            switch (owner)
            {
                case UsedCarOwnerTypes.FirstOwner:
                case UsedCarOwnerTypes.SecondOwner:
                case UsedCarOwnerTypes.ThirdOwner:
                case UsedCarOwnerTypes.FourthOwner:
                    return (int)owner;
                case UsedCarOwnerTypes.MoreThanFourOwners:
                case UsedCarOwnerTypes.FourOrMoreOwners:
                    return (int)UsedCarOwnerTypes.FourthOwner;
                default:
                    return 0;
            }

        }
        public static int GetIndicatorPosition(int fairPrice, int goodPrice, int askingPrice)
        {
            int indPos;
            int diff = goodPrice - fairPrice;
            if (askingPrice < fairPrice)
            {
                indPos = 20 - (fairPrice - askingPrice) * 20 / diff;
                if (indPos < 0)
                {
                    indPos = 0;
                }
            }
            else if (askingPrice > goodPrice)
            {
                indPos = 80 + (askingPrice - goodPrice) * 20 / diff;
                if (indPos > 100)
                {
                    indPos = 100;
                }
            }
            else
            {
                indPos = 20 + (askingPrice - fairPrice) * 60 / diff;
            }
            return indPos;
        }

        public static Dictionary<int, string> GetOwnerOptions()
        {
            return new Dictionary<int, string>()
            {
                [1] = "First owner",
                [2] = "Second owner",
                [3] = "Third owner",
                [4] = "Four or More owners",
            };
        }

        public static string GetValuationUrl(ValuationUrlParameters valuationUrlParameters)
        {
            string baseValuationUrl = string.Format("/api/valuation/?version={0}&year={1}&city={2}&askingPrice={3}&owner={4}&kms={5}",
                                                    valuationUrlParameters.VersionId, valuationUrlParameters.Year, valuationUrlParameters.CityId,
                                                    valuationUrlParameters.AskingPrice, valuationUrlParameters.Owners, valuationUrlParameters.Kilometers);
            return string.IsNullOrWhiteSpace(valuationUrlParameters.ProfileId) ?
                        baseValuationUrl : string.Format("{0}&profileid={1}", baseValuationUrl, HttpUtility.UrlEncode(valuationUrlParameters.ProfileId));
        }
    }
}
