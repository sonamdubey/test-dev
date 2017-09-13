using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UserReviews.V2
{
    /// <summary>
    /// Modified by Snehal Dange on 11 Sep 2017
    /// Description: Added 3 para :ReviewRate ,RatingsCount, ReviewCount
    /// </summary>
    [Serializable]
    public class UserReviewSummary
    {
        public uint ReviewId { get; set; }
        public ushort OverallRatingId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }


        //  public UserReviewOverallRating OverallRating { get; set; }

        //  public string ShortDescription { get; set; }

        //  public string TipsDescriptionSmall { get; set; }
        //   public string Tips { get; set; }

        //   public IEnumerable<UserReviewQuestion> Questions { get; set; }
        //   public ulong CustomerId { get; set; }
        //   public string CustomerName { get; set; }
        //  public string CustomerEmail { get; set; }
        //   public string ReturnUrl { get; set; }
        //   public uint UpVotes { get; set; }
        //   public ushort PlatformId { get; set; }
        //   public uint DownVotes { get; set; }
        //   public uint Views { get; set; }
        //   public DateTime EntryDate { get; set; }
        //   public string ReviewAge { get; set; }
        //   public uint OldReviewId { get; set; }
        //
        //  public BikeMakeEntityBase Make { get; set; }
        //  public BikeModelEntityBase Model { get; set; }
        //  public string OriginalImagePath { get; set; }
        //  public string HostUrl { get; set; }
        //  public bool IsDiscontinued { get; set; }
        //  public bool IsUpcoming { get; set; }
        //  public bool IsRatingQuestion { get; set; }
        //  public uint TotalRatings { get; set; }
        //  public uint TotalReviews { get; set; }
        //  public float OverAllModelRating { get; set; }
        //  public string Mileage { get; set; }
        //    public ushort RatingQuestionsCount { get; set; }

        //    public UInt16 RatingCount { get; set; }
        //    public float ReviewCount { get; set; }
        //    public float ReviewRate { get; set; }


    }
}
