using System;

namespace Bikewale.Notifications.MailTemplates
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 13-Sep-2017
    /// Summary: Capital first email template
    /// </summary>
    public class CapitalFirstSuccessEmailTemplate : ComposeEmailBase
    {
        private readonly string emailHtml;
        public CapitalFirstSuccessEmailTemplate(string bikeName, string saleOfficerName, string salesOfficerNumber)
        {
            emailHtml = string.Format(@"<div><p>Thank you for expressing interest in availing a loan for {0}. We have shared your details with Capital First, who is our finance partner.</p><p></p><p>The Capital First representative {1} will reach out to you for further steps. Alternatively, you can reach out to {1} at {2}.</p><p></p><p>In case of any concerns, please reach out to us at contact@bikewale.com</p><p></p><p>Regards,</p><p>Team BikeWale</p></div>", bikeName, saleOfficerName, salesOfficerNumber);
        }

        public override string ComposeBody()
        {
            return Convert.ToString(emailHtml);
        }
    }
}

