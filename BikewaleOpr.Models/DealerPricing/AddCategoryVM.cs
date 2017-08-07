using BikewaleOpr.Entities;
using System.Collections.Generic;

namespace BikewaleOpr.Models.DealerPricing
{
    public class AddCategoryVM
    {
        public IEnumerable<PQ_Price> PriceCategories { get; set; }
    }
}
