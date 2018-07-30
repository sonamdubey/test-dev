using Bikewale.Common;
using Bikewale.Entities.BikeData;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Used
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 9/8/2012
    /// </summary>
    public class Default20102016 : Page
    {
        protected DropDownList ddlMakeModel, ddlCity, ddlMake;
        protected string customerId;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        protected void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            customerId = CurrentUser.Id;
            if (!IsPostBack)
            {
                FillCity();
                FillMake();
                //FillMakeModel();                
            }
        }

        /// <summary>
        ///     Written By : Ashish G. Kamble on 29/8/2012
        ///     PopulateWhere to fill cities dropdownlist
        /// </summary>
        protected void FillCity()
        {
            StateCity objCity = new StateCity();
            DataTable dt = objCity.GetCitiesWithMappingName("USED");

            if (dt != null)
            {
                ddlCity.DataSource = dt;
                ddlCity.DataTextField = "Text";
                ddlCity.DataValueField = "Value";
                ddlCity.DataBind();

                //also prepend the list with the values of top cites
                ddlCity.Items.Insert(0, new ListItem("------------", "-1"));
                ddlCity.Items.Insert(0, new ListItem("Hyderabad", "105_hyderabad"));
                ddlCity.Items.Insert(0, new ListItem("Pune", "12_pune"));
                ddlCity.Items.Insert(0, new ListItem("Ahmedabad", "128_ahmedabad"));
                ddlCity.Items.Insert(0, new ListItem("Kolkata", "198_kolkata"));
                ddlCity.Items.Insert(0, new ListItem("Chennai", "176_chennai"));
                ddlCity.Items.Insert(0, new ListItem("Bangalore", "2_bangalore"));
                ddlCity.Items.Insert(0, new ListItem("New Delhi", "10_newdelhi"));
                ddlCity.Items.Insert(0, new ListItem("Mumbai", "1_mumbai"));
                ddlCity.Items.Insert(0, new ListItem("------------", "-1"));

                ddlCity.Items.Insert(0, new ListItem("All cities", "0"));

                //ListItem i = ddlCity.Items.FindByValue("-1");
                //i.Attributes.Add("disabled", "true");
            }
        }   // End of fill city function 

        /// <summary>
        ///     Written By : Ashwini Todkar on 31/oct/2013
        ///     PopulateWhere to fill make dropdownlist
        /// </summary>
        private void FillMake()
        {
            string sql = string.Empty;

            try
            {
                MakeModelVersion obj = new MakeModelVersion();
                //dt = obj.GetMakes("Used");

                //if (dt.Rows.Count > 0)
                //{
                //    ddlMake.DataSource = dt;
                //    ddlMake.DataTextField = "Text";
                //    ddlMake.DataValueField = "value";
                //    ddlMake.DataBind();

                //    ddlMake.Items.Insert(0, new ListItem("--Select Make--", "0")); 
                //}

                obj.GetMakes(EnumBikeType.UserReviews, ref ddlMake);

            }
            catch (SqlException ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
            }
        }



        /// <summary>
        ///     Written By : Ashish G. Kamble on 29/8/2012
        ///     Function to fill MakeModel dropdown list
        /// </summary>
        //private void FillMakeModel()
        //{
        //    string sql = string.Empty;

        //    CommonOpn op = new CommonOpn();

        //    try
        //    {

        //        sql = " SELECT Convert(Varchar,MA.ID) + '.' + Convert(Varchar,MO.ID) AS ModelId, MA.Name + ' ' + MO.Name as makemodel "
        //            + " FROM BikeMakes MA, BikeModels MO WHERE "
        //            + " MA.ID = MO.BikeMakeId AND MA.IsDeleted = 0 AND "
        //            + " MO.IsDeleted = 0 AND MO.ID IN (Select BikeModelId From BikeVersions Where IsDeleted = 0) ORDER BY MakeModel";

        //        op.FillDropDown(sql, ddlMakeModel, "makemodel", "ModelId");

        //        ddlMakeModel.Items.Insert(0, new ListItem("-- Any Bike --", "0.0"));
        //    }
        //    catch (SqlException err)
        //    {
        //        Trace.Warn(err.Message);
        //        ErrorClass.LogError(err, Request.ServerVariables["URL"]);
        //        
        //    }
        //    catch (Exception err)
        //    {
        //        Trace.Warn(err.Message);
        //        ErrorClass.LogError(err, Request.ServerVariables["URL"]);
        //        
        //    }
        //}   // End of FillMakeModel function

    }   // End of class
}   // End of namespace