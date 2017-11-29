namespace Bikewale.Entities.PriceQuote
{
    public class BikeQuotationAMPEntity 
    {
        public BikeQuotationEntity BikeQuotationEntity { get; set; }
        public string FormatedExShowroomPrice { get; set; }
        public string FormatedRTO { get; set; }
        public string FormatedInsurance { get; set; }
        public string FormatedOnRoadPrice { get; set; }

    }
}
