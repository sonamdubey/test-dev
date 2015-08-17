using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;

namespace Bikewale.Controls
{
    //  Created By : Ashish G. Kamble on 8/8/2012
    public class UpcomingBikes : UserControl
    {
        private int _topCount = 5;
        private string _makeId = "-1", _make = "";
        private bool _verticalDisplay = true, _loadStatic = false;
        protected Repeater rptData;
        protected HtmlGenericControl divUpcomingBike;
        protected Label lblNotFound;

        public int TopCount
        {
            get { return _topCount; }
            set { _topCount = value; }
        }

        public string MakeId
        {
            get { return _makeId; }
            set { _makeId = value; }
        }

        public string Make
        {
            get { return _make; }
            set { _make = value; }
        }

        public bool VerticalDisplay
        {
            get { return _verticalDisplay; }
            set { _verticalDisplay = value; }
        }

        public bool LoadStatic
        {
            get { return _loadStatic; }
            set { _loadStatic = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        void Page_Load(object sender, EventArgs e)
        {
            if (VerticalDisplay == false) divUpcomingBike.Attributes.Add("class", "uc-hor");

            if (!IsPostBack)
            {
                GetDetails();
            }
        } // Page_Load

        void GetDetails()
        {
            string sql = "";

            sql = " SELECT Top 3 MK.Name MakeName,MK.MaskingName AS MakeMaskingName, Mo.Name AS ModelName,Mo.MaskingName AS ModelMaskingName,  ECL.ExpectedLaunch, ECL.PhotoName, EstimatedPriceMin, EstimatedPriceMax, Mo.HostUrl, Mo.SmallPic, CS.SmallDescription as Description, Mo.OriginalImagePath"
                + " FROM ExpectedBikeLaunches ECL With(NoLock) "
                + " INNER JOIN BikeModels Mo With(NoLock) ON ECL.BikeModelId = Mo.ID "
                + " INNER JOIN BikeMakes MK With(NoLock) ON MK.ID = Mo.BikeMakeId "
                + " LEFT JOIN BikeSynopsis CS With(NoLock) ON CS.ModelId = Mo.BikeMakeId"

                + " WHERE Mo.Futuristic = 1 and ECL.isLaunched = 0 AND ECL.IsDeleted = 0 ";

            if (MakeId != "-1" && MakeId.Trim() != "")
            {
                sql += " AND Mo.BikeMakeId = " + MakeId;
            }

            sql += " ORDER BY ECL.LaunchDate";

            Trace.Warn(sql);

            CommonOpn op = new CommonOpn();
            try
            {
                op.BindRepeaterReader(sql, rptData);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            if (rptData.Items.Count <= 0)
            {
                lblNotFound.Visible = true;
                lblNotFound.Text = "There is no new bike expected from " + Make + " in near future";
            }
        }
    }   // End of class
}   // End of namespace