using Bikewale.Entities.BikeBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.BindViewModels.Webforms
{
    public class CommonModel
    {
        public uint GetTotalDiscount(List<PQ_Price> discountedPriceList)
        {
            uint discount = 0;
            discount = discountedPriceList.Select(o => o.Price).Aggregate((x, y) => x + y);
            return discount;
        }
    }
}