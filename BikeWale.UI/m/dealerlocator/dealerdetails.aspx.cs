using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Dealerlocator
{
    public class Dealerdetails : System.Web.UI.Page
    {
        protected Repeater rptModels;
        protected UInt16 dealerId;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (processQueryString())
            {
                Bikewale.Interfaces.DealerLocator.IDealer dealers = new Bikewale.DAL.DealerLocator.DealerRepository();
                DealerBikesEntity dealerDeatail = dealers.GetDealerBikes(dealerId);
                if(dealerDeatail != null && dealerDeatail.Models != null)
                {
                    BindModels(dealerDeatail.Models); 
                }

            }
        }

        private bool processQueryString()
        {
            dealerId = 4;
            return true;
        }

        private void BindModels(IEnumerable<MostPopularBikesBase> models)
        {
            rptModels.DataSource = models;
            rptModels.DataBind();
        }


    }
}