using Bikewale.Entities.Finance.CapitalFirst;

namespace Bikewale.RabbitMq.CapitalFirstLeadConsumer.htmltemplates
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 13-Sep-2017
    /// Summary: Class to create PDF for a lead
    /// 
    /// </summary>
    public class PdfAttachment
    {
        private readonly CapitalFirstLeadEntity _leadDetail = null;

        public PdfAttachment(CapitalFirstLeadEntity leadDetails)
        {
            _leadDetail = leadDetails;
        }
        public string ComposeBody()
        {
            string pdfHtml = string.Empty;
            try
            {
                if (_leadDetail != null)
                {
                    string strHtml = @"<html><body style='margin:0 auto;text-align:center;'><div style='max-width: 532px; padding: 30px 9px;margin:0 auto;background:#f5f5f5;'><div style='max-width: 462px; margin: 0 auto;  padding: 0; font-family: Arial, Helvetica, sans-serif; font-size:14px; color:#4d5057; background: #FFFFFF; box-shadow: 0 4px 8px 0 rgba(162,162,162,0.50); word-wrap:break-word;'><div style='color:#fff; width:100%; min-height:285px; background:url(""https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/teal-congrats-banner-463x286.png"") no-repeat center; '>     <!-- banner starts here --><div style='font-size:10px; float:left; margin:20px;'><img src='https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/capitalfirst-logo-70x29.png' alt='Capital_First' title='Capital_First' width='100%' border='0' /></div><div style='font-size:10px; float:right; margin:20px;'><img src='https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-white-logo-82x30.png' alt='BikeWale' title='BikeWale' width='100%' border='0' /></div><div style='clear:both;'></div></div>     <!-- banner ends here -->    <div style='padding:0 15px'>     <!-- main content starts here --><div style='font-weight:bold; margin-bottom:12px; padding:20px 20px 0;'>Dear {0},</div><div style='color: #82888B; margin-bottom:5px;line-height:1.5; letter-spacing: 0;text-align:center; font-size:12px;'>Your request has been approved by Capital Finance Ltd.<br/>You have been sanctioned a loan amount of &#8377; {1} </div><div style='padding:15px 0; text-align:center; font-size: 12px;'> <div style='text-align:center;padding-bottom:10px; color: #82888B;'>EMI payable for a tenure of 36 months:</div><div style='font-weight:bold; text-align:center;padding-bottom:10px; color: #002027;letter-spacing: 0;line-height: 14px;'><span style='font-weight:normal;padding:0 3px;'> &#8377;</span>{2} per month</div></div>        <!-- bike details ends here --><div style=' padding-bottom:70px; text-align:center; '>         <!-- bike details starts here --><div style='margin: 0 auto;'><div style='color:#82888b;padding:27px 0;border:1px solid #4D5057; vertical-align:middle;font-size:12px;  margin:0 auto; text-align:center; display: inline-block;min-width: 260px;'><span style='padding-bottom:25px;display:inline-block;text-transform:uppercase;'>loan application<br/> voucher</span><div style='font-weight: bold;font-size: 24px;color: #3799A7;padding-bottom:12px;'>{3}</div><span style='font-size: 9px;'>Valid till {4}</span></div></div></div></div> <!-- steps --></div> <!-- main content ends here --><div style='font-size: 10px; color: #82888B; letter-spacing: 0;padding-top:10px;'> Read all the Terms & Conditions related to this offer on <a href='' style='text-decoration:none;color: #82888B; cursor:pointer; '> www.capitalfirst.com </a></div></div></body></html>";
                    pdfHtml = string.Format(strHtml, string.Format("{0} {1}", _leadDetail.FirstName, _leadDetail.LastName), _leadDetail.LoanAmountStr, _leadDetail.Emi, _leadDetail.VoucherNumber, _leadDetail.VoucherExpiryDate.ToString("dd-MMM-yyyy"));
                }
            }
            catch (System.Exception ex)
            {

                new Bikewale.Notifications.ErrorClass(ex, "Bikewale.RabbitMq.CapitalFirstLeadConsumer.ComposeBody()");
            }
            return pdfHtml;
        }
    }
}
