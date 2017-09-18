namespace Bikewale.Entities.Finance.CapitalFirst
{
    public class LeadResponseMessage
    {
        public uint CpId { get; set; }
        public uint CTleadId { get; set; }
        public uint LeadId { get; set; }
        public string Message { get; set; }
        public ushort Status { get; set; }
    }
}
