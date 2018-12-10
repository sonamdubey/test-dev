using Carwale.Entity.PriceQuote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.BL.PriceQuote
{
    public class LeadAndInquirySource
    {
       public LeadSource GetLeadAndInquirySource(string text, List<LeadSource> leadSource)
        {
            foreach (var lead in leadSource)
            {
                if (lead.LeadClickSourceDesc == text)
                    return lead;
            }
            return null;
        }
    }
}
