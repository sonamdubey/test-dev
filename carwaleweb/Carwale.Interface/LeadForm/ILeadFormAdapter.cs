using Carwale.DTOs.LeadForm;
using Carwale.Entity.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.LeadForm
{
    public interface ILeadFormAdapter
    {
        LeadFormDto GetDealerLeadFormDetails(DealerLeadFormInput campaignInput); 
    }
}
