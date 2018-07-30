using Bikewale.BAL.Dealer;
using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.Common;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.Dealer;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;


namespace Bikewale.Mobile.Controls
{
    public class LocateDealer : System.Web.UI.UserControl
    {
        protected DropDownList ddlMake;
        protected string ddlCity_Id = String.Empty, ddlMake_Id = string.Empty, linkBtnId;
        private string _headerText = "Locate Dealers";

        public string HeaderText
        {
            get { return _headerText; }
            set { _headerText = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlMake_Id = ddlMake.ClientID.ToString();
                GetDealerMakesList();
            }
        }

        private void GetDealerMakesList()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealer, Dealer>()
                        .RegisterType<IDealerRepository, DealersRepository>()
                        .RegisterType<IDealerCacheRepository, DealerCacheRepository>()
                        .RegisterType<ICacheManager, MemcacheManager>();

                    IDealerCacheRepository objDealer = container.Resolve<IDealerCacheRepository>();

                    IEnumerable<NewBikeDealersMakeEntity> objMakes = objDealer.GetDealersMakesList();

                    var makesList = objMakes.Select(s => new { Text = s.MakeName, Value = s.MakeId + "_" + s.MaskingName });

                    ddlMake.DataSource = makesList;
                    ddlMake.DataTextField = "Text";
                    ddlMake.DataValueField = "Value";
                    ddlMake.DataBind();
                    ddlMake.Items.Insert(0, (new ListItem("--Select Make--", "0")));

                }
            }
            catch (Exception err)
            {
                Trace.Warn("Exception in GetDealerCitiesList() " + err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
        }
    }
}