namespace Bikewale.DTO.PriceQuote.BikeBooking
{
    public class DealerVersionPriceItemDTO
    {
        public string ItemName { get; set; }
        public int Price { get; set; }
        public uint DealerId { get; set; }
        public uint ItemId { get; set; }
    }
}
