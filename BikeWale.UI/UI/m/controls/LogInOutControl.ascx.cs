using System.Web.UI;

namespace Bikewale.Mobile.Controls
{

    /// <summary>
    /// Created by: Sangram Nandkhile on 16 Nov 2016
    /// Desc: To toggle navbar when logged in or not
    /// </summary>
    public class LogInOutControl : UserControl
    {
        protected uint loggedInUserId = Bikewale.Common.CurrentUser.UserId;
    }   // End of Class
}   // End of namespace