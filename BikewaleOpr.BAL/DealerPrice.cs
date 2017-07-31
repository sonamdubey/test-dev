﻿using Bikewale.Notifications;
using BikewaleOpr.DALs;
using BikewaleOpr.Entity;
using BikewaleOpr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BikewaleOpr.BAL
{
    /// <summary>
    /// Created by  :   Vishnu Teja Yalakuntla on 31-Jul-2017
    /// Description :   Performs all BAL operations for Manage Dealer Pricing page.
    /// </summary>
    public class DealerPrice : IDealerPrice
    {
        private readonly DealerPriceRepository dealerPriceRepository = null;
        public DealerPrice()
        {
            dealerPriceRepository = new DealerPriceRepository();
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
            IEnumerable<DealerVersionPriceEntity> dealerVersionPrices = null;
            DealerPriceBaseEntity dealerPriceBase = null;

            try
            {
                dealerPriceBase = dealerPriceRepository.GetDealerPrices(cityId, makeId, dealerId);

                if (dealerPriceBase != null && dealerPriceBase.DealerVersions != null)
                {
                    dealerVersionPrices = dealerPriceBase.DealerVersions.GroupJoin(dealerPriceBase.VersionPrices,
                        model => model.VersionId,
                        category => category.VersionId,
                        (model, categories) => new DealerVersionPriceEntity
                        {
                            MakeName = model.ModelName,
                            VersionName = model.VersionName,
                            ModelName = model.ModelName,
                            VersionId = model.VersionId,
                            Categories = categories,
                            NumberOfDays = model.NumberOfDays,
                            BikeModelId = model.BikeModelId
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                string exString = string.Format("GetDealerPriceQuotes cityId={0} makeId={1} dealerId={2}", cityId, makeId, dealerId);
                ErrorClass objErr = new ErrorClass(ex, exString);
            }

            return dealerVersionPrices;
        }
        /// <summary>
        /// Created by  :   Vishnu Teja Yalakuntla on 31-Jul-2017
        /// Description :   Constructs comma seperated delimiter array and calls DealerPriceRepository for price deletion.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <param name="versionIds"></param>
        /// <returns></returns>
        public bool DeleteVersionPriceQuotes(uint dealerId, uint cityId, IEnumerable<uint> versionIds)
        {
            bool isDeleted = false;

            string versionIdsString = string.Join<uint>(",", versionIds);

            try
            {
                isDeleted = dealerPriceRepository.DeleteVersionPrices(dealerId, cityId, versionIdsString);
            }
            catch (Exception ex)
            {
                string exString = string.Format("DeleteVersionPriceQuotes dealerId={0} cityId={1} versionIdsString={2}", dealerId, cityId, versionIdsString);
                ErrorClass objErr = new ErrorClass(ex, exString);
            }

            return isDeleted;
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
        public bool SaveVersionPriceQuotes(uint dealerId, uint cityId, IEnumerable<uint> versionIds,
             IEnumerable<uint> itemIds, IEnumerable<uint> itemValues, uint enteredBy)
        {
            bool isSaved = false;

            string versionIdsString = string.Join<uint>(",", versionIds);
            string itemIdsString = string.Join<uint>(",", itemIds);
            string itemValuesString = string.Join<uint>(",", itemValues);

            try
            {
                isSaved = dealerPriceRepository.SaveDealerPrice(dealerId, cityId, versionIdsString, itemIdsString, itemValuesString, enteredBy);
            }
            catch (Exception ex)
            {
                string exString = string.Format(
                    "SaveVersionPriceQuotes dealerId={0} cityId={1} versionIdsString={2} itemIdsString={3} itemValuesString={4} enteredBy={5}", 
                    dealerId, cityId, versionIdsString, itemIdsString, itemValuesString, enteredBy);
                ErrorClass objErr = new ErrorClass(ex, exString);
            }
            return isSaved;
        }
    }
}
