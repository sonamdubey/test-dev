using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using System.Collections.Generic;
using System.Linq;
namespace BikewaleOpr.Models.ManufacturerCampaign
{
    public class MfgCampaignRules
    {
        #region Variables for dependency injection
        private readonly IManufacturerCampaign _mfgCampaign = null;
        #endregion

        #region Public variables
        public uint CampaignId { get; set; }
        #endregion

        #region Constructor
        public MfgCampaignRules(IManufacturerCampaign mfgCampaign)
        {
            _mfgCampaign = mfgCampaign;
        }
        #endregion

        #region Functions        
        public ManufacturerCampaignRulesVM GetData()
        {
            ManufacturerCampaignRulesVM objData = new ManufacturerCampaignRulesVM();
            objData.CampaignId = CampaignId;
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
                });
            
            objData.Makes = _mfgCampaign.GetBikeMakes();
            objData.States = _mfgCampaign.GetStates();
            return objData;
        }
        #endregion
    }
}