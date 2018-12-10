using Carwale.Entity.Geolocation;
using System.Collections.Generic;

namespace Carwale.Entity.CompareCars
{
    public class CompareCarInputParam
    {
        public List<int> VersionIds { get; set; }
        public Location CustLocation { get; set; }
        public string CwcCookie { get; set; }
    }
}
