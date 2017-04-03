using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// </summary>
    /// <author>
    /// Create by: Sangram Nandkhile on 03-Apr-2017
    /// Summary:  View Model for 
    /// </author>
    public class ScooterComparesVM
    {
        public ICollection<SimilarCompareBikeEntity> Bikes { get; set; }
        public string MakeName { get; set; }

    }
}
