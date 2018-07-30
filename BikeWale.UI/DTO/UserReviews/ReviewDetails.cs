namespace Bikewale.Entities.DTO
{
    public class ReviewDetails
    {

        public ReviewTaggedBike Bike { get; set; }
        public Review Review { get; set; }
        public string HostUrl { get; set; }
        public string LargePicUrl { get; set; }
        public string OriginalImagePath { get; set; }
        public bool New { get; set; }  
        public bool Used { get; set; }  
        public uint NextReviewId { get; set; }
        public uint PrevReviewId { get; set; }

    }
}
