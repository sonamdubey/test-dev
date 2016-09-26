using Bikewale.Entities.Pager;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Modified By : Ashwini Todkar on 30 Sept 2014 
    /// Removed Page_Load and oninit event as not needed 
    /// </summary>
    public class ListPagerControl : System.Web.UI.UserControl
    {
        protected string prevPageUrl = string.Empty, nextPageUrl = string.Empty;
        protected DropDownList ddlPage;

        public PagerOutputEntity PagerOutput { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPageNo { get; set; }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 21th may 2014
        /// Summary : to bind pager dropdownlist
        /// </summary>
        public void BindPageNumbers()
        {
            //bind bottom paging dropdownlist with total number of pages
            if (TotalPages > 1)
            {
                ddlPage.DataSource = PagerOutput.PagesDetail;
                ddlPage.DataTextField = "PageNo";
                ddlPage.DataValueField = "PageUrl";
                ddlPage.DataBind();

                prevPageUrl = PagerOutput.PreviousPageUrl;
                nextPageUrl = PagerOutput.NextPageUrl;

                ddlPage.SelectedIndex = CurrentPageNo - 1;
            }
        }
    }
}