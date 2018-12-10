using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Deals
{
   public class BookingReasons
    {
        public string Heading { get; set; }
        public List<ReasonsText> Reasons { get; set; }
    }

   public class ReasonsText
    {
       public string Reason;
       public string Description;

       public ReasonsText(string reason, string description)
       {
           this.Reason = reason;
           this.Description = description;
       }
    }
}
