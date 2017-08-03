using BikewaleOpr.Entity.BikePricing;
using BikewaleOpr.Entity.Dealers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Interface.Dealers
{
    /// <summary>
    /// Created by : Aditi Srivastava on 18 Jan 2017
    /// Summary    : Interface for bike price categories
    /// </summary>
    public interface IDealerPriceRepository
    {
        ICollection<PriceCategoryEntity> GetAllPriceCategories();
        bool SaveBikeCategory(string categoryName);
        DealerPriceBaseEntity GetDealerPrices(uint cityId, uint makeId, uint dealerId);
        bool DeleteVersionPrices(uint dealerId, uint cityId, string versionIdList);
        bool SaveDealerPrices(uint dealerId, uint cityId, string versionIdList, string itemIdList, string itemvValueList, uint enteredBy);
    }
}
