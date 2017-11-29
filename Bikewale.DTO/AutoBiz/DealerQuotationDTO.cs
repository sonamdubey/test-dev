namespace BikeWale.DTO.AutoBiz
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 26 Oct 2015
    /// </summary>
    public class DealerQuotationDTO
    {
        public NewBikeDealersDTO Dealer { get; set; }
        public uint BookingAmount { get; set; }
        public uint Availability { get; set; }
        public uint DealerId { get; set; }
    }
}
