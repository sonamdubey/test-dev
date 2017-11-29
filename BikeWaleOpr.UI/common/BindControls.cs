using BikeWaleOpr.Common;
using System.Web.UI.WebControls;

namespace BikeWaleOpr
{
    // Class just to binding controls
    public class BindControls
    {
        public static void BindAllMakes(DropDownList drpMake)
        {
            string sql = "SELECT ID, NAME FROM BIKEMAKES WHERE ISDELETED = 0 ORDER BY NAME";
            CommonOpn objCom = new CommonOpn();
            objCom.FillDropDown(sql, drpMake, "Name", "ID");
            drpMake.Items.Insert(0, new ListItem("--Select--", "-1"));
        }

    }
}