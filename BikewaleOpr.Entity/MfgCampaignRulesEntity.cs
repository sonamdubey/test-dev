using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entities
{
    /// <summary>
    /// Created By: Aditi Srivastava on 29 Aug 2016
    /// Description: For fetching campaign rules
    /// </summary>
   public class MfgCampaignRulesEntity
    {
       public int CampaignRuleId { get; set; }
       public int CampaignId{ get; set; }
	   public string  MakeName{get;set;}
	   public string ModelName{get;set;}
	   public string CityName {get; set;}
       public string StateName { get; set; }
    }
 
}
