using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Common;
using Bikewale.CV;
using System.Data;
using System.Data.SqlClient;

namespace Bikewale.Used
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 21/8/2012
    ///     Class will do the user mobile verification and redirect customer to upload photos
    /// </summary>
    public class VerifySellBikeUser : Page
    {
        protected TextBox txtVerificationCode;
        protected Button btnVerifyCustomer;
        protected Label lblError;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnVerifyCustomer.Click += new EventHandler(btnVerifyCustomer_Click);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblError.Visible = false;                
            }
        }

        protected void btnVerifyCustomer_Click(object sender, EventArgs e)
        {
            VerifyCustomer();
        }

        /// <summary>
        ///     PopulateWhere to verify the the code entered by customer
        /// </summary>
        protected void VerifyCustomer()
        {
            bool isVerified = false;
            string cwiCode = string.Empty, mobile = string.Empty;
   
            cwiCode = txtVerificationCode.Text.Trim();
            mobile = CookiesCustomers.Mobile;

            if (!String.IsNullOrEmpty(mobile) && !String.IsNullOrEmpty(cwiCode))
            {
                CustomerVerification objCV = new CustomerVerification();
                isVerified = objCV.CheckVerification(mobile, cwiCode, "");
            }

            if (isVerified && CookiesCustomers.SellInquiryId != "-1")
            {
                RegisterCustomer objRC = new RegisterCustomer();
                SellBikeCommon common = new SellBikeCommon();

                common.UpdateIsVerifiedCustomer(CookiesCustomers.SellInquiryId);

                //If user verified then update customer name and mobile
                objRC.UpdateCustomerMobile(CookiesCustomers.Mobile, CookiesCustomers.Email, CookiesCustomers.CustomerName);


                Response.Redirect("uploadbasic.aspx");
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "Please Enter valid code.";
            }
        }

    }   // End of class
}   // End of namespace