using BikewaleOpr.Entities;
using System.Collections.Generic;

namespace BikewaleOpr.Models.DealerPricing
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
    /// Description :   View Model for add category collapsable on dealer pricing management page
    /// </summary>
    public class AddCategoryVM
    {
        public IEnumerable<PQ_Price> PriceCategories { get; set; }
    }
}
