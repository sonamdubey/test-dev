namespace Carwale.Entity.Classified.Leads
{
    public enum LeadStatus
    {
        Valid = 0,
        Duplicate = 1,
        MobileLimitExceeded = 2,
        IpLimitExceeded = 3,
        Unverified = 4,
        MobileBlocked = 5,
        IpBlocked = 6,
        InvalidChatSmsLead = 7              //this status will be return when user is coming from chat sms and hasn't given lead to this stock
    }
}
