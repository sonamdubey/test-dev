namespace BikewaleOpr.Entity.Dealers
{
    /// <summary>
    /// Created By  : Vishnu Teja Yalakuntla on 18 Aug 2017
    /// Description : Holds response of a BAL call which updates dealer pricing and bike availability
    /// </summary>
    public class UpdatePricingRulesResponseEntity
    {
        public bool IsPriceSaved { get; set; }
        public string RulesUpdatedModelNames { get; set; }
    }
}
