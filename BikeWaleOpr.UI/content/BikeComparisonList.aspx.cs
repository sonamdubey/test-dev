using BikewaleOpr.common;
using BikewaleOpr.DALs.Bikedata;
using BikewaleOpr.DALs.PopularComparisions;
using BikewaleOpr.Entities.PopularComparisions;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.PopularComparisions;
using BikeWaleOpr.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.Content
{
    public class BikeComparisonList : Page
    {
        protected DropDownList drpMake1, drpModel1, drpVersion1, drpMake2, drpModel2, drpVersion2;
        protected Button btnSave, btnCancel;
        protected HtmlInputHidden hdn_drpModel1, hdn_drpVersion1, hdn_drpModel2, hdn_drpVersion2;
        protected HtmlInputCheckBox chkIsActive;
        protected string cId = string.Empty;
        public string hostURL = string.Empty;
        protected IEnumerable<PopularBikeComparision> objBikeComps = null;

        private IPopularBikeComparisions _objCompBikesRepo = null;
        private IBikeMakes _objMakesRepo = null;
        private IBikeModelsRepository _objModelsRepo = null;
        private IBikeVersions _objVersionsRepo = null;
        private ushort compareId; private bool isDataSaved;

        /// <summary>
        /// 
        /// </summary>
        public BikeComparisonList()
        {
            using (IUnityContainer container = new UnityContainer())
            {

                container.RegisterType<IPopularBikeComparisions, PopularBikeComparisionsRepository>()
                .RegisterType<IBikeMakes, BikeMakesRepository>()
                .RegisterType<IBikeModelsRepository, BikeModelsRepository>()
                .RegisterType<IBikeVersions, BikeVersionsRepository>();

                _objCompBikesRepo = container.Resolve<IPopularBikeComparisions>();
                _objMakesRepo = container.Resolve<IBikeMakes>();
                _objModelsRepo = container.Resolve<IBikeModelsRepository>();
                _objVersionsRepo = container.Resolve<IBikeVersions>();

            }
        }

        public string SelectedVersion1
        {
            get
            {
                if (Request.Form["drpVersion1"] != null && Request.Form["drpVersion1"].ToString() != "")
                    return Request.Form["drpVersion1"].ToString();
                else
                    return "0";
            }
        }

        public string SelectedModel1
        {
            get
            {
                if (Request.Form["drpModel1"] != null && Request.Form["drpModel1"].ToString() != "")
                    return Request.Form["drpModel1"].ToString();
                else
                    return "0";
            }
        }

        public string SelectedModel2
        {
            get
            {
                if (Request.Form["drpModel1"] != null && Request.Form["drpModel1"].ToString() != "")
                    return Request.Form["drpModel1"].ToString();
                else
                    return "0";
            }
        }
        public string SelectedVersion2
        {
            get
            {
                if (Request.Form["drpVersion2"] != null && Request.Form["drpVersion2"].ToString() != "")
                    return Request.Form["drpVersion2"].ToString();
                else
                    return "0";
            }
        }

        public string ModelContents1
        {
            get
            {
                if (Request.Form["hdn_drpModel1"] != null && Request.Form["hdn_drpModel1"].ToString() != "")
                    return Request.Form["hdn_drpModel1"].ToString();
                else
                    return "";
            }
        }

        public string ModelContents2
        {
            get
            {
                if (Request.Form["hdn_drpModel2"] != null && Request.Form["hdn_drpModel2"].ToString() != "")
                    return Request.Form["hdn_drpModel2"].ToString();
                else
                    return "";
            }
        }

        public string VersionContents1
        {
            get
            {
                if (Request.Form["hdn_drpVersion1"] != null && Request.Form["hdn_drpVersion1"].ToString() != "")
                    return Request.Form["hdn_drpVersion1"].ToString();
                else
                    return "";
            }
        }

        public string VersionContents2
        {
            get
            {
                if (Request.Form["hdn_drpVersion2"] != null && Request.Form["hdn_drpVersion2"].ToString() != "")
                    return Request.Form["hdn_drpVersion2"].ToString();
                else
                    return "";
            }
        }


        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //CommonOpn op = new CommonOpn();
            if (!IsPostBack)
            {
                Ajax.Utility.RegisterTypeForAjax(typeof(AjaxFunctions));
                FillMakes();
                ShowBikeComparision();
                LoadBikeComparision(Request.QueryString["id"]);

            }
            else
            {
                AjaxFunctions aj = new AjaxFunctions();
                aj.UpdateContents(drpModel1, hdn_drpModel1.Value, Request.Form["drpModel1"]);
                aj.UpdateContents(drpVersion1, hdn_drpVersion1.Value, Request.Form["drpVersion1"]);
                aj.UpdateContents(drpModel2, hdn_drpModel2.Value, Request.Form["drpModel2"]);
                aj.UpdateContents(drpVersion2, hdn_drpVersion2.Value, Request.Form["drpVersion2"]);
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 26th Oct 2016 
        /// Description : Fill Bike Makes
        /// </summary>
        void FillMakes()
        {
            try
            {
                if (_objMakesRepo != null)
                {
                    var _objMakes = _objMakesRepo.GetMakes("NEW");
                    drpMake1.DataSource = _objMakes;
                    drpMake1.DataValueField = "MakeId";
                    drpMake1.DataTextField = "MakeName";
                    drpMake1.DataBind();

                    drpMake2.DataSource = _objMakes;
                    drpMake2.DataValueField = "MakeId";
                    drpMake2.DataTextField = "MakeName";
                    drpMake2.DataBind();
                }

            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception

            ListItem item = new ListItem("--Select--", "-1");
            drpMake1.Items.Insert(0, item);
            drpMake2.Items.Insert(0, item);
        }

        /// <summary>
        /// Created By : Sushil Kumar on 26th Oct 2016 
        /// Description : Fill Bikes Data on update/edit
        /// </summary>
        /// <param name="_objComparision"></param>
        private void FillBikeData(PopularBikeComparision _objComparision)
        {
            drpMake1.SelectedValue = _objComparision.MakeId1.ToString();
            if (_objModelsRepo != null)
            {
                var _objModels = _objModelsRepo.GetModels(_objComparision.MakeId1, "NEW");
                drpModel1.DataSource = _objModels;
                drpModel1.DataTextField = "ModelName";
                drpModel1.DataValueField = "ModelId";
                drpModel1.DataBind();
                drpModel1.Items.Insert(0, new ListItem("--Select--", "0"));
                drpModel1.SelectedIndex = drpModel1.Items.IndexOf(drpModel1.Items.FindByValue(_objComparision.ModelId1.ToString()));
                drpModel1.Enabled = true;

                if (_objVersionsRepo != null)
                {
                    var _objVersions = _objVersionsRepo.GetVersions(_objComparision.ModelId1, "NEW");
                    drpVersion1.DataSource = _objVersions;
                    drpVersion1.DataTextField = "VersionName";
                    drpVersion1.DataValueField = "VersionId";
                    drpVersion1.DataBind();
                    drpVersion1.Items.Insert(0, new ListItem("--Select--", "0"));
                    drpVersion1.SelectedIndex = drpVersion1.Items.IndexOf(drpVersion1.Items.FindByValue(_objComparision.VersionId1.ToString()));
                    drpVersion1.Enabled = true;
                }
            }

            drpMake2.SelectedValue = _objComparision.MakeId2.ToString();
            if (_objModelsRepo != null)
            {
                var _objModels = _objModelsRepo.GetModels(_objComparision.MakeId2, "NEW");
                drpModel2.DataSource = _objModels;
                drpModel2.DataTextField = "ModelName";
                drpModel2.DataValueField = "ModelId";
                drpModel2.DataBind();
                drpModel2.Items.Insert(0, new ListItem("--Select--", "0"));
                drpModel2.SelectedIndex = drpModel2.Items.IndexOf(drpModel2.Items.FindByValue(_objComparision.ModelId2.ToString()));
                drpModel2.Enabled = true;

                if (_objVersionsRepo != null)
                {
                    var _objVersions = _objVersionsRepo.GetVersions(_objComparision.ModelId2, "NEW");
                    drpVersion2.DataSource = _objVersions;
                    drpVersion2.DataTextField = "VersionName";
                    drpVersion2.DataValueField = "VersionId";
                    drpVersion2.DataBind();
                    drpVersion2.Items.Insert(0, new ListItem("--Select--", "0"));
                    drpVersion2.SelectedIndex = drpVersion2.Items.IndexOf(drpVersion2.Items.FindByValue(_objComparision.VersionId2.ToString()));
                    drpVersion2.Enabled = true;
                }
            }
        }

        // Shows all the data from the Con_BikeComparisonList table
        /// <summary>
        /// Modified By : Sadhana Upadhyay on 10th Feb 2014
        /// Summary : to get Image HostUrl, ImagePath, Image Name
        /// Modified By : Sadhana Upadhyay on 13th Feb 2014
        /// Summary : Replaced Inline Query with procedure
        /// </summary>
        void ShowBikeComparision()
        {
            try
            {
                if (_objCompBikesRepo != null)
                {
                    objBikeComps = _objCompBikesRepo.GetBikeComparisions();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        // Loads data from the id passed in the URL into the drop down
        /// <summary>
        /// Modified By : Sadhana Upadhyay on 10th Feb 2014
        /// Summary : to get Image HostUrl, ImagePath, Image Name
        /// Modified By : Sadhana Upadhyay on 13th feb 2014
        /// Summary : Rplaced Inline query with procedure
        /// </summary>
        void LoadBikeComparision(string comparisionId)
        {
            try
            {
                if (!string.IsNullOrEmpty(comparisionId))
                {
                    btnSave.Text = "Update";
                    btnCancel.Visible = true;

                    if (objBikeComps != null && objBikeComps.Count() > 0)
                    {

                        var _objComparision = objBikeComps.FirstOrDefault(x => x.ComparisionId == Convert.ToUInt16(comparisionId));
                        if (_objComparision != null)
                        {
                            FillBikeData(_objComparision);
                        }
                        chkIsActive.Checked = _objComparision.IsActive;
                    }
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.ConsumeError();
            }
        }

        // Saves/Updates data in the database
        /// <summary>
        /// Modified By : Sadhana Upadhyay
        /// Summary : To insert/update hostUrl, ImageName, IsActive, IsReplicated, CompId
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object Sender, EventArgs e)
        {


            try
            {
                if (Request.QueryString["id"] != null)
                {
                    compareId = UInt16.Parse(Request.QueryString["id"]);
                }

                if (_objCompBikesRepo != null)
                {


                    isDataSaved = _objCompBikesRepo.SaveBikeComparision(compareId, Convert.ToUInt32(drpVersion1.SelectedItem.Value), Convert.ToUInt32(drpVersion2.SelectedItem.Value), chkIsActive.Checked);
                }

                if (isDataSaved)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Data successfully inserted');", true);
                    if (compareId > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Data successfully updated');", true);
                    }

                    // Removed memcached key which shows data on home page and new page
                    MemCachedUtil.Remove("BW_CompareBikes_Cnt_4");
                    Response.Redirect("/content/bikecomparisonlist.aspx");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Duplicate data not allowed');", true);
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 20th feb 2014
        /// Summary : To cancel Update Operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/content/bikecomparisonlist.aspx");
        }
    }
}
