﻿using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Common;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.Unity;
using Bikewale.Interfaces.Dealer;
using Bikewale.BAL.Dealer;
using System.Collections.Generic;
using Bikewale.Entities.Dealer;
using System.Linq;


namespace Bikewale.New
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 10/8/2012
    ///     Class to search the Bike dealers
    /// </summary>
    public class LocateNewBikeDealers : Page
    {
        protected DataList dlShowMakes;
        protected DropDownList cmbCity, cmbMake;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //code for device detection added by Ashwini Todkar
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();

            if (!IsPostBack)
            {
                FillMakes();
                BindControl();
            }
        }

        private void FillMakes()
        {
         try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealer, Dealer>();

                    IDealer objDealer = container.Resolve<IDealer>();

                    IList<NewBikeDealersMakeEntity> objMakes = objDealer.GetDealersMakesList();

                    var makesList = objMakes.Select(s => new { Text = s.MakeName, Value = s.MakeId + "_" + s.MaskingName });

                    cmbMake.DataSource = makesList;
                    cmbMake.DataTextField = "Text";
                    cmbMake.DataValueField = "Value";
                    cmbMake.DataBind();
                    cmbMake.Items.Insert(0, (new ListItem("--Select Make--", "0")));

                }
            }
            catch (Exception err)
            {
                Trace.Warn("Exception in GetDealerCitiesList() " + err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        private void BindControl()
        {
            string sql = "";

            sql = " SELECT DNC.MakeId AS MakeId, CM.Name AS BikeMake,CM.MaskingName, COUNT(DNC.Id) AS TotalCount "
                + " FROM Dealer_NewBike AS DNC, BikeMakes AS CM With(NoLock) "
                + " WHERE DNC.MakeId = CM.Id AND DNC.IsActive = 1 AND CM.IsDeleted = 0 AND CM.New = 1"
                + " AND DNC.CityId IN (SELECT Id FROM Cities With(NoLock) WHERE IsDeleted = 0)"
                + " GROUP By DNC.MakeId, CM.Name ,CM.MaskingName ORDER BY BikeMake ";

            Trace.Warn(sql);
            
            CommonOpn op = new CommonOpn();

            try
            {
                op.BindListReader(sql, dlShowMakes);
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }
}