﻿using Bikewale.BAL.Dealer;
using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.Common;
using Bikewale.Entities.Dealer;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

///
///Created By : Umesh Ojha on 31/8/2012
///
namespace Bikewale.Controls
{
    public class LocateDealer : System.Web.UI.UserControl
    {
        protected DropDownList ddlMake;
        protected HtmlAnchor btnGo;
        private string _selectedMake = String.Empty;

        public string Make
        {
            get { return _selectedMake; }
            set { _selectedMake = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
            btnGo.ServerClick += new EventHandler(btnGo_ServerClick);
        }


        private void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindMake();
        }

        private void BindMake()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealer, Dealer>()
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

                    if (!String.IsNullOrEmpty(_selectedMake))
                        ddlMake.SelectedValue = _selectedMake;

                }
            }
            catch (Exception err)
            {
                Trace.Warn("Exception in GetDealerCitiesList() " + err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private void btnGo_ServerClick(object sender, EventArgs e)
        {
            string redirectUrl = string.Empty;

            if (ddlMake.SelectedValue != "-1")
            {
                string makeVal = string.Empty;
                makeVal = ddlMake.SelectedItem.Value.Split('_')[1];
                //redirectUrl =  "/new/" + UrlRewrite.FormatURL(ddlMake.SelectedItem.Text) + "-dealers/";
                redirectUrl = "/new/" + makeVal + "-dealers/";
                Response.Redirect(redirectUrl);
            }
        }
    }
}