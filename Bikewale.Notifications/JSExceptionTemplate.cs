using System;
using System.Web;
using Bikewale.Entities.JSErrorLog;

namespace Bikewale.Notifications
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created On : 10th March 2016
    /// Description  : Mail Template to compose body for JS Exception / Error logging 
    /// </summary>
    public class JSExceptionTemplate : ComposeEmailBase
    {

        private readonly JSExceptionEntity _error = null;

        /// <summary>
        /// Summary : Constructor to intialize the parameters for creating JSerror/JSException mail template.
        /// </summary>
        /// <param name="ex">Exception/Error object.</param>
        public JSExceptionTemplate(JSExceptionEntity ex)
        {
            _error = ex;
        }

        private readonly string ErrorMsgBody = @"Person Accessing the Page : <br /><br /><b>HOST :</b> {0}<br /><b>URL :</b> {1}<br /><b>REWRITE URL :</b> {2}<br /><b>REFERRER :</b> {3}<br /><b>Error Type :</b> {4}<br/><b>Message :</b> {5}<br/><b>Page Url :</b> {6}<br /><b>Source File :</b> {7}<br /><b>Line No :</b> {8}<br /><b>Trace :</b> {9}<br />";

        /// <summary>
        /// Created By  : Sushil Kumar
        /// Created On : 10th March 2016
        /// Description : To override body text for default mail and set text value related to JS Exception/Error Object
        /// </summary>
        /// <returns></returns>
        public override string ComposeBody()
        {
            string _mailBodyText = string.Empty;
            try
            {
                var ServerVar = HttpContext.Current.Request.ServerVariables;
                _mailBodyText = string.Format(ErrorMsgBody, ServerVar["HTTP_HOST"], ServerVar["URL"], ServerVar["HTTP_X_REWRITE_URL"], ServerVar["HTTP_REFERER"], _error.ErrorType, _error.Message, ServerVar["HTTP_REFERER"], _error.SourceFile, _error.LineNo, _error.Trace);
            }
            catch (Exception e)
            {
                HttpContext.Current.Trace.Warn("Notifications.JSExceptionTemplate ComposeBody : " + e.Message);
            }
            return _mailBodyText;
        }

    }
}
