﻿using BikeWaleOpr.Common;
using MySql.CoreDAL;
using System;

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

                char[] MyChar = { 'A' };
                string ID = Id.TrimEnd(MyChar);
                string sql = "update customerreviews set isverified = 1, isdiscarded = 0, lastupdatedon = now(), lastupdatedby = " + CurrentUser.Id + " where id = " + ID;
                int a = MySqlDatabase.UpdateQueryReturnRowCount(sql, ConnectionType.MasterDatabase);
                BikewaleOpr.Cache.BwMemCache.ClearUserReviewsCache();
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
                char[] MyChar = { 'D' };
                string ID = Id.TrimEnd(MyChar);
                string sql = "update customerreviews set isdiscarded = 1, isverified = 0 , lastupdatedon = now(), lastupdatedby = " + CurrentUser.Id + " where id = " + ID;
                int a = MySqlDatabase.UpdateQueryReturnRowCount(sql, ConnectionType.MasterDatabase);
                BikewaleOpr.Cache.BwMemCache.ClearUserReviewsCache();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "ReviewDetail.SetIsDiscarded");
                objErr.SendMail();
            }
        }
    }
}
