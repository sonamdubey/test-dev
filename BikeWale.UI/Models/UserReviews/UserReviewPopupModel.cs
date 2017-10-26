using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using System.Collections.Generic;

namespace Bikewale.Models.UserReviews
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 24th Aug 2017
    /// Description :   Model for UserReviewModel   
    /// </summary>
    public class UserReviewPopupModel
    {
        private IEnumerable<BikeMakeEntityBase> _popupMakesList = null;
        private readonly IBikeMakesCacheRepository _makeRepository = null;

        public UserReviewPopupModel(IBikeMakesCacheRepository makeRepository, IEnumerable<BikeMakeEntityBase> makesList)
        {
            if (makesList != null)
                _popupMakesList = makesList;
            _makeRepository = makeRepository;
        }
        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 24th Aug 2017
        /// Description :   Fetches makes lists.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BikeMakeEntityBase> GetMakesList()
        {
            if (_popupMakesList != null)
                return _popupMakesList;
            else
                return _makeRepository.GetMakesByType(Entities.BikeData.EnumBikeType.UserReviews);
        }
    }
}