using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.CarDetails
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 27 May 2015
    /// </summary>
    [Serializable]
    public class OwnersComment
    {
        public string SellerNote { get; set; }
        public string ReasonForSell { get; set; }
    }
}
