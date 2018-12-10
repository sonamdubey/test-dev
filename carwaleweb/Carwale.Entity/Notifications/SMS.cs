using Carwale.Entity.Enum;

namespace Carwale.Entity.Notifications
{
    public class SMS
    {
        public string Mobile { get; set; }
        public string Message { get; set; }
        public string PageUrl { get; set; }
        public int SMSType { get; set; }
        public bool Status { get; set; }
        public string ReturnedMsg { get; set; }
        public bool IsPriority { get; set; }
        public Platform Platform { get; set; }
        public string IpAddress { get; set; }
        public SMSType SourceModule { get; set; }
    }
}
