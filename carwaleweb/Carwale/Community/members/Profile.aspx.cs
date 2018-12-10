using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Controls;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Carwale.Notifications;
using Carwale.UI.Common;
using Carwale.DAL.CoreDAL;
using Carwale.Utility;
using Carwale.DAL.CoreDAL.MySql;
using System.Data.Common;
using Carwale.DAL.Forums;

namespace Carwale.UI.Community
{
    public class MemberProfile : Page
    {
        //Tested ok for Parameterized Queries		
        protected HtmlGenericControl divThumbNailPhoto, divAboutMe, divEditProfile;
        protected HtmlGenericControl divPhotoApproval, divAvtar, divAvtarApproval
                , divCarsOwned, divMessages, divSendMessage, divForums;
        protected HtmlTable tblPointsSummary, tblPointsDetailed;
        protected CarwaleReviews urUserReviewsMostRead;        
        protected Repeater rptPosts;
        protected HtmlImage imgReal;

        public string customerId = "", customerName = "", customerCity = "", customerState = "", communityId = "-1";
        public int total = 0, reviews = 0, reviewsTotal = 0, answers = 0, answersTotal = 0,
            forums = 0, forumsTotal = 0, photos = 0, photosTotal = 0,
            pointsReview = 0, pointsAnswer = 0, pointsForum = 0, pointsPhotos = 0;

        string aboutMe = "";
        string thumbNail = "";
        public string realImage = "", avtar = "", HostURL = "", realOriginalImgPath = "", avtOriginalImgPath="";

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            try
            {
                if (Request["handle"] != null && Request["handle"] != "")
                {
                    //CommonMessage cm = new CommonMessage();
                    //customerId = cm.GetHandleId(Request["handle"].ToString());
                    ForumsModeratorDAL forumDal = new ForumsModeratorDAL();
                    DataSet ds = new DataSet();
                    ds = forumDal.GetCustomerDetailsByHandleName(Request["handle"].ToString());
                    if (ds != null && ds.Tables.Count > 0)
                        customerId = ds.Tables[0].Rows[0]["CustomerId"].ToString();
                }
                else
                {
                    customerId = CarwaleSecurity.DecryptUserId(long.Parse(Request["uId"]));
                }

               // uaRecentAlbums.UserId = long.Parse(customerId);

                Trace.Warn("customerId:" + customerId);
                // umcomment the following code when planning to start private message.
                /*
                if ( CurrentUser.Id == customerId )
                {
                    divMessages.Visible = true;
                    divSendMessage.Visible = false;
                }
                */
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                Response.Redirect("/MyCarWale/Forums/");
            }

            if (!IsPostBack)
            {
                urUserReviewsMostRead.ReviewerId = customerId;

                GetUserDetails(customerId);
              //  GetPhotos();
               // GetUserCars();
                GetForumPosts();

                // show about me
                if (aboutMe.Length > 0)
                    divAboutMe.InnerHtml = "<b>About me:</b> " + aboutMe.Replace("\n", "<br>");

                if (CurrentUser.Id != "-1" && CurrentUser.Id == customerId)
                {
                    // show edit profile link
                    divEditProfile.Visible = true;
                }
            }
        } // Page_Load

        void GetUserDetails(string customerId)
        {
            //Database db = new Database();
           // SqlDataReader dr = null;

          //  string sql;
            try
            {
                //get the list of subcategories
                //sql = " SELECT Cu.Name Customer, Ci.Name City, St.Name State, RPD.*, "
                //    + " (SELECT COUNT(ID) FROM CustomerReviews WITH(NOLOCK) WHERE IsActive=1 AND CustomerId= @CustomerId) reviews, "
                //    + " (SELECT COUNT(ID) FROM CAAnswers WITH(NOLOCK) WHERE IsActive=1 AND CustomerId= @CustomerId) answers, "
                //    + " (SELECT IsNull(SUM(Photos),0) FROM UP_Albums WITH(NOLOCK) WHERE isActive=1 AND UserId= @CustomerId) photos, "
                //    + " (SELECT COUNT(FT.ID) FROM ForumThreads FT WITH(NOLOCK), Forums F WITH(NOLOCK) "
                //    + " WHERE F.Id=FT.ForumId AND FT.IsActive=1 AND F.IsActive=1 AND FT.CustomerId= @CustomerId AND FT.IsModerated = 1 ) forums "
                //    + " FROM ((( Customers Cu WITH(NOLOCK) LEFT JOIN Cities Ci WITH(NOLOCK) ON Cu.CityId=Ci.ID ) "
                //    + " LEFT JOIN States St WITH(NOLOCK) ON Cu.StateId=St.Id ) "
                //    + " LEFT JOIN RewardPointsDistribution RPD WITH(NOLOCK) ON 1=1 ) WHERE Cu.Id = @CustomerId ";

                //Trace.Warn(sql);
                //passing parameters
               // SqlParameter[] param = { new SqlParameter("@CustomerId", customerId) };

                //dr = db.SelectQry(sql, param);
                using (DbCommand cmd = DbFactory.GetDBCommand("GetForumUserProfileDetails_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, customerId));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.ForumsMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            customerName = dr["Customer"].ToString();
                            customerCity = dr["City"].ToString();
                            customerState = dr["State"].ToString();

                           // reviews = int.Parse(dr["reviews"].ToString());
                           // answers = int.Parse(dr["answers"].ToString());
                            forums = int.Parse(dr["forums"].ToString());
                            //photos = int.Parse(dr["photos"].ToString());

                            //pointsReview = int.Parse(dr["PointsPerReview"].ToString());
                           // pointsAnswer = int.Parse(dr["PointsPerAnswer"].ToString());
                           // pointsForum = int.Parse(dr["PointsPerForumPost"].ToString());
                           // pointsPhotos = int.Parse(dr["PointsPerPhoto"].ToString());
                        }
                    }
                }           
                //points calculations.
               // reviewsTotal = reviews * pointsReview;
                //answersTotal = answers * pointsAnswer;
                //forumsTotal = forums * pointsForum;
              //  photosTotal = photos * pointsPhotos;

              //  total = reviewsTotal + answersTotal + forumsTotal + photosTotal;
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            //finally
            //{
            //    dr.Close();
            //    db.CloseConnection();
            //}

        }

        //FUNCTION RETURNS IMAGE OF THE USER
        private void GetPhotos()
        {
           // SqlDataReader dr = null;
           // Database db = new Database();

            //string sql = "";

            //sql = " SELECT AboutMe, AvtarPhoto, ThumbNail, RealPhoto, UP.HostURL, UP.RealOriginalImgPath, UP.AvtOriginalImgPath " //, C.CommunityId
            //    + " FROM UserProfile UP WITH(NOLOCK), Customers C WITH(NOLOCK) WHERE UserId=C.id AND UserId= @CustomerId";

            //Trace.Warn(sql);
            try
            {
                //passing parameters
               // SqlParameter[] param = { new SqlParameter("@CustomerId", customerId) };

               // dr = db.SelectQry(sql, param);
                using (DbCommand cmd = DbFactory.GetDBCommand("GetUserPhotos_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, customerId));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.CarDataMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            aboutMe = dr["AboutMe"].ToString();
                            thumbNail = dr["ThumbNail"].ToString();
                            realImage = dr["RealPhoto"].ToString();
                            avtar = dr["AvtarPhoto"].ToString();
                            HostURL = dr["HostURL"].ToString();
                            realOriginalImgPath = dr["RealOriginalImgPath"].ToString();
                            avtOriginalImgPath = dr["AvtOriginalImgPath"].ToString();
                        }
                    }
                }
                
                
                if (realOriginalImgPath != "")
                {
                    imgReal.Src = ImageSizes.CreateImageUrl(HostURL, ImageSizes._160X89, realOriginalImgPath);
                    imgReal.Alt = customerName + "'s photo";
                    imgReal.Attributes["Title"] = customerName;
                }
                else
                    imgReal.Src = "https://" + HostURL + "/c/up/no.jpg";

                if (avtar != "")
                    divAvtar.InnerHtml = "<br><img title=\"" + customerName + "'s avtar\" alt=\"" + customerName + "'s avtar\" src='" + ImageSizes.CreateImageUrl(HostURL, ImageSizes._160X89, avtOriginalImgPath)+"'>";
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception          
        }

        // will fetch cars owned by 
        //private void GetUserCars()
        //{
        //    string sql = "";

        //    sql = " SELECT YEAR(MakeYear) CarYear, Ma.Name + ' ' + Mo.Name + ' ' + Ve.Name CarName "
        //        + " FROM MyCarwaleCars MC WITH(NOLOCK), CarMakes Ma WITH(NOLOCK), CarModels Mo WITH(NOLOCK), CarVersions Ve WITH(NOLOCK) "
        //        + " WHERE MC.VersionId=Ve.Id AND Mo.Id=Ve.CarModelId AND Ma.Id=Mo.CarMakeId "
        //        + " AND MC.IsActive=1 AND CustomerId = @CustomerId ";

        //    Trace.Warn(sql);
        //    SqlDataReader dr;
        //    Database db = new Database();

        //    try
        //    {
        //        //passing parameters
        //        SqlParameter[] param = { new SqlParameter("@CustomerId", customerId) };

        //        dr = db.SelectQry(sql, param);

        //        while (dr.Read())
        //        {
        //            if (divCarsOwned.InnerHtml.Length > 0) divCarsOwned.InnerHtml += ", ";
        //            divCarsOwned.InnerHtml += dr["CarYear"].ToString() + " " + dr["CarName"].ToString();
        //        }

        //        dr.Close();
        //        db.CloseConnection();
        //    }
        //    catch (Exception err)
        //    {
        //        Trace.Warn(err.Message);
        //        ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    } // catch Exception

       // }

        // will fetch posts in Forums
        private void GetForumPosts()
        {
            CommonOpn op = new CommonOpn();
            //string sql = "";

            //sql = " SELECT TOP 5 ForumId, FT.Id PostId, Topic, Message, F.Url AS Url "
            //    + " FROM ForumThreads FT WITH(NOLOCK), Forums F WITH(NOLOCK) WHERE F.id=FT.ForumId "
            //    + " AND FT.IsActive = 1 AND FT.IsModerated = 1 AND F.IsActive = 1 AND FT.ID IN (SELECT MAX(FT1.Id) FROM ForumThreads FT1 WITH(NOLOCK) "
            //    + " WHERE FT1.IsActive = 1 AND FT1.IsModerated = 1 AND FT1.CustomerId= @CustomerId "
            //    + " GROUP BY FT1.ForumId) ORDER BY MsgDateTime DESC";

            //Trace.Warn(sql);

            try
            {
                //passing parameters
               // SqlParameter[] param = { new SqlParameter("@CustomerId", customerId) };
                DataSet ds = new DataSet();
                using(DbCommand cmd = DbFactory.GetDBCommand("GetCustomersTopForums_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, customerId));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                }
                op.BindRepeaterReaderDataSet(ds, rptPosts);

                if (rptPosts.Items.Count > 0)
                    divForums.Visible = true;
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }

        public string GetMessage(string message)
        {
            StringCollection sc = new StringCollection();

            sc.Add(@"\[\^\^quote(.|\n)*?\^\^\](.|\n)*?\[\^\^/quote\^\^\]"); // quote text cleanup.
            sc.Add(@"[\n\t\r]*"); // line breaks cleanup
            sc.Add(@"<(.|\n)*?>"); // html tag cleanup

            foreach (string s in sc)
            {
                message = Regex.Replace(message, s, "", RegexOptions.IgnoreCase);
            }

            if (message.Length > 100) message = message.Substring(0, 100) + "...";

            return message;
        }
    } // class
} // namespace