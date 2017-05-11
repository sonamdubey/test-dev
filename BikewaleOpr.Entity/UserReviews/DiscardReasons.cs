using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.UserReviews
{
    /// <summary>
    /// Created By : Ashish G. Kamble On 18 Apr 2017
    /// Summary : class to hold the values for the user reviews discard/rejection reasons
    /// </summary>
    [Serializable]
    public class DiscardReasons
    {
        public ushort Id { get; set; }
        public string Reason { get; set; }
    }
}
