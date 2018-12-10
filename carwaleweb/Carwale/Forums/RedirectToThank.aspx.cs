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
using Ajax;
using Carwale.UI.Common;
using Carwale.DAL.Forums;

namespace Carwale.UI.Forums
{
    public class RedirectToThank : Page
    {
        #region Global Variables
        private string[] splittedParams;
        protected string postId;
        protected string ch;
        protected string handleExists;
        protected string isSaved = "-2";
        #endregion

        #region On Init
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }
        #endregion

        #region Initialize Component
        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }
        #endregion

        #region Page Load
        void Page_Load(object Sender, EventArgs e)
        {
            splittedParams = Request.QueryString["params"].ToString().Split(',');
            postId = splittedParams[0].ToString();
            ch = splittedParams[1].ToString();
            if (CommonOpn.CheckId(postId) == false || CommonOpn.CheckId(ch) == false)
            {
                return;
            }
            if (ch == "0")
            {
                handleExists = "1";
                if (!NotPostedByCurrentUser())
                {
                    SaveToPostThanks(CurrentUser.Id, postId);
                }
            }
            else if (ch == "1")
            {
                handleExists = "0";
                if (DoesHandleExists(CurrentUser.Id))
                {
                    handleExists = "1";
                    if (!NotPostedByCurrentUser())
                    {
                        SaveToPostThanks(CurrentUser.Id, postId);
                    }
                }
            }
        }
        #endregion

        #region Not Posted By Current User
        protected bool NotPostedByCurrentUser()
        {
            bool retVal = true;
            UserDAL userDetails = new UserDAL();
            retVal = userDetails.GetLoginStatus(CurrentUser.Id, postId);
            if (!retVal) isSaved = "-3";
            return retVal;
        }
        #endregion

        #region Does Handle Exist
        private bool DoesHandleExists(string cId)
        {
            UserDAL userDetails = new UserDAL();
            return (userDetails.CheckUserHandle(cId));
        }
        #endregion

        #region Save To Post Thanks
        private void SaveToPostThanks(string _customerId, string _postId)
        {
            PostDAL postDetails = new PostDAL();
            if (postDetails.SaveToPostThanks(_customerId, _postId))
                isSaved = "1";
            else
                isSaved = "0";
        }
        #endregion
    }
}