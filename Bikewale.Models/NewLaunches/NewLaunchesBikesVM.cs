using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Entities.Pager;
using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 30 Mar 2017
    /// Description :   NewLaunchesBikes partial view ViewModel
    /// </summary>
    public class NewLaunchesBikesVM
    {
        public NewLaunchedBikeResult Bikes { get; set; }
        public IEnumerable<BikesCountByMakeEntityBase> Makes { get; set; }
        public PagerEntity Pager { get; set; }
        public bool HasBikes { get { return (Bikes != null && Bikes.Bikes != null && Bikes.Bikes.Any()); } }
        public bool HasMakes { get { return (Makes != null && Makes.Any()); } }
        public bool HasPages { get { return (Pager != null && Pager.TotalResults > 0); } }
        public string Page_H2 { get; set; }
        public PQSourceEnum PqSource { get; set; }
    }

}
