using Bikewale.Entities.DealerLocator;
using System;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by :- Subodh Jain 30 March 2017
    /// Summary :- dealer card view model
    /// </summary>
    [Serializable]
    public class DealerCardVM
    {


        public IEnumerable<DealersList> Dealers { get; set; }
        public UInt16 TotalCount { get; set; }
        public string MakeName { get; set; }
        public string CityName { get; set; }
        public string CityMaskingName { get; set; }
        public string MakeMaskingName { get; set; }

    }
}
