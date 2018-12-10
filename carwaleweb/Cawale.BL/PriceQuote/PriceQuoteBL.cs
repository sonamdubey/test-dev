using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using AutoMapper;
using Carwale.DTOs.Campaigns;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.CompareCars;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Price;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CompareCars;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Carwale.BL.PriceQuote
{
    public class PriceQuoteBL : IPriceQuoteBL
    {
        private readonly IPQCacheRepository _pqCachedRepo;
        private readonly ICacheManager _cacheCore;
        private readonly IPrices _pricesBL;
        private readonly ICharges _chargesBL;
        private readonly IUnityContainer _container;
      
        public PriceQuoteBL(IPQCacheRepository pqCachedRepo, ICacheManager cacheCore, IPrices pricesBL, ICharges chargesBL, IUnityContainer container)
        {
            _pqCachedRepo = pqCachedRepo;
            _cacheCore = cacheCore;
            _pricesBL = pricesBL;
            _chargesBL = chargesBL;
            _container = container;
        }

        public List<PuneThaneZones> GetSpecialPuneZones()
        {
            var puneZones = new List<PuneThaneZones>();

            puneZones.Add(new PuneThaneZones
            {
                Id = 12,
                Region = "Pune",
                Description = "Prices for Pune area vary according to the administrative unit that a car buyer resides in.PMC stands for Pune Municipal Corporation. PCMC stands for Pimpri-Chinchwad Municipal Corporation. Pune without LBT is all parts of Pune and surroundings which doesn't come under either of these corporations."
            });
            puneZones.Add(new PuneThaneZones
            {
                Id = 647,
                Region = "PCMC",
                Description = "Prices for Pune area vary according to the administrative unit that a car buyer resides in.PMC stands for Pune Municipal Corporation. PCMC stands for Pimpri-Chinchwad Municipal Corporation. Pune without LBT is all parts of Pune and surroundings which doesn't come under either of these corporations."
            });
            puneZones.Add(new PuneThaneZones
            {
                Id = 646,
                Region = "Pune without LBT",
                Description = "Prices for Pune area vary according to the administrative unit that a car buyer resides in.PMC stands for Pune Municipal Corporation. PCMC stands for Pimpri-Chinchwad Municipal Corporation. Pune without LBT is all parts of Pune and surroundings which doesn't come under either of these corporations."
            });

            return puneZones;
        }

        public List<PuneThaneZones> GetSpecialThaneZones()
        {
            var thaneZones = new List<PuneThaneZones>();

            thaneZones.Add(new PuneThaneZones
            {
                Id = 40,
                Region = "Thane with LBT",
                Description = "Prices for Thane area vary according to the administrative unit that a car buyer resides in.Parts of Thane city come under the LBT bracket. For users from all other parts, in and around Thane, the prices without LBT are applicable."
            });
            thaneZones.Add(new PuneThaneZones
            {
                Id = 645,
                Region = "Thane without LBT",
                Description = "Prices for Thane area vary according to the administrative unit that a car buyer resides in.Parts of Thane city come under the LBT bracket. For users from all other parts, in and around Thane, the prices without LBT are applicable."
            });
            return thaneZones;
        }

        public List<CarVersionEntity> GetCarVersionDetails(int modelId, int cityId)
        {
            try
            {
                var versionDetails = _cacheCore.GetFromCache<List<CarVersionEntity>>(string.Format("pq-version-details-{0}-{1}", modelId, cityId));
                if (versionDetails != null)
                {
                    return versionDetails;
                }
				//Deleting this info logging
				//CacheManager.logInfo("GetCarVersionDetails miss:{0}", string.Format("pq-version-details-{0}-{1}", modelId, cityId) ?? "NULLKEY");

                var versionDetailsDTO = new List<CarVersionEntity>();
                ICarVersions carVersionsBL = _container.Resolve<ICarVersions>();
                var carVersions = carVersionsBL.GetCarVersions(modelId, Entity.Status.New);

                foreach (var version in carVersions)
                {
                    long solidPrice = 0;
                    PQItem solidExshowroomPrice = null;
                    int versionId = version.Id;
                    var pqList = _pqCachedRepo.GetPQ(cityId, versionId).PriceQuoteList;

                    var ExshowroomPriceList = pqList.Where(c => c.Key == "Ex-Showroom Price");

                    versionDetailsDTO.Add(new CarVersionEntity
                    {
                        ID = versionId,
                        Name = version.Version,
                        MaskingName = version.MaskingName,
                        OnRoadPrice = ((solidPrice = pqList.Where(c => !c.IsMetallic).Sum(x => x.Value)) > 0 ? solidPrice : pqList.Where(c => c.IsMetallic).Sum(x => x.Value)),
                        ExshowroomPrice = ExshowroomPriceList.Any() ? (((solidExshowroomPrice = ExshowroomPriceList.FirstOrDefault(c => !c.IsMetallic)) != null) ?
                        solidExshowroomPrice.Value : ExshowroomPriceList.FirstOrDefault(c => c.IsMetallic).Value) : 0,
                        FuelType = version.CarFuelType,
                        Transmission = version.TransmissionType,
                        Displacement = version.Displacement
                    });
                }

                var carVersionDetailsDTO = versionDetailsDTO;
                versionDetailsDTO = versionDetailsDTO.Where(x => x.OnRoadPrice > 0).OrderBy(x => x.OnRoadPrice).ToList();
                versionDetailsDTO.AddRange(carVersionDetailsDTO.Where(x => x.OnRoadPrice == 0).ToList());

                if (versionDetailsDTO.Count > 0)
                {
                    _pqCachedRepo.StoreCarVersionDetails(modelId, cityId, versionDetailsDTO);
                    return versionDetailsDTO;
                }
                return null;
            }
            catch (Exception e)
            {
                var exception = new ExceptionHandler(e, "PriceQuoteBL GetCarVersionDetails()");
                exception.LogException();
                return null;
            }
        }

        /// <summary>
        /// This function returns the PQ details from encryptedId
        /// </summary>
        /// <param name="encryptedId"></param>
        /// <returns></returns>
        public PQ GetPQDetails(string encryptedId)
        {
            var pq = new PQ();
            try
            {
                string decryptUrl = HttpUtility.UrlDecode(CustomTripleDES.DecryptTripleDES(encryptedId));
                var decrpytedCarDetails = decryptUrl.Split('~');

                if (decrpytedCarDetails.Length < 5)
                    return null;

                pq.VersionId = CustomParser.parseIntObject(decrpytedCarDetails[0]);
                pq.CityId = CustomParser.parseIntObject(decrpytedCarDetails[1]);
                pq.ZoneId = decrpytedCarDetails[2];
                pq.PageId = CustomParser.parseIntObject(decrpytedCarDetails[3]);

                if (decrpytedCarDetails.Length == 5)
                {
                    pq.AreaId = CustomParser.parseIntObject(decrpytedCarDetails[4]);
                }

                return pq;
            }
            catch (Exception ex)
            {
                Carwale.Notifications.Logs.Logger.LogException(ex);
                return null;
            }
        }

        public CarDataTrackingEntity GetBasicTrackingObject(PQInput pqInput)
        {
            var trackingObject = new CarDataTrackingEntity();

            if (pqInput != null)
            {
                trackingObject.ModelId = pqInput.CarModelId;
                trackingObject.Location = Mapper.Map<CustLocation>(pqInput);
                trackingObject.Platform = pqInput.SourceId;
                trackingObject.VersionId = pqInput.CarVersionId;
            }

            return trackingObject;
        }

        public List<CarVersionEntity> GetVersions(int modelId, int cityId)
        {
            try
            {
                List<CarVersionEntity> versionDetailsList = new List<CarVersionEntity>();
                ICarVersions carVersionsBL = _container.Resolve<ICarVersions>();
                var versions = carVersionsBL.GetCarVersions(modelId, Entity.Status.New);
                var modelPrices = _pricesBL.GetOnRoadPrice(modelId, cityId);

                foreach (var version in versions)
                {
                    versionDetailsList.Add(new CarVersionEntity
                    {
                        ID = version.Id,
                        Name = version.Version,
                        FuelType = version.CarFuelType,
                        Transmission = version.TransmissionType,
                        Displacement = version.Displacement,
                        OnRoadPrice = modelPrices.Versions.Where(x => x.VersionId == version.Id).Select(p => p.OnRoadPrice).FirstOrDefault()
                    });
                }

                OrderVersionsList(ref versionDetailsList);
                return versionDetailsList;
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
            return null;
        }

        /// <summary>
        /// This function orders versions acc. to onRoadPrice and 
        /// append versions with no price at the end
        /// </summary>
        /// <param name="versionsList"></param>
        /// <returns></returns>
        private static void OrderVersionsList(ref List<CarVersionEntity> versionDetailsList)
        {
            try
            {
                if (versionDetailsList.Count > 0)
                {
                    var orderedVersionsList = versionDetailsList.Where(x => x.OnRoadPrice > 0).OrderBy(x => x.OnRoadPrice).ToList();
                    orderedVersionsList.AddRange(versionDetailsList.Where(x => x.OnRoadPrice == 0).ToList());
                    versionDetailsList = orderedVersionsList;
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
        }

        public List<Entity.Price.PriceQuote> GetVersionPrice(int modelId, int versionId, int cityId)
        {
            try
            {
                CarPriceQuote modelPrices = _pricesBL.GetModelPrices(modelId, cityId, true, true);
                List<Entity.Price.PriceQuote> pricesList = new List<Entity.Price.PriceQuote>();
                if (modelPrices == null)
                {
                    return null;
                }

                List<VersionPriceQuote> versionPrice = modelPrices.VersionPricesList.Where(x => x.VersionId == versionId
                                    && x.PricesList.Count > 0 && x.PricesList[0].PQItemId > 0).ToList();

                foreach (var priceList in versionPrice)
                {
                    Entity.Price.PriceQuote price = new Entity.Price.PriceQuote();
                    List<ChargeGroupPrice> chargeGroup = new List<ChargeGroupPrice>();
                    var chargeGroupList = priceList.PricesList.Where(x => x.ChargeGroupPrice != null);
                    List<int> chargeGroupIds = chargeGroupList.OrderBy(x => x.ChargeGroupPrice.SortOrder).Select(c => c.ChargeGroupPrice.Id).Distinct().ToList();

                    foreach (var item in chargeGroupIds)
                    {
                        var currentChargeGroup = chargeGroupList.Where(c => c.ChargeGroupPrice.Id == item).ToList();
                        var charges = currentChargeGroup.Select(c => c.ChargePrice).OrderBy(x => x.Charge.SortOrder).ToList();

                        GetChargeComponents(ref charges);

                        var newChargeGroup = currentChargeGroup.Select(cg => cg.ChargeGroupPrice).FirstOrDefault();
                        newChargeGroup.ChargePrice.AddRange(charges);

                        chargeGroup.Add(newChargeGroup);
                    }

                    price.ChargeGroup.AddRange(chargeGroup);
                    price.IsMetallic = priceList.IsMetallic;
                    price.OnRoadPrice = priceList.OnRoadPrice;
                    pricesList.Add(price);
                }

                if (pricesList.Count > 0)
                {
                    return pricesList;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        private void GetChargeComponents(ref List<ChargePrice> charges)
        {
            foreach (var charge in charges)
            {
                if (charge.Charge.Components.Count == 0)
                {
                    var components = _chargesBL.GetComponents(charge.Charge.Id);
                    if (components != null)
                    {
                        charge.Charge.Components.AddRange(components);
                    }
                }
            }
        }

        /// <summary>
        /// This function decides whether to fetch crosssell pricequote or not
        /// </summary>
        /// <param name="priceList"></param>
        /// <param name="dealerAd"></param>
        /// <returns></returns>
        public bool FetchCrossSell(List<Entity.Price.PriceQuote> priceList, Entity.Campaigns.DealerAd dealerAd)
        {
            if (priceList != null && priceList.Count > 0 && dealerAd != null && (dealerAd.CampaignType == CampaignAdType.PaidCrossSell
                 || dealerAd.CampaignType == CampaignAdType.HouseCrossSell))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// This function returns the calculate the OnRoadPrice 
        /// If solid prices are not available then calculate OnRoadPrice for metallic
        /// </summary>
        /// <param name="priceList"></param>
        /// <returns></returns>
        public long CalculateOnRoadPrice(List<PQItem> priceList)
        {
            long solidOnRoadPrice = priceList.Where(c => !c.IsMetallic).Sum(x => x.Value);
            return solidOnRoadPrice > 0 ? solidOnRoadPrice : priceList.Where(c => c.IsMetallic).Sum(x => x.Value);
        }

        /// <summary>
        /// This function returns the onRoadPrice for solid or 
        /// metallic onRoadPrice if solid is not present
        /// </summary>
        /// <param name="priceList"></param>
        /// <returns></returns>
        public long CalculateOnRoadPrice(List<Entity.Price.PriceQuote> priceList)
        {
            return priceList.Where(x => x.OnRoadPrice > 0)
                    .OrderBy(y => y.IsMetallic)
                    .Select(o => o.OnRoadPrice)
                    .FirstOrDefault();
        }
    }//class
}
