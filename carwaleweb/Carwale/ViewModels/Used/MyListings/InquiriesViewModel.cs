using Carwale.Entity.Classified.Leads;
using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carwale.UI.ViewModels.Used.MyListings
{
    public class InquiriesViewModel
    {
        public IList<ClassifiedRequest> Inquiries { get; set; }
        public Platform Platform { get; set; }
    }
}