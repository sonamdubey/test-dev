namespace Carwale.Entity.ES
{
    public class EsInquiry
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int VersionId { get; set; }
        public int DealerId { get; set; }
        public int ExteriorColorId { get; set; }
        public int InteriorColorId { get; set; }
        public int BookingAmount { get; set; }
        public int PlatformId { get; set; }
        public int PaymentType { get; set; }
        public bool IsTransactionCompleted { set; get; }
        public string TransactionId { set; get; }
        public bool IsApp { set; get; }
    }
}
