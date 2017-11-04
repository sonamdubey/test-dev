namespace Bikewale.Entities.PriceQuote
{
    public enum PriceQuoteStates
    {
        LeadSubmitted = 1,
        InitiatedBooking = 2,
        InitiatedPayment = 3,
        SuccessfulPayment = 4,
        FailurePayment = 5,
        PaymentAborted = 6,
        Cancelled = 7
    }
}
