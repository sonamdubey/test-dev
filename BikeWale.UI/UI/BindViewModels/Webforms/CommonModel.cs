using Bikewale.Entities.BikeBooking;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.BindViewModels.Webforms
{
    public class CommonModel
    {
        public static uint GetTotalDiscount(List<PQ_Price> discountedPriceList)
        {
            uint discount = 0;
            if (discountedPriceList != null && discountedPriceList.Count > 0)
            {
                discount = discountedPriceList.Select(o => o.Price).Aggregate((x, y) => x + y);
            }
            return discount;
        }
    }
}