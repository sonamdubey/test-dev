using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.Dealer;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created By : Subodh Jain on 20 Dec 2016
    /// Summary    : To bind dealers data by brand
    /// </summary>
    public class DealersByBrand : System.Web.UI.UserControl
    {
        protected IEnumerable<DealerBrandEntity> AllDealers;
        public string WidgetTitle { get; set; }
        public int makeId { get; set; }
        public int FetchedRecordsCount;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DealerData();
        }
        private void DealerData()
        {
            try
            {
                BindDealersByBrand servicecentViewModel = new BindDealersByBrand();
                AllDealers = servicecentViewModel.GetDealerByBrandList();
                if (AllDealers != null && AllDealers.Any())
                {
                    FetchedRecordsCount = AllDealers.Count();
                    AllDealers = AllDealers.Where(v => v.MakeId != makeId);
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "DealersByBrand.Page_Load()");
                
            }
        }
    }
}