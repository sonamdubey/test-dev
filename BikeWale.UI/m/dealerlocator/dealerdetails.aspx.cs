using Bikewale.DAL.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Interfaces.Dealer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Bikewale.Mobile
{
    public class DealerDetails : System.Web.UI.Page
    {
        protected Repeater rptModels;
        protected UInt16 dealerId;
        protected DealerBikesEntity dealer;
        protected DealerDetailEntity dealerDetail;
        protected bool isDealerDetail;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if(!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["dId"]))
                {
                    dealerId =  Convert.ToUInt16(HttpContext.Current.Request.QueryString["dId"]);
                    IDealer dealerModels = new DealersRepository();//Ask ??
                    dealer = dealerModels.GetDealerBikes(dealerId);
                    if (dealer != null)
                    {
                        if (dealer.DealerDetail != null)
                        {
                            isDealerDetail = true;
                            dealerDetail = dealer.DealerDetail;
                        }
                        if (dealer.Models != null)
                        {
                            rptModels.DataSource = dealer.Models;
                            rptModels.DataBind();
                            
                        }
                    }
                        
                }
            }
            catch
            { 
            
            }
        }
    }
}