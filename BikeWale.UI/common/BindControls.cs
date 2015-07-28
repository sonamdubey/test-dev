using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Drawing;
using System.Configuration;
using Bikewale.Common;
using Ajax;

namespace Bikewale
{
    // Class just to binding controls
    public class BindControls
    {
        public static void BindAllMakes(DropDownList drpMake)
        {
            string sql = "SELECT ID, NAME FROM BIKEMAKES WITH(NOLOCK) WHERE ISDELETED = 0 ORDER BY NAME";
            CommonOpn objCom = new CommonOpn();
            objCom.FillDropDown(sql, drpMake, "Name", "ID");
            drpMake.Items.Insert(0, new ListItem("--Select--", "-1"));
        }

    }
}