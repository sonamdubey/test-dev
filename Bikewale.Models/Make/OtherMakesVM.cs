using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Models
{

    /// <author>
    /// Create by: Sangram Nandkhile on 13-Apr-2017
    /// Summary:  View Model for other makes
    /// Modified by: Snehal Dange on 14th Dec 2017
    /// Descritpion: Added CardText for generi card title
    /// </author>
    public class OtherMakesVM
    {
        public IEnumerable<BikeMakeEntityBase> Makes { get; set; }
        public string PageLinkFormat { get; set; }
        public string PageTitleFormat { get; set; }
        public string CardText { get; set; }
    }
}
