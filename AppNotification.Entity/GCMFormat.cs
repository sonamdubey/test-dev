using System.Collections.Generic;

namespace AppNotification.Entity
{
    public class GCMFormat
    {
        public string to { get; set; }
        public List<string> registration_tokens { get; set; }
    }
}
