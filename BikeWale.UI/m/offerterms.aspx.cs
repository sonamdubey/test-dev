using Bikewale.Entities.PriceQuote;
using Microsoft.Practices.Unity;
using System;

namespace Bikewale.Mobile
{
    public class OfferTerms : System.Web.UI.Page
    {
        public string htmlContent = string.Empty;
        public bool isExpired = false;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                string MaskingName = Convert.ToString(Request.QueryString["id"]);
                MaskingName = MaskingName.Replace("/", string.Empty);
                if (!string.IsNullOrEmpty(MaskingName))
                {
                    htmlContent = GetHtml(MaskingName, out isExpired);
                    if (string.IsNullOrEmpty(htmlContent))
                        Server.Transfer("default.aspx");
                }
            }
        }
        /// <summary>
        /// Written by: Sangram Nandkhile on 8 Oct 2015
        /// Function to get Terms and condition for Masking Name
        /// </summary>
        /// <param name="maskingName"></param>
        /// <returns></returns>
        protected string GetHtml(string maskingName, out bool isExpired)
        {
            isExpired = default(bool);
            try
            {
                OfferHtmlEntity objTerms = null;

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealerPriceQuote, Bikewale.DAL.AutoBiz.DealerPriceQuoteRepository>();
                    Bikewale.Interfaces.AutoBiz.IDealerPriceQuote objCategoryNames = container.Resolve<Bikewale.DAL.AutoBiz.DealerPriceQuoteRepository>();
                    objTerms = objCategoryNames.GetOfferTerms(maskingName, 0);
                }

                if (objTerms != null)
                {
                    isExpired = objTerms.IsExpired;
                    return objTerms.Html;
                }
                else
                    return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}