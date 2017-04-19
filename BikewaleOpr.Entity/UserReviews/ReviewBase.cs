using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.UserReviews
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 15 Apr 2017
    /// Summary : Class should be used to hold basic values of user review
    /// </summary>
    public class ReviewBase
    {
        public uint Id { get; set; }
        public uint MakeId { get; set; }
        public string MakeName { get; set; }
        public uint ModelId { get; set; }
        public string ModelName { get; set; }
        public string WrittenBy { get; set; }
        public string EntryDate { get; set; }
        public ushort ReviewStatus { get; set; }
    }
}
