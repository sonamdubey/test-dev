using System;

namespace BikewaleOpr.Entities
{
    public class MfgNewRulesEntity
   /// <summary>
   /// Created By: Aditi Srivastava on 29 Aug 2016
   /// Description: For inserting new rules
   /// </summary>      
    {
    public int UserId {get; set;}
    public int CampaignId {get;set;}
    public string CityIds {get;set;}
    public string ModelIds {get;set;}
    public Boolean IsAllIndia { get; set; }
    }
    
}
