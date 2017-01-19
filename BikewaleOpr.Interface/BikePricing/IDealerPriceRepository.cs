using BikewaleOpr.Entity.BikePricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Interface.BikePricing
{
    /// <summary>
    /// Created by : Aditi Srivastava on 18 Jan 2017
    /// Summary    : Interface for bike price categories
    /// </summary>
    public interface IDealerPriceRepository
    {
        ICollection<PriceCategoryEntity> GetAllPriceCategories();
        bool SaveBikeCategory(string categoryName);
    }
}
