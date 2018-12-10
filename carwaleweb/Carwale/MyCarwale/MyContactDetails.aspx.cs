using Carwale.BL.Forums;
using Carwale.DAL.Customers;
using Carwale.Entity.Enum;
using Carwale.Entity.Forums;
using Carwale.UI.Common;
using System;

namespace Carwale.UI.MyCarwale
{
    public partial class MyContactDetails : System.Web.UI.Page
    {
        public CustomerDetails cd;
        public UserProfile result;
        protected string imgCategory = "0"; 
        protected string userId = string.Empty; 
        protected string userIdReal = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentUser.Id == "-1")
            {
                Trace.Warn("yipeee....");
                Response.Redirect("/Users/Login.aspx?returnUrl=/MyCarwale/MyContactDetails.aspx");
            }
            cd = new CustomerDetails(CurrentUser.Id);
            imgCategory = Convert.ToInt16(ImageCategories.FORUMSREALIMAGE).ToString();
            result = new UserProfile();
            UserBusinessLogic userDetails = new UserBusinessLogic();
            result = userDetails.GetProfileDetails(Convert.ToInt32(CurrentUser.Id));
            if (result != null)
            {
                result.AvtarPhoto = result.AvtarPhoto != "" ? "a/" + result.AvtarPhoto : "no.jpg";
                result.RealPhoto = result.RealPhoto != "" ? "r/" + result.RealPhoto : "no.jpg";
                if (result.HandleName != null && result.HandleName.ToString() != "")
                {
                    if (result.IsUpdated.ToString() == null || result.IsUpdated.ToString() == "False")
                    {
                        result.HandleName = result.HandleName + "&nbsp;[<a href='/users/EditUserHandle.aspx'>Please update your Community User Name</a>]";
                    }
                }
                else
                {
                    result.HandleName = "[<a href='/users/EditUserHandle.aspx'>Please choose your Community User Name</a>]";
                }
            }
            else
            {
                result = new UserProfile();
                result.AboutMe = string.Empty;
                result.AvtarPhoto = string.Empty;
                result.AvtOriginalImgPath = string.Empty;
                result.StatusId = "1";
                result.Signature = string.Empty;
                result.RealPhoto = string.Empty;
                result.RealOriginalImgPath = string.Empty;
                result.HandleName = "[<a href='/users/EditUserHandle.aspx'>Please choose your Community User Name</a>]";
            }
            userId = CurrentUser.Id;
            userIdReal = CurrentUser.Id;
            Trace.Warn("done...");
        }      
    }
}