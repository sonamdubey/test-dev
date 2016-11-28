using System.Collections.Generic;

namespace Bikewale.NoPhotoListingsCommunication
{
    public class NoPhotoUserListEntity
    {
        public ICollection<NoPhotoSMSEntity> objTwoDaySMSList { get; set; }
        public ICollection<NoPhotoSMSEntity> objThreeDayMailList { get; set; }
        public ICollection<NoPhotoSMSEntity> objSevenDayMailList { get; set; }

    }
}
