using Bikewale.Common;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    public class UserReviewsMin : System.Web.UI.UserControl
    {
        protected Repeater rptReviews;
        protected HtmlGenericControl divControl;
        private string makeId = "", modelId = "", seriesId = string.Empty;
        private int recordCount = 0;
        private string _topCount;

        public string MakeId
        {
            get { return makeId; }
            set { makeId = value; }
        }

        public string ModelId
        {
            get { return modelId; }
            set { modelId = value; }
        }

        public string TopRecords
        {
            get { return _topCount; }
            set { _topCount = value; }
        }

        public int RecordCount
        {
            get { return recordCount; }
            set { recordCount = value; }
        }

        public string SeriesId
        {
            get { return seriesId; }
            set { seriesId = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(makeId) || !String.IsNullOrEmpty(modelId) || !String.IsNullOrEmpty(seriesId))
                FetchReviews();
        }
        private void FetchReviews()
        {
            throw new Exception("Method not used/commented");

            //SqlCommand cmd = new SqlCommand("GetCustomerReviewMin");
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("@TopCount", SqlDbType.SmallInt).Value = TopRecords;
            //if (MakeId != "")
            //    cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = MakeId;
            //if (ModelId != "")
            //    cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = ModelId;
            //if (!String.IsNullOrEmpty(SeriesId))
            //    cmd.Parameters.Add("@SeriesId", SqlDbType.Int).Value = SeriesId;

            //Database db = new Database();
            //SqlDataReader dr = null;
            //try
            //{
            //    dr = db.SelectQry(cmd);
            //    if (dr.HasRows)
            //    {
            //        divControl.Attributes.Remove("class");
            //        rptReviews.DataSource = dr;
            //        rptReviews.DataBind();
            //    }
            //    else
            //        divControl.Attributes.Add("class", "hide");
                
            //    RecordCount = rptReviews.Items.Count;
            //    Trace.Warn("RecordCountUserReview: " + RecordCount.ToString());
            //}
            //catch (Exception ex)
            //{
            //    Trace.Warn(ex.Message);
            //}
            //finally
            //{
            //    if(dr != null)
            //        dr.Close();
            //    db.CloseConnection();
            //}
        }

        //public string GetRateImage(double value)
        //{
        //    string oneImg = "<img src='" + ImagingFunctions.GetRootImagePath() + "/images/ratings/1.gif'>";
        //    string zeroImg = "<img src='" +ImagingFunctions.GetRootImagePath() + "/images/ratings/0.gif'>";
        //    string halfImg = "<img src='" + ImagingFunctions.GetRootImagePath() + "/images/ratings/half.gif'>";

        //    StringBuilder sb = new StringBuilder();
        //    int absVal = (int)Math.Floor(value);

        //    int i;
        //    for (i = 0; i < absVal; i++)
        //        sb.Append(oneImg);

        //    if (value > absVal)
        //        sb.Append(halfImg);
        //    else
        //        i--;

        //    for (int j = 5; j > i + 1; j--)
        //        sb.Append(zeroImg);

        //    return sb.ToString();
        //}

        protected string GetAuthorName(string _customerName, string _handleName)
        {
            if (_handleName == "")
                return _customerName;
            else
                return "<a href=\"/community/members/" + _handleName + ".html\" style=\"margin:0px;padding:0px;color:#999999;text-decoration:underline;background-image: none !important;\">" + _handleName + "</a>";
        }

        public string RemoveHtmlTags(string text)
        {
            String strOutput = Regex.Replace(text, "<(.|\n)+?>", string.Empty);
            return strOutput;
        }

        protected string GetModifiedComment(string text)
        {
            if (text.Length < 185)
            {
                return text;
            }
            else
            {
                return text.Substring(0, text.Substring(0, 185).LastIndexOf(" ")) + " ...";
            }
        }

    }
}