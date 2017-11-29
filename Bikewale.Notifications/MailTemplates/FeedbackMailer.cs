﻿using System;

namespace Bikewale.Notifications.MailTemplates
{
    public class FeedbackMailer : ComposeEmailBase
    {
        public string PageUrl { get; set; }
        public string Feedback { get; set; }

        public FeedbackMailer(string pageUrl,string feedback)
        {
            PageUrl = pageUrl;
            Feedback = feedback;
        }

        static readonly string _MailBody = "<div style=\"max-width:670px; margin:0 auto; border:1px solid #d8d8d8; font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; padding:10px; word-wrap:break-word\"><div style=\"margin:5px 0 0; background:#fff;\"><div style=\"padding:10px 10px 0;\"><div style=\"font-size:14px; font-weight:bold; color:#333333; margin-bottom:10px; float:left\">Dear Saurabh Awasthi,</div><div style=\"font-size:14px; font-weight:normal; color:#333333; margin-bottom:10px; float:right\">{0}</div><div style=\"clear:both;\"></div><div style=\"color:#666666;\">BikeWale user has submitted a feedback. The feedback details are as follows: </div></div></div><div style=\"margin:5px 0 0; background:#fff;\"><div style=\"padding:10px 10px 0;\"><table><tbody><tr><td style=\"width:70px;font-size:14px;\"><b>Page Url : </b></td><td style=\"font-size:12px;\"><a href=\"{1}\">{1}</a></td></tr><tr><td style=\"font-size:14px;\"><b>Feedback : </b></td><td style=\"font-size:12px;\">{2}</td></tr></tbody></table></div></div></div>";

        public override string ComposeBody()
        {
           //  StringBuilder sb = null;
          return string.Format(_MailBody, DateTime.Now.ToString("dd MMMM yyyy"), PageUrl, Feedback);

            //try
            //{



            //  sb = new StringBuilder();

            //  sb.Append("<div style=\"max-width:670px; margin:0 auto; border:1px solid #d8d8d8; font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; padding:10px; word-wrap:break-word\">");
            //  sb.Append("<div style=\"margin:5px 0 0; background:#fff;\"><div style=\"padding:10px 10px 0;\"><div style=\"font-size:14px; font-weight:bold; color:#333333; margin-bottom:10px; float:left\">Dear Saurabh Awasthi,</div>");
            //  sb.Append("<div style=\"font-size:14px; font-weight:normal; color:#333333; margin-bottom:10px; float:right\">" + DateTime.Now.ToString("dd MMMM yyyy") + "</div><div style=\"clear:both;\"></div>");
            //  sb.Append("<div style=\"color:#666666;\">BikeWale user has submitted a feedback. The feedback details are as follows: </div></div></div><div style=\"margin:5px 0 0; background:#fff;\">");
            //  sb.Append("<div style=\"padding:10px 10px 0;\"><table><tbody><tr><td style=\"width:70px;font-size:14px;\"><b>Page Url : </b></td><td style=\"font-size:12px;\"><a href=\"" + PageUrl + "\">" + PageUrl + "</a></td></tr>");
            //  sb.Append("<tr><td style=\"font-size:14px;\"><b>Feedback : </b></td><td style=\"font-size:12px;\">" + Feedback + "</td></tr></tbody></table></div></div></div>");
            //}
            //catch (Exception ex)
            //{
            //  HttpContext.Current.Trace.Warn("Notifications.ErrorTempate ComposeBody : " + ex.Message);
            //}
            //return sb.ToString();
        }
    }
}
