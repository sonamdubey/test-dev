using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.UserReviews
{
    /// <summary>
    /// Created By  : Ashish G. Kamble on 15 Apr 2017
    /// Summary : Enum have values for each status of the user reviews
    /// Modified by Sajal Gupta on 04-07-2017
    /// Summary : Added AutomaticallyRejected for rejecting reviews having same emailid and modelid combination.
    /// </summary>
    public enum ReviewsStatus
    {        
        Pending = 1,
        Approved = 2,
        Discarded = 3,
        AutomaticallyRejected = 4
    }
}
