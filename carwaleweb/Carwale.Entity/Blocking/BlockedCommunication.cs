using Carwale.Entity.Blocking.Enums;

namespace Carwale.Entity.Blocking
{
    public class BlockedCommunication
    {
        public string Value { get; set; }
        public CommunicationType Type { get; set; }
        public CommunicationModule Module { get; set; }
        public string Reason { get; set; }
        public string ActionBy { get; set; }
        public bool IsBlocked { get; set; }
    }
    
}
