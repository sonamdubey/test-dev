using System;

namespace Carwale.Entity.Classified.Leads
{
    public class ClassifiedRequest
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerMobile { get; set; }
        public DateTime RequestDateTime { get; set; }
        public string SourceIdentifier { get; set; } = "carwale";
    }
}
