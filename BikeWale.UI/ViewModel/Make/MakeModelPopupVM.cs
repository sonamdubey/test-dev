using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Models.Make
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 22nd Aug 2017
    /// Description :   View Model for the popup which containes Makes and Models. 
    /// Modified By :   Snehal Dange on 25th Sep 2017
    /// Descrption By:  Added makeId and modelId  
    /// </summary>
    public class UserReviewPopupVM
    {
        public IEnumerable<BikeMakeEntityBase> Makes { get; set; }
        public uint? MakeId { get; set; }
        public uint? ModelId { get; set; }

        public string MakeName { get; set; }
        public string ModelName { get; set; }
    }
}

