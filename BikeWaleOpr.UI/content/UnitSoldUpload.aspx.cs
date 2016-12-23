using BikewaleOpr.DALs.Bikedata;
using BikewaleOpr.DALs.PopularComparisions;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.PopularComparisions;
using BikeWaleOpr.Common;
using Microsoft.Practices.Unity;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BikewaleOpr.content
{
    /// <summary>
    /// Created By : Sajal Gupta on 22-12-2016
    /// Desc : Class to hold functions for uploading xl file for model sold unit.
    /// </summary>
    /// <param name="data"></param>
    public class UnitSoldUpload : System.Web.UI.Page
    {
        protected Button btnUploadFile;
        protected HtmlInputFile flUpload;
        private IBikeModels _objModelsRepo = null;
        protected BikeWaleOpr.Controls.DateControl calFrom;
        protected HtmlGenericControl spnFile;

        protected override void OnInit(EventArgs e)
        {
            btnUploadFile.Click += new EventHandler(btnUploadFile_Click);
        }

        public UnitSoldUpload()
        {
            using (IUnityContainer container = new UnityContainer())
            {

                container.RegisterType<IPopularBikeComparisions, PopularBikeComparisionsRepository>()
                .RegisterType<IBikeMakes, BikeMakesRepository>()
                .RegisterType<IBikeModels, BikeModelsRepository>()
                .RegisterType<IBikeVersions, BikeVersionsRepository>();

                _objModelsRepo = container.Resolve<IBikeModels>();

            }
        }

        /// <summary>
        /// Created By : Sajal Gupta on 22-12-2016
        /// Desc : Method to be called on button click
        /// </summary>
        /// <param name="data"></param>
        void btnUploadFile_Click(object sender, EventArgs e)
        {
            Label lbl = FindControl("lblMessage") as Label;
            HtmlGenericControl spnFile = FindControl("spnFile") as HtmlGenericControl;

            if (UploadFile() == true)
            {
                spnFile.InnerHtml = "";

                var data = GetDataFromExcel(Server.MapPath("~/content/modelsoldunitdata/ModelUnitSold.xlsx"));

                if (data != null)
                {
                    ProcessData(data);
                    lbl.Visible = true;
                }
            }
            else
            {

                spnFile.InnerHtml = "Please update file";

                lbl.Visible = false;
            }
        }

        /// <summary>
        /// Created By : Sajal Gupta on 22-12-2016
        /// Desc : Method to save date, modelid, number of units sold in db;
        /// </summary>
        /// <param name="data"></param>
        private void ProcessData(DataTable data)
        {
            try
            {

                int rowCount = data.Rows.Count, a = 0;
                string modelId, unitSold;
                StringBuilder modelUnitsSoldList = new StringBuilder();

                for (int i = 0; i < rowCount; i++)
                {
                    a++;
                    modelId = data.Rows[i]["model id"].ToString();
                    unitSold = data.Rows[i]["units sold"].ToString();

                    if (a < 50)
                    {
                        modelUnitsSoldList = modelUnitsSoldList.AppendFormat("{0}:{1},", modelId, unitSold);
                    }
                    else
                    {
                        a = 0;
                        modelUnitsSoldList = modelUnitsSoldList.AppendFormat("{0}:{1},", modelId, unitSold);
                        _objModelsRepo.SaveModelUnitSold(modelUnitsSoldList.ToString(), calFrom.Value);
                        modelUnitsSoldList.Clear();
                    }

                }

                if (modelUnitsSoldList.Length != 0)
                {
                    _objModelsRepo.SaveModelUnitSold(modelUnitsSoldList.ToString(), calFrom.Value);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "UnitSoldUpload.ProcessData()");
                objErr.SendMail();

                HtmlGenericControl spnFile = FindControl("spnFile") as HtmlGenericControl;
                spnFile.InnerHtml = "Please update file in correct format";

                Label lbl = FindControl("lblMessage") as Label;
                lbl.Visible = false;

            } // catch Exception


        }

        /// <summary>
        /// Created By : Sajal Gupta on 22-12-2016
        /// Desc : Method to upload file on server
        /// </summary>
        /// <param name="data"></param>
        bool UploadFile()
        {
            bool uploaded = false;
            //temp image path
            string filePath = Server.MapPath("~/content/modelsoldunitdata");

            if (!Directory.Exists(Request.ServerVariables["APPL_PHYSICAL_PATH"] + @"\content\modelsoldunitdata\"))
                Directory.CreateDirectory(Request.ServerVariables["APPL_PHYSICAL_PATH"] + @"\content\modelsoldunitdata\");

            try
            {
                if (flUpload.PostedFile.FileName != "")
                {
                    string fileName = "ModelUnitSold.xlsx";
                    flUpload.PostedFile.SaveAs(filePath + "/" + fileName);

                    uploaded = true;
                }
                else
                    uploaded = false;
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "UnitSoldUpload.UploadFile()");
                objErr.SendMail();
                uploaded = false;
            } // catch Exception

            return uploaded;
        }

        /// <summary>
        /// Created By : Sajal Gupta on 22-12-2016
        /// Desc : Method to parse file data
        /// </summary>
        /// <param name="data"></param>
        private DataTable GetDataFromExcel(string file)
        {
            try
            {
                OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + file + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";");
                OleDbDataAdapter adapter = new OleDbDataAdapter("select * from [modelunitsold$]", conn);
                DataSet dset = new DataSet();
                adapter.Fill(dset);
                var data = dset.Tables[0];
                return data;
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "UnitSoldUpload.GetDataFromExcel()");
                objErr.SendMail();

                HtmlGenericControl spnFile = FindControl("spnFile") as HtmlGenericControl;
                spnFile.InnerHtml = "Please update file in correct format";

                Label lbl = FindControl("lblMessage") as Label;
                lbl.Visible = false;

                return null;
            }
        }
    }
}