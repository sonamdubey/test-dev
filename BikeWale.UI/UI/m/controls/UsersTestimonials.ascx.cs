using Bikewale.BindViewModels.Controls;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Controls
{
    public partial class UsersTestimonials : System.Web.UI.UserControl
    {
        public int FetchedCount { get; private set; }
        public uint TopCount { get; set; }
        protected Repeater rptTestimonial;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindControls();
        }

        private void BindControls()
        {
            try
            {
                BindUsersTestimonialControl objBindUsersTestimonialControl = new BindUsersTestimonialControl();
                objBindUsersTestimonialControl.TopCount = TopCount;
                objBindUsersTestimonialControl.BindRepeater(rptTestimonial);
                FetchedCount = objBindUsersTestimonialControl.FetchedCount;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "UsersTestimonials.BindControls");
                
            }
        }
    }
}