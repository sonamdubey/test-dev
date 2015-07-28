using System;
using System.Web;

namespace Bikewale.Common
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 12 Dec 2012
    /// Summary : Class to hold information about details of the city.
    /// </summary>
    public class Cities
    {
        public string CityId { get; set; }
        public string City { get; set; }
        public string StateId { get; set; }
        public string State { get; set; }
        public string Lattitude { get; set; }
        public string Longitude { get; set; }
        public string StdCode { get; set; }
        public string DefaultPinCode { get; set; }
    }
}