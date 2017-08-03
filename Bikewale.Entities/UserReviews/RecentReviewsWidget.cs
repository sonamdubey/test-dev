using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UserReviews
{
    /// <summary>
    /// Created by Sajal Gupta on 02-08-2017
    /// descripton : Entity to hold data for most recent reviews widget
    /// </summary>
    [Serializable]
    public class RecentReviewsWidget
    {
        public uint ReviewId { get; set; }
        public string CustomerName { get; set; }
        public ushort Rating { get; set; }
        public string ReviewDescription { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsWinner { get; set; }
        public string ModelName { get; set; }
        public string ModelMaskingName { get; set; }
        public string MakeName { get; set; }
        public string MakeMaskingName { get; set; }
        public string ReviewTitle { get; set; }
        public string BikeName { get { return string.Format("{0} {1}", MakeName, ModelName);  } }

        public string Source { get; set; }
    }
}
