using System;
using System.Collections.Generic;

namespace Carwale.Entity.Classified.Leads
{
    [Serializable]
    public class C2BLeadResponse
    {
        public List<C2BLead> Result { get; set; }
        public int Status { get; set; }
        public string Details { get; set; }
    }
}
