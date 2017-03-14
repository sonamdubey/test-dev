using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.Shared
{
    /// <summary>
    /// Created by : Aditi Srivastava on 14 Mar 2017
    /// Summary    : model for dealer card
    /// </summary>
    public class Showrooms
    {
       public DealersEntity dealers { get; set; }
       public PopularDealerServiceCenter dealerServiceCenter { get; set; }
    }
}
