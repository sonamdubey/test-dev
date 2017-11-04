namespace BikewaleOpr.Entity.Dealers
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla
    /// Description :   Entity holding primary dealer information.
    /// </summary>
    public class DealerMakeEntity
    {
        public uint DealerId { get; set; }
        public string DealerName { get; set; }
        public uint MakeId { get; set; }
    }
}
