using Bikewale.Notifications;
using BikewaleOpr.DALs;
using BikewaleOpr.Entity.Dealers;
using BikewaleOpr.Interface.Dealers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BikewaleOpr.BAL.BikePricing
{
    /// <summary>
    /// Created by  :   Vishnu Teja Yalakuntla on 31-Jul-2017
    /// Description :   Performs all BAL operations for Manage Dealer Pricing page.
    /// </summary>
    public class DealerPrice : IDealerPrice
    {
        private readonly IDealerPriceRepository dealerPriceRepository = null;
        public DealerPrice(IDealerPriceRepository dealerPriceRepositoryObject)
        {
            dealerPriceRepository = dealerPriceRepositoryObject;
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
                            MakeName = model.MakeName,
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
                ErrorClass objErr = new ErrorClass(ex, string.Format(
                    "GetDealerPriceQuotes cityId={0} makeId={1} dealerId={2}", cityId, makeId, dealerId));
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
            string versionIdsString = null;

            try
            {
                versionIdsString = string.Join<uint>(",", versionIds);
                isDeleted = dealerPriceRepository.DeleteVersionPrices(dealerId, cityId, versionIdsString);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format(
                    "DeleteVersionPriceQuotes dealerId={0} cityId={1} versionIdsString={2}", dealerId, cityId, versionIdsString));
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

            string versionIdsString = null;
            string itemIdsString = null;
            string itemValuesString = null;

            try
            {
                versionIdsString = string.Join<uint>(",", versionIds);
                itemIdsString = string.Join<uint>(",", itemIds);
                itemValuesString = string.Join<uint>(",", itemValues);
                isSaved = dealerPriceRepository.SaveDealerPrices(dealerId, cityId, versionIdsString, itemIdsString, itemValuesString, enteredBy);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format(
                    "SaveVersionPriceQuotes dealerId={0} cityId={1} versionIdsString={2} itemIdsString={3} itemValuesString={4} enteredBy={5}",
                    dealerId, cityId, versionIdsString, itemIdsString, itemValuesString, enteredBy));
            }
            return isSaved;
        }
    }
}
