using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.UserReviews
{
    /// <summary>
    /// Created by Snehal Dange on 01-09-2017
    /// Description :DTO created as input for rate-bike page
    /// </summary>
    /// <returns></returns>
    public class RateBikeInput
    {
         public uint ModelId { get; set; }
         public uint ReviewId { get; set; }
         public ulong CustomerId { get; set; }
         public ushort SourceId { get; set; }
         public ushort SelectedRating { get; set; }
         public bool IsFake { get; set; }
         
         public string ReturnUrl { get; set; }
         public ushort Contestsrc { get; set; }
    }
}
