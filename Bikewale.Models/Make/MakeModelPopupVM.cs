using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Models.Make
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 22nd Aug 2017
    /// Description :   View Model for the popup which containes Makes and Models. 
    /// </summary>
    public class UserReviewPopupVM
    {
        public IEnumerable<BikeMakeEntityBase> Makes { get; set; }
    }
}
