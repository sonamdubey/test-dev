using System;
using System.Web;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BikeWaleOpr.Common;
using BikeWaleOpr.Content;
using AjaxPro;

namespace BikeWaleOpr
{
    /// <summary>
    ///     Modified By : Ashish G. Kamble on 2 Apr 2013
    ///     Summary : Class have ajax functions related to customer reviews.
    /// </summary>
    public class ReviewDetail
    {
        /// <summary>
        ///     Modified By : Ashish G. Kamble on 2 Apr 2013
        ///     Summary : Function approve the user review of the customer.
        /// </summary>
        /// <param name="Id"></param>
        [AjaxPro.AjaxMethod()]
        public void SetIsReviewed(string Id)
        {
            try
            {
                Database db = new Database();
                char[] MyChar = {'A'};
                string ID = Id.TrimEnd(MyChar);
                string sql = "update CustomerReviews set IsVerified = '1', IsDiscarded = '0', LastUpdatedOn = GETDATE(), LastUpdatedBy = " + CurrentUser.Id + " where ID = " + ID;
                int a = db.UpdateQryRetRows(sql);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "ReviewDetail.SetIsReviewed");
                objErr.SendMail();
            }
        }

        /// <summary>
        ///     Modified By : Ashish G. Kamble on 2 Apr 2013
        ///     Summary : Function discard the user review of the customer.
        /// </summary>
        /// <param name="Id"></param>
        [AjaxPro.AjaxMethod()]
        public void SetIsDiscarded(string Id)
        {
            try
            {
                Database db = new Database();
                char[] MyChar = {'D'};
                string ID = Id.TrimEnd(MyChar);
                string sql = "update CustomerReviews set IsDiscarded = '1', IsVerified = '0' , LastUpdatedOn = GETDATE(), LastUpdatedBy = " + CurrentUser.Id + " where ID = " + ID;
                int a = db.UpdateQryRetRows(sql);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "ReviewDetail.SetIsDiscarded");
                objErr.SendMail();
            }
        }
    }
}
