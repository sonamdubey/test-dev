using Carwale.BL.Stock.Search;
using Carwale.Entity.Classified;
using Carwale.Entity.Elastic;
using Carwale.Entity.Stock.Search;
using Carwale.Interfaces.Classified.ElasticSearch;
using Carwale.Interfaces.Classified.Slots;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.Stock;
using Carwale.Utility;
using Carwale.Utility.Classified;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.BL.Stock
{
    public class StockManager : IStockManager
    {
        private readonly ISlotsCacheRepository _slotsCacheRepository;
        private readonly IESStockQueryRepository _esStockQueryRepository;
        private readonly IStocksBySlot _stocksBySlot;
        private readonly IGeoCitiesCacheRepository _geoCitiesCacheRepository;
        public StockManager(ISlotsCacheRepository slotsCacheRepository, IESStockQueryRepository esStockQueryRepository, IStocksBySlot stocksBySlot, IGeoCitiesCacheRepository geoCitiesCacheRepository)        
        {
            _slotsCacheRepository = slotsCacheRepository;
            _esStockQueryRepository = esStockQueryRepository;
            _stocksBySlot = stocksBySlot;
            _geoCitiesCacheRepository = geoCitiesCacheRepository;
        }
        public SearchResultBase GetStocks(ElasticOuptputs filterInputs)
        {
            IEnumerable<Slot> featuredSlots = null;
            if (SearchUtility.ShouldFetchFeatureStocks(filterInputs.pn, filterInputs.so))
            {
                var cityId = filterInputs.cities.IsNotNullOrEmpty() ? Convert.ToInt32(filterInputs.cities[0]) : Convert.ToInt32(Constants.AllIndiaCityId);
                featuredSlots = _slotsCacheRepository.GetSlotsByCityId(cityId);
                if (!featuredSlots.IsNotNullOrEmpty())
                {
                    featuredSlots = _slotsCacheRepository.GetSlotsByCityId(Constants.DefaultCityIdForFeaturedSlots);
                }
                filterInputs.FeaturedSlotsCount = featuredSlots.Count(); 
            }


            var stockListByFeaturedType = _esStockQueryRepository.GetStocksForSearchResults(filterInputs);


            var finalFeaturedStocks = _stocksBySlot.GetStocksAccordingToSlot(stockListByFeaturedType.StockPackageMap, featuredSlots);
            int finalFeaturedStockCount = finalFeaturedStocks == null ? 0 : finalFeaturedStocks.Count;
            var finalNonFeaturedStock = GetFinalNonFeaturedList(stockListByFeaturedType.StockPackageMap[Entity.Enum.CwBasePackageId.Default], finalFeaturedStockCount, filterInputs);

            //Do Processing for separate featured and non featured
            //Set isPremium and isPremiumPackage
            //Need to do this during indexing and remove this
            finalFeaturedStocks = finalFeaturedStocks?.Select(s => 
            {
                s.IsPremium = true;
                s.IsPremiumPackage = true;
                return s;
            }).ToList();

            finalNonFeaturedStock = finalNonFeaturedStock?.Select(s =>
            {
                s.IsPremium = false;
                s.IsPremiumPackage = false;
                return s;
            }).ToList();
            //order by non feature listing based user preference
            //user preference root comes on top
            //preference is set for next page e.g. lead given for page 1, order listing on page 2
            if (filterInputs.userPreferredRootId > 0 && string.IsNullOrEmpty(filterInputs.sc) && (filterInputs.cars == null || filterInputs.cars.Length == 0))
            {
                finalNonFeaturedStock = finalNonFeaturedStock?.OrderBy(pdl => pdl.RootId == filterInputs.userPreferredRootId.ToString() ? 0 : 1).ToList();
            }

            List<StockBaseEntity> finalListings = new List<StockBaseEntity>();
            if (finalFeaturedStocks != null) {
                finalListings.AddRange(finalFeaturedStocks);
            }
            if(finalNonFeaturedStock != null)
            {
                finalListings.AddRange(finalNonFeaturedStock);
            }
            

            //Processing after merging
            AddMultiCityDealerDeliveryText(finalListings, filterInputs);


            //Mapping it in ResultBase
            SearchResultBase searchResultBase = new SearchResultBase();
            searchResultBase.ResultData.AddRange(finalListings);
            searchResultBase.TotalStockCount = stockListByFeaturedType.Count;
            searchResultBase.LastNonFeaturedSlotRank = filterInputs.lcr + finalNonFeaturedStock.Count;
            return searchResultBase; 
        }

        private static List<StockBaseEntity> GetFinalNonFeaturedList(List<StockBaseEntity> fullList, int selectedFeatureListingCount, ElasticOuptputs filterInputs)
        {
            int totalStocksFetched = fullList.Count + selectedFeatureListingCount;
            int pageSize = Convert.ToInt32(filterInputs.ps);
            if (totalStocksFetched > pageSize)
            {
                int maxNonFeaturedSlot = pageSize - selectedFeatureListingCount;
                if (maxNonFeaturedSlot > 0 && fullList.Count >= maxNonFeaturedSlot)
                {
                    return fullList.GetRange(0, maxNonFeaturedSlot);
                }
                return fullList;
                //slot of non featured is less than slot of featured
                //return same list
            }
            else
            {
                return fullList;
            }

        }

        private void AddMultiCityDealerDeliveryText(List<StockBaseEntity> listings, ElasticOuptputs filterInputs)
        {
            if (filterInputs.cities != null && filterInputs.cities.Length > 0 && filterInputs.cities[0] != "9999")
            {
                int deliveryCityId;
                string deliveryCity = string.Empty;
                if (filterInputs.multiCityId != 0)
                {
                    deliveryCityId = filterInputs.multiCityId;
                    deliveryCity = filterInputs.multiCityName;
                }
                else
                {

                    if (int.TryParse(filterInputs.cities[0].Trim(), out deliveryCityId))
                    {
                        deliveryCity = _geoCitiesCacheRepository.GetCityNameById(deliveryCityId.ToString());
                    }
                }

                foreach (var listing in listings)
                {
                    //Check if car comes from (multi-city dealer) and car has different city as compared to user input cities(filterInputs.cities) then show car delivery text.
                    if (listing.CityId != null && !IsCityMatch(listing.CityId, filterInputs.cities))
                    {
                        listing.DeliveryCity = deliveryCityId;
                        listing.DeliveryText = "Delivery available in " + deliveryCity;
                    }
                }
            }
        }

        private static bool IsCityMatch(string cityId, string[] filterInputCities)
        {
            foreach (var filterInputCity in filterInputCities)
            {
                if (filterInputCity.Trim().Equals(cityId.Trim()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
