/*THIS CLASS HOLDS ALL TH EFUNCTION FOR BINDING GRID, FILLING DROPDOWN LIST AND OTHER SORTS OF
COMMON OPERATIONS.
*/

using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Security.Principal;
using System.Web.Mail;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using System.Net;
using System.IO;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using Carwale.Entity.Enum;
using Carwale.Interfaces;
using Carwale.Entity;
using Carwale.BL.Customers;

namespace Carwale.UI.Common
{
    public class SourceIdCommon
    {
        public static void UpdateSourceId(EnumTableType tbl, string id)
        {
            ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
            customerRepo.UpdateSourceId(tbl, id);
        }
    }
}
