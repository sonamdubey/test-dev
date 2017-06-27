using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using System.Collections.Generic;
using System.Linq;
namespace BikewaleOpr.Models.ManufacturerCampaign
{
    public class MfgCampaignRules
    {
        #region Variables for dependency injection
        private readonly IManufacturerCampaignRepository _mfgCampaign = null;
        #endregion

        #region Public variables
        public uint CampaignId { get; set; }
        public uint DealerId { get; set; }
        public bool ShowOnExShowroom {get;set;}
        #endregion

        #region Constructor
        public MfgCampaignRules(IManufacturerCampaignRepository mfgCampaign)
        {
            _mfgCampaign = mfgCampaign;
        }
        #endregion

        #region Functions        
        public ManufacturerCampaignRulesVM GetData()
        {

            ManufacturerCampaignRulesVM objData = new ManufacturerCampaignRulesVM();
            objData.CampaignId = CampaignId;
            objData.NavigationWidget = new NavigationWidgetEntity();
            objData.NavigationWidget.ActivePage = 4;
           objData.NavigationWidget.CampaignId = CampaignId;
            objData.NavigationWidget.DealerId = DealerId;
            objData.ShowOnExShowroom = ShowOnExShowroom;
            IEnumerable<MfgRuleEntity> rules = _mfgCampaign.GetManufacturerCampaignRules(CampaignId);
            IList<ManufacturerCampaignRulesEntity> tempRules = new List<ManufacturerCampaignRulesEntity>();
            objData.Rules = rules.GroupBy(x => new { x.ModelId, x.ModelName, x.MakeId, x.MakeName }).Select(
                y => new ManufacturerCampaignRulesEntity
                {
                    Make = new BikeMakeEntity
                    {
                        MakeId = y.Key.MakeId,
                        MakeName = y.Key.MakeName
                    },
                    Model = new BikeModelEntity
                    {
                        ModelId = y.Key.ModelId,
                        ModelName = y.Key.ModelName
                    },
                    State = y.GroupBy(n => new { n.StateId, n.StateName }).Select(
                   p => new StateEntity
                   {
                       StateId = p.Key.StateId,
                       StateName = p.Key.StateName,
                       Cities = p.Select(r => new CityEntity
                       {
                           CityId = r.CityId,
                           CityName = r.CityName
                       })
                   })
                }).OrderBy(x=>x.Make.MakeId);
            
            objData.Makes = _mfgCampaign.GetBikeMakes();
            objData.States = _mfgCampaign.GetStates();
            return objData;
        }
        #endregion
    }
}