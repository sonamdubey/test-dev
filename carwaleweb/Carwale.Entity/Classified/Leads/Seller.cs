using System;

namespace Carwale.Entity.Classified.Leads
{
    [Serializable]
    public class Seller
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public string DisplayNumber { get; set; }
        public string Address { get; set; }
        public int Id { get; set; }
        public string City { get; set; }
        public bool IsDealerPageAvailable { get; set; }
        public string ChatUserId { get; set; }
        public string WhatsAppNumber { get; set; }
    }
}
