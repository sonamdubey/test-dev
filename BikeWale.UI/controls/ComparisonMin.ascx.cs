using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using Bikewale.New;
using System.Configuration;

namespace Bikewale.Controls
{
    public class ComparisonMin : System.Web.UI.UserControl
    {
        protected Repeater rptComparison;        
        
        DataSet ds = new DataSet();
        protected string bike1 = string.Empty, bike2 = string.Empty, imageUrl = string.Empty, versionId1 = string.Empty, versionId2 = string.Empty, price1 = string.Empty, price2 = string.Empty, reviewCount1 = string.Empty, reviewCount2 = string.Empty,
            makeMaskingName1 = string.Empty, makeMaskingName2 = string.Empty, modelMaskingName1 = string.Empty, modelMaskingName2 = string.Empty, modelId1 = string.Empty, modelId2 = string.Empty;
        protected double review1, review2;
        protected HtmlGenericControl compButton;

        private string _topRecords = "4";
        public string TopRecords
        {
            get { return _topRecords; }
            set { _topRecords = value; }
        }

        private bool _showCompButton = true;
        public bool ShowCompButton
        {
            get { return _showCompButton; }
            set { _showCompButton = value; }
        }
        
        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }
        private void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindComparison();
                ShowButton();
            }
        }//pageload

        /// <summary>
        /// Modified By : Sadhana Upadhyay on 24th Feb 2014
        /// Summary : To get Bike Comparison list
		/// Modified By : Sadhana Upadhyay on 24 Sept 2014
		/// Summary : to get date comparebike.cs
        /// </summary>
        private void BindComparison()
        {
            //SqlCommand cmd = new SqlCommand("GetBikeComparisonMin");
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("@TopCount", SqlDbType.SmallInt).Value = TopRecords;
            //cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = ConfigurationManager.AppSettings["DefaultCity"];

            //Database db = new Database();
            try
            {
                CompareBikes CB = new CompareBikes();
                ds = CB.GetComparisonBikeList(Convert.ToUInt16(TopRecords));
                // Get first row
                DataRow objRow = ds.Tables[0].Rows[0];

                GetCompareBikeData(objRow);
                
                ds.Tables[0].Rows[0].Delete();
                ds.Tables[0].AcceptChanges();

                if (ds != null)
                {
                    rptComparison.DataSource = ds;
                    rptComparison.DataBind();
                }
            }
            catch (SqlException exSql)
            {
                ErrorClass objErr = new ErrorClass(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   //End of BindComparison

        /// <summary>
        /// Created By : Sadhana Upadhyay on 24th Feb 2014
        /// Summary to get 1st record of BikeComparison
        /// </summary>
        /// <param name="ds"></param>
        void GetCompareBikeData(DataRow objRow)
        {
            bike1 = objRow["Bike1"].ToString();
            bike2 = objRow["Bike2"].ToString();
            imageUrl = ImagingOperations.GetPathToShowImages(objRow["ImagePath"].ToString() + objRow["ImageName"].ToString(), objRow["HostURL"].ToString());
            versionId1 = objRow["VersionId1"].ToString();
            versionId2 = objRow["VersionId2"].ToString();
            price1 = objRow["Price1"].ToString();
            price2 = objRow["Price2"].ToString();
            review1 = Convert.ToDouble(objRow["Review1"]);
            review2 = Convert.ToDouble(objRow["Review2"]);
            reviewCount1 = objRow["ReviewCount1"].ToString();
            reviewCount2 = objRow["ReviewCount2"].ToString();
            makeMaskingName1 = objRow["MakeMaskingName1"].ToString();
            makeMaskingName2 = objRow["MakeMakingName2"].ToString();
            modelMaskingName1 = objRow["ModelMaskingName1"].ToString();
            modelMaskingName2 = objRow["ModelMaskingName2"].ToString();
            modelId1 = objRow["ModelId1"].ToString();
            modelId2 = objRow["ModelId2"].ToString();
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 24th Feb 2014
        /// Summary : to hide Compare Button
        /// </summary>
        void ShowButton() 
        {
            if (ShowCompButton == false)
            {
                compButton.Visible = false;
            }
        }
    }//class
}//namespace