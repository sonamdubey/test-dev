using System.Web.UI;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 08 Nov 2016
    /// Summary: Control to show nav links depending on if the customer is logged in o not
    /// </summary>
    public class LogInOutControl : UserControl
    {
        protected string loggedInUser = Bikewale.Common.CurrentUser.Id;
    }   // End of Class
}   // End of namespace