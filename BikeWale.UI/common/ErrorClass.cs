// ErrorClass.cs
//

using System;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace Bikewale.Common
{
    public class ErrorClass
    {
        /// <summary>
        /// constructor which assigns the exception
        /// </summary>
        /// <param name="ex">Exception object.</param>
        /// <param name="pageUrl">Page URL on which error occured. If possible pass function name also.</param>
        public ErrorClass(Exception ex, string pageUrl)
        {
            Bikewale.Notifications.ErrorClass.LogError(ex, pageUrl);
        }

        /// <summary>
        /// constructor which assigns the exception
        /// </summary>
        /// <param name="ex">Sql Exception object.</param>
        /// <param name="pageUrl">Page URL on which error occured. If possible pass function name also.</param>
        public ErrorClass(SqlException ex, string pageUrl)
        {
            Bikewale.Notifications.ErrorClass.LogError(ex, pageUrl);
        }

        /// <summary>
        /// constructor which assigns the exception
        /// </summary>
        /// <param name="ex">OleDbException Exception object.</param>
        /// <param name="pageUrl">Page URL on which error occured. If possible pass function name also.</param>
        public ErrorClass(OleDbException ex, string pageUrl)
        {
            Bikewale.Notifications.ErrorClass.LogError(ex, pageUrl);
        }

        public static void LogError(Exception ex, string pageUrl)
        {
            Bikewale.Notifications.ErrorClass.LogError(ex, pageUrl);
        }
    }//class
}//namespace
