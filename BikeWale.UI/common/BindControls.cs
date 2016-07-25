using Bikewale.Common;
using System.Web.UI.WebControls;

namespace Bikewale
{
    // Class just to binding controls
    public class BindControls
    {
        public static void BindAllMakes(DropDownList drpMake)
        {
            string sql = "select id, name from bikemakes where isdeleted = 0 order by name";
            CommonOpn objCom = new CommonOpn();
            objCom.FillDropDown(sql, drpMake, "Name", "ID");
            drpMake.Items.Insert(0, new ListItem("--Select--", "-1"));
        }

    }
}