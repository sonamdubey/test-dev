using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using System.Collections.Generic;
using System.Linq;
namespace BikewaleOpr.Models.ManufacturerCampaign
{
    /// <summary>
    /// Created by : Aditi Srivastava on 23 Jun 2017
    /// Summary    : Page model for manufacturer campaign rules
    /// </summary>
    public class ManufacturerCampaignRules
    {
        #region Variables for dependency injection
        private readonly IManufacturerCampaignRepository _mfgCampaign = null;
        #endregion

        #region Public variables
        public uint CampaignId { get; set; }
        public uint DealerId { get; set; }
        public bool ShowOnExShowroom { get; set; }
        #endregion

        #region Constructor
        public ManufacturerCampaignRules(IManufacturerCampaignRepository mfgCampaign)
        {
            _mfgCampaign = mfgCampaign;
        }
        #endregion

        #region Functions        
        /// <summary>
        /// Created by : Aditi Srivastava on 23 Jun 2017
        /// Summary    : Function to populate manufacturer campaign rules view model
        /// </summary>
        public ManufacturerCampaignRulesVM GetData()
        {

            ManufacturerCampaignRulesVM objData = new ManufacturerCampaignRulesVM();
            objData.CampaignId = CampaignId;
            objData.NavigationWidget = new NavigationWidgetEntity();
            objData.NavigationWidget.ActivePage = 4;
            objData.NavigationWidget.CampaignId = CampaignId;
            objData.NavigationWidget.DealerId = DealerId;
            objData.ShowOnExShowroom = ShowOnExShowroom;
            IEnumerable<ManufacturerRuleEntity> rules = _mfgCampaign.GetManufacturerCampaignRules(CampaignId);

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
                }).OrderBy(x => x.Make.MakeId);

            objData.Makes = _mfgCampaign.GetBikeMakes();
            objData.States = _mfgCampaign.GetStates();
            return objData;
        }
        #endregion
    }
}