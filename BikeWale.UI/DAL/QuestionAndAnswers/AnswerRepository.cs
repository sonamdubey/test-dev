
using Bikewale.Interfaces.QuestionAndAnswers;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
namespace Bikewale.DAL.QuestionAndAnswers
{
    /// <summary>
    /// Created By : Snehal Dange on 18th July 2018
    /// </summary>
    public class AnswerRepository : IAnswerRepository
    {
        public bool CheckDuplicateAnswerByUser(string questionId, uint customerId)
        {
            bool isDuplicate = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("checkuseranswer"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_questionid", DbType.String, questionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.UInt32, customerId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            isDuplicate = SqlReaderConvertor.ToBoolean(dr["status"]);
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.DAL.QuestionAndAnswers.AnswerRepository.CheckDulpicateAnswerByUser, Question Id: {0}, Customer  Id: {1}.", questionId, customerId));
            }
            return isDuplicate;
        }
    }
}
