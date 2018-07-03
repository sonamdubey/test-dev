 using Bikewale.Interfaces.QuestionAndAnswers;
using Bikewale.Notifications;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DAL.QuestionAndAnswers
{
    public class QuestionsRepository : IQuestionsRepository
    {
        /// <summary>
        /// Created By : Deepak Israni on 14 June 2018
        /// Description: Function to store the mapping of question id and model id.
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="modelId"></param>
        public void StoreQuestionModelMapping(Guid? questionId, uint modelId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "storequestionmodelmapping";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_qid", DbType.String, questionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.UInt32, modelId));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.DAL.QuestionAndAnswers.StoreQuestionModelMapping, Question Id: {0}, Model Id: {1}.", questionId, modelId));
            }
        }

        /// <summary>
        /// Created By : Deepak Israni on 20 June 2018
        /// Description: Function to get all the question Ids for a certain model id.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<string> GetQuestionIdsByModelId(uint modelId)
        {
            IEnumerable<string> questionIds = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "questionidsbymodelid";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.UInt32, modelId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            IList<String> ids = new List<String>();

                            while (dr.Read())
                            {
                                ids.Add(Convert.ToString(dr["questionid"]));
                            }

                            questionIds = ids;
                        }
                
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.DAL.QuestionAndAnswers.GetQuestionIdsByModelId, Model Id: {0}", modelId));
            }

            return questionIds;
        }
    }
}
