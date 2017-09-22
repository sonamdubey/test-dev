using Bikewale.Entities.Finance.CapitalFirst;

namespace Bikewale.RabbitMq.CapitalFirstLeadConsumer.htmltemplates
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 13-Sep-2017
    /// Summary: Class to create PDF for a lead
    /// 
    /// </summary>
    public class CapitalFirstPdfAttachment
    {
        private readonly CapitalFirstLeadEntity _leadDetail = null;

        public CapitalFirstPdfAttachment(CapitalFirstLeadEntity leadDetails)
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
                    string strHtml = @"<html><body style='margin:0 auto;text-align:center;'><div style='max-width: 930px; padding-top:106px; padding-bottom:66px;margin:0 auto;background:#f5f5f5;color-adjust: exact !important; -webkit-print-color-adjust:exact; '><div style='max-width: 724px;margin: 0 auto; padding: 0; font-family: Arial, Helvetica, sans-serif; color:#4d5057; background: #FFFFFF; box-shadow: 0 4px 8px 0 rgba(162,162,162,0.50); word-wrap:break-word;'><div style='color:#fff; width:100%; min-height:448px;color-adjust: exact !important; -webkit-print-color-adjust:exact; background:url(""https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/teal-congrats-banner-724x449.png"") no-repeat center; '> <div style='float:left; margin:30px;'><img src='https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/capital-first-111x46.png' alt='Capital_First' title='Capital_First' width='100%' border='0' /></div><div style='float:right; margin:30px;'><img src='https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-white-logo-102x31.png' alt='BikeWale' title='BikeWale' width='100%' border='0' /></div><div style='clear:both;'></div></div> <div style='text-align:center;'> <div style='font-weight:bold; margin-bottom:20px; padding-top:31px; font-size: 21px;'>Dear {0},</div><div style='color: #82888B; margin-bottom:48px;line-height:1.5; letter-spacing: 0;text-align:center; font-size:19px;'>Your request has been approved by Capital Finance Ltd.<br/>You have been sanctioned a loan amount of &#8377; {1} </div><div style='text-align:center; font-size:19px;'> <div style='text-align:center;padding-bottom:20px; color: #82888B;'>EMI payable for a tenure of 36 months:</div><div style='font-weight:bold; text-align:center;padding-bottom:71px; color: #002027;letter-spacing: 0;line-height: 21px;'><span style='font-weight:normal;padding:0 5px;'> &#8377;</span>{2} per month</div></div><div style=' padding-bottom:117px; text-align:center; '> <div style='margin: 0 auto;'><div style='color:#82888b;padding:43px 0;border:1px solid #4D5057; vertical-align:middle; margin:0 auto; text-align:center; display: inline-block;min-width: 470px;'><span style='padding-bottom:41px;display:inline-block;text-transform:uppercase; font-size:19px;'>loan application<br/> voucher</span><div style='font-weight: bold;font-size: 38px;color: #3799A7;padding-bottom:19px;'>{3}</div><span style='font-size: 14px;'>Valid till {4}</span></div></div></div></div></div><div style='font-size: 15px; color: #82888B; letter-spacing: 0;padding:10px;'> Read all the Terms & Conditions related to this offer on <a href='' style='text-decoration:none;color: #82888B; cursor:pointer; '>www.capitalfirst.com </a></div></div></body></html>";
                    pdfHtml = string.Format(strHtml, _leadDetail.FullName, _leadDetail.LoanAmountStr, _leadDetail.Emi, _leadDetail.VoucherNumber, _leadDetail.VoucherExpiryDate.ToString("dd-MMM-yyyy"));
                }
            }
            catch (System.Exception ex)
            {
                new Bikewale.Notifications.ErrorClass(ex, "Bikewale.RabbitMq.CapitalFirstLeadConsumer.CapitalFirstPdfAttachment.ComposeBody()");
            }
            return pdfHtml;
        }
    }
}
