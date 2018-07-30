using Bikewale.Interfaces.Feedback;

namespace Bikewale.DAL.Feedback
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 21 Jan 2015
    /// </summary>
    public class FeedbackRepository : IFeedback
    {
        /// <summary>
        /// Created By : Sadhana Upadhyay on 21 Jan 2015
        /// Summary : To Save Customer feedback
        /// </summary>
        /// <param name="feedbackComment"></param>
        /// <param name="feedbackType"></param>
        /// <param name="platformId"></param>
        /// <returns></returns>
        public bool SaveCustomerFeedback(string feedbackComment, ushort feedbackType, ushort platformId,string pageUrl)
        {
            bool isSaved = false;
            //Database db = null;

            //try
            //{
            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        cmd.CommandText = "SaveFeedback";
            //        cmd.CommandType = CommandType.StoredProcedure;

            //        cmd.Parameters.Add("@FeedbackComment", SqlDbType.VarChar, 500).Value = feedbackComment;
            //        cmd.Parameters.Add("@FeedbackType", SqlDbType.TinyInt).Value = feedbackType;
            //        cmd.Parameters.Add("@PlatformId", SqlDbType.TinyInt).Value = platformId;
            //        cmd.Parameters.Add("@PageUrl", SqlDbType.VarChar, 150).Value = pageUrl;

            //        db = new Database();

            //        isSaved = db.InsertQry(cmd);
            //    }
            //}
            //catch (SqlException ex)
            //{
            //    HttpContext.Current.Trace.Warn("SaveCustomerFeedback sql ex : " + ex.Message + ex.Source);
            //    ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    
            //    isSaved = false;
            //}
            //catch (Exception ex)
            //{
            //    HttpContext.Current.Trace.Warn("SaveCustomerFeedback ex : " + ex.Message + ex.Source);
            //    ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    
            //    isSaved = false;
            //}
            //finally
            //{
            //    db.CloseConnection();
            //}
            return isSaved;
        }   //End of SaveCustomerFeedback
    }   //End of class
}   //End of namespace
