using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Microsoft.Practices.Unity;
using MySql.CoreDAL;
/*******************************************************************************************************
IN THIS CLASS WE GET THE ID OF THE BIKE MAKE FROM THE QUERY STRING, AND FROM IT WE FETCH ALL THE
MODELS FOR THIS MAKE, and the count for this model in the sell inquiry.
*******************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
//using BikeWale.Controls;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.New
{
    /// <summary>
    /// Modified By : Lucky Rathore
    /// Description : Radio button for "ALL" or "New" bikes and its logic Removed.
    /// </summary>
    public class ComparisonChoose : Page
    {
        protected int featuredBikeIndex = 0; // this variable not used any where in this page.
        protected HtmlGenericControl spnError;
        protected DropDownList cmbMake, cmbMake1, cmbMake2, cmbMake3;

        protected Button btnCompare;

        public int make1 = 0, model1 = 0, version1 = 0;
        public int make2 = 0, model2 = 0, version2 = 0;
        public int make3 = 0, model3 = 0, version3 = 0;
        public int make4 = 0, model4 = 0, version4 = 0;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            this.btnCompare.Click += new EventHandler(btnCompare_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            // fill makes in drop-downs
            FillMakes();

            // if Bike ids are passed in query-string, fill the appropriate dropdowns.
            for (int i = 1; i <= 4; i++)
                if (Request["bike" + i] != null && Bikewale.Common.CommonOpn.CheckId(Request["bike" + i]))
                    FillExisting(Request["bike" + i], i);
        } // Page_Load

        /// <summary>
        ///  Modified by : Lucky Rathore on 21 July 2016.
        ///  Description : Remove function's "onlyNew" parameter. 
        /// </summary>
        void FillMakes()
        {
            Bikewale.Common.CommonOpn op = new Bikewale.Common.CommonOpn();

            IEnumerable<BikeMakeEntityBase> makeList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakes<BikeMakeEntity, int>>()
                        .RegisterType<IBikeMakesCacheRepository, BikeMakesCacheRepository>()
                        .RegisterType<ICacheManager, MemcacheManager>();
                    var objMake = container.Resolve<IBikeMakesCacheRepository>();

                    makeList = objMake.GetMakesByType(EnumBikeType.NewBikeSpecification);

                    var _makeList = from mk in makeList select new { Name = mk.MakeName, Value = string.Format("{0}_{1}", mk.MakeId, mk.MaskingName) };

                    cmbMake.DataSource = _makeList;
                    cmbMake1.DataSource = _makeList;
                    cmbMake2.DataSource = _makeList;
                    cmbMake3.DataSource = _makeList;

                    cmbMake.DataTextField = "Name";
                    cmbMake1.DataTextField = "Name";
                    cmbMake2.DataTextField = "Name";
                    cmbMake3.DataTextField = "Name";

                    cmbMake.DataValueField = "Value";
                    cmbMake1.DataValueField = "Value";
                    cmbMake2.DataValueField = "Value";
                    cmbMake3.DataValueField = "Value";

                    cmbMake.DataBind();
                    cmbMake1.DataBind();
                    cmbMake2.DataBind();
                    cmbMake3.DataBind();
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);

            } // catch Exception
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);

            } // catch Exception

            ListItem item = new ListItem("--Select Make--", "0");
            cmbMake.Items.Insert(0, item);
            cmbMake1.Items.Insert(0, item);
            cmbMake2.Items.Insert(0, item);
            cmbMake3.Items.Insert(0, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bike"></param>
        /// <param name="bikeNo"></param>
        void FillExisting(string bike, int bikeNo)
        {
            Trace.Warn("inside fikll existing");

            string sql = @"select ve.id version, ve.bikemodelid model, ve.bikemakeid make, ve.makemaskingname as makemaskingname,ve.modelmaskingname as modelmaskingname 
                 from bikeversions ve
				 where ve.id=par_id";


            Trace.Warn("sql ::: ", sql);

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    //cmd.Parameters.Add("par_id", SqlDbType.BigInt).Value = bike;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.Int32, bike));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {

                            switch (bikeNo)
                            {
                                case 1:
                                    make1 = Convert.ToInt16(dr["Make"].ToString());
                                    model1 = Convert.ToInt16(dr["Model"].ToString());
                                    version1 = Convert.ToInt16(dr["Version"].ToString());

                                    break;
                                case 2:
                                    make2 = Convert.ToInt16(dr["Make"].ToString());
                                    model2 = Convert.ToInt16(dr["Model"].ToString());
                                    version2 = Convert.ToInt16(dr["Version"].ToString());

                                    break;
                                case 3:
                                    make3 = Convert.ToInt16(dr["Make"].ToString());
                                    model3 = Convert.ToInt16(dr["Model"].ToString());
                                    version3 = Convert.ToInt16(dr["Version"].ToString());

                                    break;
                                case 4:
                                    make4 = Convert.ToInt16(dr["Make"].ToString());
                                    model4 = Convert.ToInt16(dr["Model"].ToString());
                                    version4 = Convert.ToInt16(dr["Version"].ToString());

                                    break;
                            }

                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);

            } // catch Exception

        }

        void btnCompare_Click(object sender, EventArgs e)
        {
            Trace.Warn("Comparing bikes...");

            string bike1 = "", bike2 = "", bike3 = "", bike4 = "";
            string compString = "", compareUrl = String.Empty;

            string makeMaskingName1 = String.Empty, modelMaskingName1 = String.Empty;
            string makeMaskingName2 = String.Empty, modelMaskingName2 = String.Empty;
            string makeMaskingName3 = String.Empty, modelMaskingName3 = String.Empty;
            string makeMaskingName4 = String.Empty, modelMaskingName4 = String.Empty;

            if (Request.Form["cmbMake"] != null && Request.Form["cmbMake"] != "0")
                makeMaskingName1 = Request.Form["cmbMake"].Split('_')[1];
            if (Request.Form["cmbMake1"] != null && Request.Form["cmbMake1"] != "0")
                makeMaskingName2 = Request.Form["cmbMake1"].Split('_')[1];
            if (Request.Form["cmbMake2"] != null && Request.Form["cmbMake2"] != "0")
                makeMaskingName3 = Request.Form["cmbMake2"].Split('_')[1];
            if (Request.Form["cmbMake3"] != null && Request.Form["cmbMake3"] != "0")
                makeMaskingName4 = Request.Form["cmbMake3"].Split('_')[1];

            ushort model1 = 0, model2 = 0, model3 = 0, model4 = 0;
            if (Request.Form["cmbModel"] != null && Request.Form["cmbModel"] != "0")
            {
                modelMaskingName1 = Request.Form["cmbModel"].Split('_')[1];
                model1 = Convert.ToUInt16(Request.Form["cmbModel"].Split('_')[0]);
            }
            if (Request.Form["cmbModel1"] != null && Request.Form["cmbModel1"] != "0")
            {
                modelMaskingName2 = Request.Form["cmbModel1"].Split('_')[1];
                model2 = Convert.ToUInt16(Request.Form["cmbModel1"].Split('_')[0]);
            }
            if (Request.Form["cmbModel2"] != null && Request.Form["cmbModel2"] != "0")
            {
                modelMaskingName3 = Request.Form["cmbModel2"].Split('_')[1];
                model3 = Convert.ToUInt16(Request.Form["cmbModel2"].Split('_')[0]);
            }
            if (Request.Form["cmbModel3"] != null && Request.Form["cmbModel3"] != "0")
            {
                modelMaskingName4 = Request.Form["cmbModel3"].Split('_')[1];
                model4 = Convert.ToUInt16(Request.Form["cmbModel3"].Split('_')[0]);
            }

            if (Request.Form["cmbVersion"] != null && Request.Form["cmbVersion"] != "0" && CommonOpn.CheckId(Request.Form["cmbVersion"]))
                bike1 = Request.Form["cmbVersion"];
            if (Request.Form["cmbVersion1"] != null && Request.Form["cmbVersion1"] != "0" && CommonOpn.CheckId(Request.Form["cmbVersion1"]))
                bike2 = Request.Form["cmbVersion1"];
            if (Request.Form["cmbVersion2"] != null && Request.Form["cmbVersion2"] != "0" && CommonOpn.CheckId(Request.Form["cmbVersion2"]))
                bike3 = Request.Form["cmbVersion2"];
            if (Request.Form["cmbVersion3"] != null && Request.Form["cmbVersion3"] != "0" && CommonOpn.CheckId(Request.Form["cmbVersion3"]))
                bike4 = Request.Form["cmbVersion3"];

            int bikeCount = 0;

            if (bike1 != "") bikeCount++;
            if (bike2 != "") bikeCount++;
            if (bike3 != "") bikeCount++;
            if (bike4 != "") bikeCount++;


            Trace.Warn("bikeCount : " + bikeCount.ToString());

            if (bikeCount < 2)
            {
                Response.Redirect("./", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
            for (int i = 2; i <= bikeCount; i++)
            {
                switch (i)
                {
                    case 2:
                        if (bike1 == "")
                        {
                            bike1 = bike2;
                            bike2 = bike3;
                            bike3 = bike4;

                            makeMaskingName1 = makeMaskingName2;
                            makeMaskingName2 = makeMaskingName3;
                            makeMaskingName3 = makeMaskingName4;

                            modelMaskingName1 = modelMaskingName2;
                            modelMaskingName2 = modelMaskingName3;
                            modelMaskingName3 = modelMaskingName4;
                        }
                        else if (bike2 == "")
                        {
                            bike2 = bike3;
                            bike3 = bike4;

                            makeMaskingName2 = makeMaskingName3;
                            makeMaskingName3 = makeMaskingName4;

                            modelMaskingName2 = modelMaskingName3;
                            modelMaskingName3 = modelMaskingName4;
                        }

                        if (bike1 == "")
                        {
                            bike1 = bike2;
                            bike2 = bike3;

                            makeMaskingName1 = makeMaskingName2;
                            makeMaskingName2 = makeMaskingName3;

                            modelMaskingName1 = modelMaskingName2;
                            modelMaskingName2 = modelMaskingName3;
                        }
                        else if (bike2 == "")
                        {
                            bike2 = bike3;

                            makeMaskingName2 = makeMaskingName3;

                            modelMaskingName2 = modelMaskingName3;
                        }

                        if (bike2 == "")
                        {
                            bike2 = bike3;

                            makeMaskingName2 = makeMaskingName3;
                            modelMaskingName2 = modelMaskingName3;
                        }

                        compString = "bike1=" + bike1 + "&bike2=" + bike2;
                        compareUrl = makeMaskingName1 + "-" + modelMaskingName1 + "-vs-" + makeMaskingName2 + "-" + modelMaskingName2;
                        break;
                    case 3:
                        if (bike1 == "")
                        {
                            bike1 = bike2;
                            bike2 = bike3;
                            bike3 = bike4;

                            makeMaskingName1 = makeMaskingName2;
                            makeMaskingName2 = makeMaskingName3;
                            makeMaskingName3 = makeMaskingName4;

                            modelMaskingName1 = modelMaskingName2;
                            modelMaskingName2 = modelMaskingName3;
                            modelMaskingName3 = modelMaskingName4;
                        }
                        else if (bike2 == "")
                        {
                            bike2 = bike3;
                            bike3 = bike4;

                            makeMaskingName2 = makeMaskingName3;
                            makeMaskingName3 = makeMaskingName4;

                            modelMaskingName2 = modelMaskingName3;
                            modelMaskingName3 = modelMaskingName4;
                        }
                        else if (bike3 == "")
                        {
                            bike3 = bike4;
                            makeMaskingName3 = makeMaskingName4;
                            modelMaskingName3 = modelMaskingName4;
                        }
                        compString = "bike1=" + bike1 + "&bike2=" + bike2 + "&bike3=" + bike3;
                        compareUrl = makeMaskingName1 + "-" + modelMaskingName1 + "-vs-" + makeMaskingName2 + "-" + modelMaskingName2 + "-vs-" + makeMaskingName3 + "-" + modelMaskingName3;
                        break;
                    case 4:
                        compString = "bike1=" + bike1 + "&bike2=" + bike2 + "&bike3=" + bike3 + "&bike4=" + bike4;
                        compareUrl = makeMaskingName1 + "-" + modelMaskingName1 + "-vs-" + makeMaskingName2 + "-" + modelMaskingName2 + "-vs-" + makeMaskingName3 + "-" + modelMaskingName3 + "-vs-" + makeMaskingName4 + "-" + modelMaskingName4;
                        break;
                }
            }

            #region

            // Compare all the bikes and order them in an ascending order

            List<CompareMakeModelEntity> bikeList = new List<CompareMakeModelEntity>();
            bikeList.Add(new CompareMakeModelEntity() { Id = 1, MakeMaskingName = makeMaskingName1, ModelMaskingName = modelMaskingName1, ModelId = model1 });
            bikeList.Add(new CompareMakeModelEntity() { Id = 2, MakeMaskingName = makeMaskingName2, ModelMaskingName = modelMaskingName2, ModelId = model2 });
            bikeList.Add(new CompareMakeModelEntity() { Id = 3, MakeMaskingName = makeMaskingName3, ModelMaskingName = modelMaskingName3, ModelId = model3 });
            bikeList.Add(new CompareMakeModelEntity() { Id = 4, MakeMaskingName = makeMaskingName4, ModelMaskingName = modelMaskingName4, ModelId = model4 });
            compareUrl = CreateCompareUrl(bikeList);

            #endregion

            string returnUrl = string.Format("/comparebikes/{0}/?{1}&source={2}", compareUrl, compString, (int)Bikewale.Entities.Compare.CompareSources.Desktop_CompareBike_UserSelection);
            Response.Redirect(returnUrl);
        } // btnSend_Click

        /// <summary>
        /// Creates the compare URL.
        /// </summary>
        /// <param name="bikeList">The bike list.</param>
        /// <returns>
        /// Created by : Sangram Nandkhile on 21-Apr-2017 
        /// </returns>
        private string CreateCompareUrl(List<CompareMakeModelEntity> bikeList)
        {
            string url = string.Empty;
            try
            {
                url = string.Join("-vs-", bikeList.Where(x => x.ModelId != 0).OrderBy(x => x.ModelId).Select(x => string.Format("{0}-{1}", x.MakeMaskingName, x.ModelMaskingName)));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ComparisonChoose.CreateCompareUrl()");
            }
            return url;
        }
    } // class
} // namespace