using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using Carwale.UI.Controls;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications;
using Carwale.DAL.Customers;
using Carwale.Entity;

namespace Carwale.UI.Newsletter
{
    public class Unsubscribe : Page
    {
        protected TextBox txtEmail;
        protected Button butUnsubscribe;
        protected HtmlGenericControl dReq, dMes;

        protected override void OnInit(EventArgs e)
        {            
            butUnsubscribe.Click += new EventHandler(butUnsubscribe_Click);
        }

        void butUnsubscribe_Click(object Sender, EventArgs e)
        {
            var email = txtEmail.Text;
            
            if( Carwale.Utility.RegExValidations.IsValidEmail(email) )
            {
                var customersRepo = new CustomerRepository<Customer, CustomerOnRegister>();
                customersRepo.UnsubscribeNewsletter(email);
                dReq.Visible = false;
                dMes.Visible = true;
            }
        }
    }//class
}//namespace