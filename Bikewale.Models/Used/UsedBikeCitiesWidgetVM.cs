using Bikewale.Entities.Used;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <author>
    /// Create by: Sangram Nandkhile on 25-Mar-2017
    /// Summary:  View Model for Used bike cities
    /// </author>
    public class UsedBikeCitiesWidgetVM
    {
        public string Title { get; set; }
        public string Href { get; set; }
        public IEnumerable<UsedBikeCities> Cities { get; set; }
    }
}
