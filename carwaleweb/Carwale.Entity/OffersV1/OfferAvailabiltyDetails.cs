namespace Carwale.Entity.OffersV1
{
    public class OfferAvailabiltyDetails
    {
        public int ModelId { get; set; }
        public int CityId { get; set; }
        public bool IsOfferAvailable { get; set; }
        public int ApplicationId { get; set; }
    }
}
