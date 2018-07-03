using Bikewale.Notifications;
using BikewaleOpr.Interface.QuestionsAnswers;
using MySql.CoreDAL;
using QuestionsAnswers.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using BikewaleOpr.Entity.QnA;
using Bikewale.Utility;

namespace BikewaleOpr.DALs.QuestionsAnswers
{
    /// <summary>
    /// Created by  :   Sumit Kate on 16 Jun 2018
    /// Description:    Questions Repository
    /// </summary>
    public class QuestionsRepository : IQuestionsRepository
    {
        public bool PublishModelQuestion(string questionId)
        {
            bool isSuccess = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "publishmodelquestion";


                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_questionid", DbType.String, questionId));
                    isSuccess = MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase) > 0;
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BikewaleOpr.DALs.QuestionsAnswers.QuestionsRepository.PublishModelQuestion, Question Id: {0}", questionId));
            }
            return isSuccess;
        }

        /// <summary>
        /// Created By : Deepak Israni on 16 June 2018
        /// Description: To update the Bikewale Model-Question mapping table when updating tags.
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="modelid"></param>
        public bool UpdateQuestionTags(string questionId, uint modelid)
        {
            bool isSuccess = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "updatequestionmodelmapping";


                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_qid", DbType.String, questionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_newmodelid", DbType.UInt32, modelid));
                    isSuccess = MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase) > 0;
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BikewaleOpr.DALs.QuestionsAnswers.QuestionsRepository.UpdateQuestionTags, Question Id: {0}, Model Id: {1}", questionId, modelid));
            }
            return isSuccess;
        }

        /// <summary>
        /// Created by : Snehal Dange on 20th June 2018
        /// Desc : Increse answer count for questionmodelmapping table
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public bool IncreaseAnswerCount(string questionId)
        {
            bool isSuccess = false;
            try
            {
                if (!string.IsNullOrEmpty(questionId))
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "increasemodelquestionmappinganswercount";
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_questionid", DbType.String, questionId));
                        isSuccess = MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BikewaleOpr.DALs.QuestionsAnswers.QuestionsRepository.IncreaseAnswerCount, Question Id: {0}", questionId));
            }
            return isSuccess;
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 26 June 2018
        /// Description : Function to get data for a model associated with a particular `questionId`
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public BikeModelData GetBikeModelDataForQuestion(string questionId)
        {
            BikeModelData bikeModelData = null;
            try
            {
                if (!string.IsNullOrEmpty(questionId))
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("getbikemodeldatabyquestionid"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_questionid", DbType.String, questionId));

                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (dr != null)
                            {
                                while (dr.Read())
                                {
                                    bikeModelData = new BikeModelData();

                                    bikeModelData.ModelId = SqlReaderConvertor.ParseToUInt32(dr["Id"]);
                                    bikeModelData.BikeName = Convert.ToString(dr["BikeName"]);
                                    bikeModelData.MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                    bikeModelData.ModelMaskingName = Convert.ToString(dr["MaskingName"]);
                                }
                            }
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BikewaleOpr.DALs.QuestionsAnswers.QuestionsRepository.GetBikeModelDataForQuestion, Question Id: {0}", questionId));
            }
            return bikeModelData;
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 27 June 2018
        /// Description : Function to get question ids for a particular model.
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
                ErrorClass.LogError(ex, String.Format("BikewaleOpr.DALs.QuestionsAnswers.GetQuestionIdsByModelId, Model Id: {0}", modelId));
            }

            return questionIds;
        }
    }
}
