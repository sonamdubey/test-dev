using AutoMapper;
using Bikewale.Notifications;
using BikewaleOpr.Cache;
using BikewaleOpr.Entity.Dealers;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.Dealers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BikewaleOpr.BAL.BikePricing
{
    /// <summary>
    /// Created by  :   Vishnu Teja Yalakuntla on 31-Jul-2017
    /// Description :   Performs all BAL operations for Manage Dealer Pricing page.
    /// </summary>
    public class DealerPrice : IDealerPrice
    {
        private readonly IDealerPriceRepository _dealerPriceRepository = null;
        private readonly IDealerPriceQuote _dealerPriceQuoteRepository = null;
        public DealerPrice(IDealerPriceRepository dealerPriceRepositoryObject, IDealerPriceQuote dealerPriceQuoteRepositoryObject)
        {
            _dealerPriceRepository = dealerPriceRepositoryObject;
            _dealerPriceQuoteRepository = dealerPriceQuoteRepositoryObject;
        }
        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
        /// Description :   Maps DealerVersionEntity and DealerVersionPriceEntity
        /// </summary>
        /// <param name="objDealers"></param>
        /// <returns></returns>
        private IList<DealerVersionPriceEntity> Convert(IEnumerable<DealerVersionEntity> objDealers)
        {
            Mapper.CreateMap<DealerVersionEntity, DealerVersionPriceEntity>();
            return Mapper.Map<IEnumerable<DealerVersionEntity>, IList<DealerVersionPriceEntity>>(objDealers);
        }

        /// <summary>
        /// Created by  :   Vishnu Teja Yalakuntla on 31-Jul-2017
        /// Description :   Fetches dealer pricings and performs grouping between version and category lists.
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="makeId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public IEnumerable<DealerVersionPriceEntity> GetDealerPriceQuotes(uint cityId, uint makeId, uint dealerId)
        {
            IList<DealerVersionPriceEntity> dealerVersionPrices = null;
            //DealerVersionPriceEntity dealerVersionPriceEntity = null;
            DealerPriceBaseEntity dealerPriceBase = null;
            ICollection<VersionPriceEntity> nullCategories = new List<VersionPriceEntity>();
            nullCategories.Add(new VersionPriceEntity
            {
                VersionId = 0,
                ItemCategoryId = 3,
                ItemName = "Ex-showroom",
                ItemValue = 0,
            }
            );
            nullCategories.Add(new VersionPriceEntity
            {
                VersionId = 0,
                ItemCategoryId = 5,
                ItemName = "RTO",
                ItemValue = 0,
            }
            );

            try
            {
                dealerPriceBase = _dealerPriceRepository.GetDealerPrices(cityId, makeId, dealerId);

                if (dealerPriceBase != null && dealerPriceBase.DealerVersions != null && dealerPriceBase.VersionPrices.Any())
                {
                    ICollection<VersionPriceEntity> partialNullCategories = new List<VersionPriceEntity>();

                    var temp = dealerPriceBase.VersionPrices.Select(o => new { o.ItemCategoryId, o.ItemName }).Distinct();

                    foreach (var category in temp)
                    {
                        partialNullCategories.Add(new VersionPriceEntity
                        {
                            ItemCategoryId = category.ItemCategoryId,
                            ItemName = category.ItemName
                        }
                        );
                    }

                    dealerVersionPrices = dealerPriceBase.DealerVersions.GroupJoin(dealerPriceBase.VersionPrices,
                        model => model.VersionId,
                        category => category.VersionId,
                        (model, categories) => new DealerVersionPriceEntity
                        {
                            MakeName = model.MakeName,
                            VersionName = model.VersionName,
                            ModelName = model.ModelName,
                            VersionId = model.VersionId,
                            Categories = categories != null && categories.Any() ? categories : partialNullCategories,
                            NumberOfDays = model.NumberOfDays,
                            BikeModelId = model.BikeModelId
                        }
                    ).ToList();

                    var compare = new VersionPriceEntityComparer();
                    for (int i = 0; i < dealerVersionPrices.Count(); i++)
                    {
                        if (dealerVersionPrices[i].Categories.Count() != partialNullCategories.Count)
                        {
                            dealerVersionPrices[i].Categories = dealerVersionPrices[i].Categories.Union(partialNullCategories, compare).OrderBy(category => category.ItemCategoryId);
                        }
                    }

                }
                else if (dealerPriceBase != null && (dealerPriceBase.DealerVersions != null || !dealerPriceBase.VersionPrices.Any()))
                {
                    dealerVersionPrices = Convert(dealerPriceBase.DealerVersions);
                    foreach (DealerVersionPriceEntity dealerVersionEntity in dealerVersionPrices)
                    {
                        dealerVersionEntity.Categories = nullCategories;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format(
                    "GetDealerPriceQuotes cityId={0} makeId={1} dealerId={2}", cityId, makeId, dealerId));
            }

            return dealerVersionPrices;
        }
        /// <summary>
        /// Created By  :   Sumit Kate on 11 Aug 2017
        /// Description :   Equality comparer for price categories
        /// </summary>
        private class VersionPriceEntityComparer : IEqualityComparer<VersionPriceEntity>
        {
            public bool Equals(VersionPriceEntity b1, VersionPriceEntity b2)
            {
                if (b2 == null && b1 == null)
                    return true;
                else if (b1 == null | b2 == null)
                    return false;
                else if (b1.ItemCategoryId == b2.ItemCategoryId && b1.ItemName == b2.ItemName)
                    return true;
                else
                    return false;
            }

            public int GetHashCode(VersionPriceEntity bx)
            {
                int hCode = (int)bx.ItemCategoryId;
                return hCode.GetHashCode();
            }
        }
        /// <summary>
        /// Created by  :   Vishnu Teja Yalakuntla on 31-Jul-2017
        /// Description :   Constructs comma seperated delimiter array and calls DealerPriceRepository for price deletion.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <param name="versionIds"></param>
        /// <returns></returns>
        public bool DeleteVersionPriceQuotes(uint dealerId, uint cityId, IEnumerable<uint> versionIds, IEnumerable<uint> bikeModelIds)
        {
            bool isDeleted = false;
            string versionIdsString = null;

            try
            {
                versionIdsString = string.Join<uint>(",", versionIds);
                isDeleted = _dealerPriceRepository.DeleteVersionPrices(dealerId, cityId, versionIdsString);
                if (isDeleted)
                {
                    ClearDealerPriceCache(dealerId, versionIds);
                    BwMemCache.ClearDefaultPQVersionList(bikeModelIds.Distinct(), new[] { cityId });
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format(
                    "DeleteVersionPriceQuotes dealerId={0} cityId={1} versionIdsString={2}", dealerId, cityId, versionIdsString));
            }

            return isDeleted;
        }
        /// <summary>
        ///  Created by  :   Vishnu Teja Yalakuntla on 31-Jul-2017
        ///  Description :   Constructs comma seperated delimiter arrays and calls SaveDealerPrice for price updation or insertion. Calls AddRulesOnPriceUpdation for rules update.
        /// </summary>
        /// <param name="dealerIds"></param>
        /// <param name="cityIds"></param>
        /// <param name="versionIds"></param>
        /// <param name="itemIds"></param>
        /// <param name="itemValues"></param>
        /// <param name="enteredBy"></param>
        /// <returns></returns>
        public bool SaveVersionPriceQuotes(IEnumerable<uint> dealerIds, IEnumerable<uint> cityIds, IEnumerable<uint> versionIds,
             IEnumerable<uint> itemIds, IEnumerable<uint> itemValues, uint enteredBy, IEnumerable<uint> bikeModelIds)
        {
            bool isSaved = false;

            string versionIdsString = null;
            string itemIdsString = null;
            string itemValuesString = null;
            string dealerIdsString = null;
            string cityIdsString = null;

            try
            {

                if (itemIds != null && itemValues != null && itemValues.Any() && itemIds.Any())
                {
                    itemIdsString = string.Join<uint>(",", itemIds);
                    itemValuesString = string.Join<uint>(",", itemValues);
                    versionIdsString = string.Join<uint>(",", versionIds);
                    dealerIdsString = string.Join<uint>(",", dealerIds);
                    cityIdsString = string.Join<uint>(",", cityIds);
                    isSaved = _dealerPriceRepository.SaveDealerPrices(dealerIdsString, cityIdsString, versionIdsString, itemIdsString, itemValuesString, enteredBy);

                    if (isSaved)
                    {
                        ClearDealerPriceCache(dealerIds, versionIds);
                        BwMemCache.ClearDefaultPQVersionList(bikeModelIds.Distinct(), cityIds.Distinct());
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format(
                    "SaveVersionPriceQuotes dealerId={0} cityId={1} versionIdsString={2} itemIdsString={3} itemValuesString={4} enteredBy={5}",
                    dealerIds, cityIds, versionIdsString, itemIdsString, itemValuesString, enteredBy));
            }
            return isSaved;
        }
        /// <summary>
        /// Created by  :   Vishnu Teja Yalakuntla on 31-Jul-2017
        /// Description :   Constructs comma seperated delimiter arrays and calls SaveDealerPrice for price updation or insertion.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <param name="versionIds"></param>
        /// <param name="itemIds"></param>
        /// <param name="itemValues"></param>
        /// <param name="enteredBy"></param>
        /// <returns></returns>
        public UpdatePricingRulesResponseEntity SaveVersionPriceQuotes(IEnumerable<uint> dealerIds, IEnumerable<uint> cityIds, IEnumerable<uint> versionIds,
             IEnumerable<uint> itemIds, IEnumerable<uint> itemValues, IEnumerable<uint> bikeModelIds, IEnumerable<string> bikeModelNames, uint enteredBy, uint makeId)
        {
            UpdatePricingRulesResponseEntity response = new UpdatePricingRulesResponseEntity();
            response.IsPriceSaved = false;

            string versionIdsString = null;
            string itemIdsString = null;
            string itemValuesString = null;
            string dealerIdsString = null;
            string cityIdsString = null;
            string modelIdNamesString = null;

            try
            {
                if (itemIds != null && itemValues != null && itemValues.Any() && itemIds.Any())
                {
                    itemIdsString = string.Join<uint>(",", itemIds);
                    itemValuesString = string.Join<uint>(",", itemValues);

                    versionIdsString = string.Join<uint>(",", versionIds);
                    dealerIdsString = string.Join<uint>(",", dealerIds);
                    cityIdsString = string.Join<uint>(",", cityIds);
                    modelIdNamesString = string.Join<string>(",", bikeModelIds.Zip(bikeModelNames, (modelId, modelName) => string.Format("{0}:{1}", modelId, modelName)));

                    response.IsPriceSaved = _dealerPriceRepository.SaveDealerPrices(dealerIdsString, cityIdsString, versionIdsString, itemIdsString, itemValuesString, enteredBy);
                }

                if (dealerIds.Count() == 1)
                    response.RulesUpdatedModelNames = _dealerPriceQuoteRepository.AddRulesOnPriceUpdation(modelIdNamesString, dealerIds.First(), makeId, enteredBy);

                if (response.IsPriceSaved)
                {
                    ClearDealerPriceCache(dealerIds, versionIds);
                    BwMemCache.ClearDefaultPQVersionList(bikeModelIds.Distinct(), cityIds.Distinct());
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format(
                    "SaveVersionPriceQuotes dealerId={0} cityId={1} versionIdsString={2} itemIdsString={3} itemValuesString={4} enteredBy={5}",
                    dealerIds, cityIds, versionIdsString, itemIdsString, itemValuesString, enteredBy));
            }
            return response;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Nov 2017
        /// Description :   Copy Dealer Pricing to other dealers
        /// </summary>
        /// <param name="dealerIds"></param>
        /// <param name="cityIds"></param>
        /// <param name="versionIds"></param>
        /// <param name="itemIds"></param>
        /// <param name="itemValues"></param>
        /// <param name="enteredBy"></param>
        /// <returns></returns>
        public bool CopyDealerPriceToOtherDealer(IEnumerable<uint> dealerIds, IEnumerable<uint> cityIds, IEnumerable<uint> versionIds, IEnumerable<uint> itemIds, IEnumerable<uint> itemValues, uint enteredBy, IEnumerable<uint> bikeModelIds)
        {
            bool isSaved = false;

            string versionIdsString = null;
            string distinctVersionIdsString = null;
            string itemIdsString = null;
            string itemValuesString = null;
            string dealerIdsString = null;
            string cityIdsString = null;

            try
            {

                if (itemIds != null && itemValues != null && itemValues.Any() && itemIds.Any() && versionIds != null && versionIds.Any())
                {
                    itemIdsString = string.Join<uint>(",", itemIds);
                    itemValuesString = string.Join<uint>(",", itemValues);
                    versionIdsString = string.Join<uint>(",", versionIds);
                    dealerIdsString = string.Join<uint>(",", dealerIds);
                    cityIdsString = string.Join<uint>(",", cityIds);
                    distinctVersionIdsString = string.Join<uint>(",", versionIds.Distinct());
                    foreach (var dealerId in dealerIds)
                    {
                        foreach (var cityId in cityIds)
                        {
                            isSaved = _dealerPriceRepository.DeleteVersionPrices(dealerId, cityId, distinctVersionIdsString);
                        }
                    }

                    isSaved = _dealerPriceRepository.SaveDealerPrices(dealerIdsString, cityIdsString, versionIdsString, itemIdsString, itemValuesString, enteredBy);
                    if (isSaved)
                    {
                        ClearDealerPriceCache(dealerIds, versionIds.Distinct());
                        BwMemCache.ClearDefaultPQVersionList(bikeModelIds.Distinct(), cityIds.Distinct());
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format(
                    "SaveVersionPriceQuotes dealerId={0} cityId={1} versionIdsString={2} itemIdsString={3} itemValuesString={4} enteredBy={5}",
                    dealerIds, cityIds, versionIdsString, itemIdsString, itemValuesString, enteredBy));
            }
            return isSaved;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 27 Dec 2017
        /// Description :   Clear memcache for changed dealer-version
        /// </summary>
        /// <param name="dealerIds"></param>
        /// <param name="versionIds"></param>
        private void ClearDealerPriceCache(IEnumerable<uint> dealerIds, IEnumerable<uint> versionIds)
        {
            foreach (var dealer in dealerIds)
            {
                foreach (var version in versionIds)
                {
                    BwMemCache.ClearBikeVersionPrice(dealer, version);
                }
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 27 Dec 2017
        /// Description :   Clear memcache for changed dealer-version
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="versionIds"></param>
        private void ClearDealerPriceCache(uint dealerId, IEnumerable<uint> versionIds)
        {

            foreach (var version in versionIds)
            {
                BwMemCache.ClearBikeVersionPrice(dealerId, version);
            }
        }
    }
}
