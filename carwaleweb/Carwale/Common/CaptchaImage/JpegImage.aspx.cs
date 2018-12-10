using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using Carwale.Utility;

namespace Carwale.UI.Common.CaptchaImage
{
    public class JpegImage : System.Web.UI.Page
    {
        // For generating random numbers.
        private Random random = new Random();

        private void Page_Load(object sender, System.EventArgs e)
        {
            string imageText = GenerateRandomCode();



            // check if cookie is already there. if yes, delete it
            //if ( Request.Cookies["CaptchaImageText"] != null )
            //    Response.Cookies["CaptchaImageText"].Expires = DateTime.Now.AddDays(-1);

            //Response.Write("Encrypted Cookie!!" + CarwaleSecurity.Encrypt(imageText));

            // Create a random code and store it in the cookie object.
            HttpCookie cookie = new HttpCookie("CaptchaImageText");
            cookie.Value = CarwaleSecurity.Encrypt(imageText);
            cookie.Expires = DateTime.Now.AddHours(5);
            Response.Cookies.Add(cookie);

            using (CaptchaImage ci = new CaptchaImage(imageText, 200, 50, "Century Schoolbook"))
            {
                // Change the response headers to output a JPEG image.
                this.Response.Clear();
                this.Response.ContentType = "image/jpeg";

                // Write the image to the response stream in JPEG format.
                ci.Image.Save(this.Response.OutputStream, ImageFormat.Jpeg); 
            }
        }


        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion

        private string GenerateRandomCode()
        {
            string s = "";
            for (int i = 0; i < 6; i++)
                s = String.Concat(s, this.random.Next(10).ToString());
            return s;
        }
    }
}