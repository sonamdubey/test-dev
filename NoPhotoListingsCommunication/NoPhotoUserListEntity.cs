using System.Collections.Generic;

namespace Bikewale.NoPhotoListingsCommunication
{
    /// <summary>
    /// Created By:-Subodh Jain 24 Nov 2016
    /// summary:- For photo upload sms and mail
    /// </summary>
    public class NoPhotoUserListEntity
    {
        public ICollection<NoPhotoSMSEntity> objTwoDaySMSList { get; set; }
        public ICollection<NoPhotoSMSEntity> objThreeDayMailList { get; set; }
        public ICollection<NoPhotoSMSEntity> objSevenDayMailList { get; set; }

    }
}
