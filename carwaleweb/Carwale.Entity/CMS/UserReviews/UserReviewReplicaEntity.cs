using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS.UserReviews
{
    public class UserReviewReplicaEntity
    {
        public int Id { get; set; }
        public int ReviewId { get; set; }
        public int StyleR { get; set; }
        public int ComfortR { get; set; }
        public int PerformanceR { get; set; }
        public int ValueR { get; set; }
        public int FuelEconomyR { get; set; }
        public float OverallR { get; set; }
        public string Pros { get; set; }
        public string Cons { get; set; }
        public string Comments { get; set; }
        public string Title { get; set; }
        public string LastUpdatedOn { get; set; }
        public int LastUpdatedBy { get; set; }
        public int IsOwned { get; set; }
        public int IsNewlyPurchased { get; set; }
        public int Familiarity { get; set; }
        public double Mileage { get; set; }
        public int ModelId { get; set; }
        public int VersionId { get; set; }
        public string EntryDateTime { get; set; }
        public int Liked { get; set; }
        public int Disliked { get; set; }
        public int Viewed { get; set; }
        public int ModeratorRecommendedReview { get; set; }
        public int CustomerId { get; set; }
        public int PlatformId { get; set; }
    }
}
