using MySql.CoreDAL;
using QuestionsAnswers.Entities;
using QuestionsAnswers.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace QuestionsAnswers.DAL
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 06 June 2018
    /// Description : DAL layer for Questions.
    /// </summary>
    public class QuestionsRepository : IQuestionsRepository
    {

        /// <summary>
        /// Created by : Snehal Dange on 7th June 2018
        /// Desc : To get all the filtered questions list for opr `manage questions`
        /// </summary>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public QuestionResult GetQuestions(QuestionsFilter questionFilters, byte startIndex, byte recordCount, byte applicationId)
        {
            QuestionResult result = null;
            IDictionary<string, Question> questionDictionary = null;
            try
            {
                if (questionFilters != null)
                {

                    using (DbCommand cmd = DbFactory.GetDBCommand("getquestions"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_tags", DbType.String, questionFilters.TagName));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_moderationstatus", DbType.UInt16, questionFilters.ModerationStatus));


                        if (questionFilters.EntryDate != DateTime.MinValue)
                        {
                            cmd.Parameters.Add(DbFactory.GetDbParam("par_startdate", DbType.DateTime, questionFilters.EntryDate));
                        }
                        else
                        {
                            cmd.Parameters.Add(DbFactory.GetDbParam("par_startdate", DbType.DateTime, DBNull.Value));
                        }

                        if (questionFilters.EntryDate != DateTime.MinValue)
                        {
                            cmd.Parameters.Add(DbFactory.GetDbParam("par_enddate", DbType.DateTime, questionFilters.EntryDate));
                        }
                        else
                        {
                            cmd.Parameters.Add(DbFactory.GetDbParam("par_enddate", DbType.DateTime, DBNull.Value));
                        }


                        cmd.Parameters.Add(DbFactory.GetDbParam("par_applicationid", DbType.Byte, applicationId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_startindex", DbType.Byte, startIndex));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_recordcount", DbType.Byte, recordCount));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customerEmail", DbType.String, questionFilters.CustomerEmails));


                        if (questionFilters.AnsweredStatus.HasValue)
                        {
                            cmd.Parameters.Add(DbFactory.GetDbParam("par_answerstatus", DbType.String, questionFilters.AnsweredStatus.Value ? 1 : 0));
                        }
                        else
                        {
                            cmd.Parameters.Add(DbFactory.GetDbParam("par_answerstatus", DbType.String, DBNull.Value));
                        }

                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (dr != null && dr.Read())
                            {
                                result = new QuestionResult();

                                result.TotalRecordCount = SqlReaderConvertor.ToInt32(dr["RecordCount"]);

                            }
                            if (result.TotalRecordCount > 0 && dr.NextResult())
                            {
                                questionDictionary = new Dictionary<string, Question>();
                                while (dr.Read())
                                {
                                    Question objQuestions = new Question()
                                    {
                                        Id = new Guid(Convert.ToString(dr["id"])),
                                        Text = Convert.ToString(dr["Text"]),
                                        AskedOn = SqlReaderConvertor.ToDateTime(dr["EntryDate"]),
                                        AskedBy = new Customer()
                                        {
                                            Id = SqlReaderConvertor.ToUInt32(dr["CustomerId"]),
                                            Name = Convert.ToString(dr["AskedBy"]),
                                            Email = Convert.ToString(dr["UserEMail"])
                                        },
                                        Status = (EnumModerationStatus)SqlReaderConvertor.ToUInt16(dr["status"]),
                                        AnswerCount = SqlReaderConvertor.ToUInt32(dr["answercount"])
                                    };

                                    questionDictionary.Add(Convert.ToString(objQuestions.Id), objQuestions);

                                }
                            }

                            if (questionDictionary != null && dr.NextResult())
                            {

                                while (dr.Read())
                                {
                                    string questionId = Convert.ToString(dr["qid"]);
                                    Tag tagObj = new Tag()
                                    {
                                        Id = SqlReaderConvertor.ToUInt32(dr["tagid"]),
                                        Name = Convert.ToString(dr["TagName"]),
                                    };

                                    if (questionDictionary.ContainsKey(questionId))
                                    {
                                        Question tempObj = questionDictionary[questionId];
                                        if (tempObj != null)
                                        {
                                            if (tempObj.Tags == null)
                                            {
                                                tempObj.Tags = new List<Tag>();
                                            }
                                            IList<Tag> a = (IList<Tag>)tempObj.Tags;
                                            a.Add(tagObj);
                                            tempObj.Tags = a;
                                        }

                                    }
                                }
                            }
                            IList<Question> quesList = null;
                            if (questionDictionary != null)
                            {
                                quesList = new List<Question>();
                                foreach (var ques in questionDictionary)
                                {
                                    quesList.Add(ques.Value);
                                }
                            }

                            result.QuestionList = quesList;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;

            }
            return result;
        }



        public bool ApproveQuestion(string questionId, uint userId)
        {
            bool isApproved = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "approvequestion";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_qid", DbType.String, questionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_moderator", DbType.UInt32, userId));
                    isApproved = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isApproved;
        }

        /// <summary>
        /// Created By : Deepak Israni on 11 June 2018
        /// Description: DAL method to store question in database.
        /// </summary>
        /// <param name="inputQuestion"></param>
        /// <returns></returns>
        public bool SaveQuestion(Question inputQuestion, ushort platformId, ushort applicationId, ushort sourceId)
        {
            bool success = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "savequestion";


                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_questiontext", DbType.String, inputQuestion.Text));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.UInt32, inputQuestion.AskedBy.Id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customeremailid", DbType.String, inputQuestion.AskedBy.Email));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customermaskingname", DbType.String, inputQuestion.AskedBy.Name));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_sourceid", DbType.UInt16, sourceId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_applicationid", DbType.UInt16, applicationId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_platformid", DbType.UInt16, platformId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_tags", DbType.String, String.Join(",", inputQuestion.Tags.Select(m => m.Name))));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_questionid", DbType.String, inputQuestion.Id));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                }

                success = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return success;
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 13 June 2018
        /// Description : DAL Method to Reject the question moderated by the internal user
        /// </summary>
        /// <param name="questionId">Id of the question to be approved</param>
        /// <param name="moderatorId">Id of the user rejecting the question from the OPR</param>
        /// <returns></returns>
        public bool RejectQuestion(string questionId, uint moderatorId, EnumRejectionReasons? rejectionReason)
        {
            bool isRejected = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "rejectquestion";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_qid", DbType.String, questionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_moderator", DbType.UInt32, moderatorId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_rejectionid", DbType.UInt16, rejectionReason));
                    isRejected = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isRejected;
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 14 June 2018
        /// Description : Function to Update Tags related to a particular question
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="moderatorId"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public bool UpdateQuestionTags(string questionId, uint moderatorId, List<uint> oldTags, List<string> newTags)
        {
            bool isUpdateSuccessful = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "updatequestiontags";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_qid", DbType.String, questionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_oldtags", DbType.String, string.Join(",", oldTags)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_newtags", DbType.String, string.Join(",", newTags)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_moderator", DbType.UInt32, moderatorId));
                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);

                    isUpdateSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isUpdateSuccessful;
        }


        /// <summary>
        /// CReated by : Snehal Dange on 19th June 2018
        /// Desc : Method created to save answer for a question
        /// </summary>
        /// <returns></returns>
        public bool SaveQuestionAnswer(Answer answerObj, ushort platformId, ushort sourceId)
        {
            bool isSavedSuccessfully = false;
            try
            {
                if (answerObj != null && !String.IsNullOrEmpty(answerObj.Text) && answerObj.AnsweredBy != null)
                {
                    Customer objCustomerBase = answerObj.AnsweredBy;
                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandText = "savequestionanswer";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_questionid", DbType.String, answerObj.QuestionId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_answertext", DbType.String, answerObj.Text));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.UInt32, objCustomerBase.Id));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customername", DbType.String, objCustomerBase.Name));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customeremail", DbType.String, objCustomerBase.Email));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_sourceid", DbType.UInt16, sourceId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_platformid", DbType.UInt16, platformId));
                        isSavedSuccessfully = MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase) > 0;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSavedSuccessfully;

        }

        /// <summary>
        /// Created by : Snehal Dange on 20th June 2018
        /// Desc : Increse answer count for questions table
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
                        cmd.CommandText = "increaseanswercount";
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_questionid", DbType.String, questionId));
                        isSuccess = MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSuccess;
        }

        /// <summary>
        /// Created By : Deepak Israni on 20 June 2018
        /// Description : DAL Method to get all the relevant question data based on Question Ids.
        /// </summary>
        /// <param name="questionIds"></param>  
        /// <returns></returns>
        public IEnumerable<Question> GetQuestionDataByQuestionIds(IEnumerable<string> questionIds)
        {

            IEnumerable<Question> questions = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getquestiondatabyquestionids";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_questionids", DbType.String, String.Join(",", questionIds)));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            IList<Question> _questions = new List<Question>();
                            Question question = null;

                            while (dr.Read())
                            {
                                question = new Question()
                                {
                                    Id = new Guid(Convert.ToString(dr["QId"])),
                                    Text = Convert.ToString(dr["QText"]),
                                    AskedOn = SqlReaderConvertor.ToDateTime(dr["EntryDate"]),
                                    AskedBy = new Customer()
                                    {
                                        Id = SqlReaderConvertor.ToUInt32(dr["QCustomerId"]),
                                        Name = Convert.ToString(dr["QCustomerName"]),
                                        Email = Convert.ToString(dr["QCustomerEmail"])
                                    }
                                };

                                _questions.Add(question);

                            }

                            if (dr.NextResult())
                            {
                                AnswerBase answer = null;
                                string associatedQuestion = null;

                                IDictionary<string, IList<AnswerBase>> questionAnswerMapping = new Dictionary<string, IList<AnswerBase>>();

                                while (dr.Read())
                                {
                                    answer = new AnswerBase()
                                    {
                                        Id = SqlReaderConvertor.ToUInt32(dr["AId"]),
                                        Text = Convert.ToString(dr["AText"]),
                                        AnsweredOn = SqlReaderConvertor.ToDateTime(dr["AnsweredOn"]),
                                        AnsweredBy = new Customer()
                                        {
                                            Id = SqlReaderConvertor.ToUInt32(dr["ACustomerId"]),
                                            Name = Convert.ToString(dr["ACustomerName"]),
                                            Email = Convert.ToString(dr["ACustomerEmail"])
                                        }
                                    };

                                    associatedQuestion = Convert.ToString(dr["QuestionId"]);

                                    if (questionAnswerMapping.ContainsKey(associatedQuestion))
                                    {
                                        questionAnswerMapping[associatedQuestion].Add(answer);
                                    }
                                    else
                                    {
                                        IList<AnswerBase> answers = new List<AnswerBase>();
                                        answers.Add(answer);
                                        questionAnswerMapping.Add(associatedQuestion, answers);
                                    }

                                }

                                foreach (Question ques in _questions)
                                {
                                    if (questionAnswerMapping.ContainsKey(ques.Id.ToString()))
                                    {
                                        ques.Answers = questionAnswerMapping[ques.Id.ToString()];
                                    }
                                }

                                questions = _questions;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return questions;
        }

        /// <summary>
        /// Created By : Deepak Israni on 21 June 2018
        /// Description : Get Question data based on a single question id.
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public IEnumerable<Question> GetQuestionDataByQuestionId(string questionId)
        {
            try
            {
                if (!String.IsNullOrEmpty(questionId))
                {
                    IList<string> input = new List<string>();
                    input.Add(questionId);
                    return GetQuestionDataByQuestionIds(input);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Created By : Deepak Israni on 9 July 2018
        /// Description : Overloaded function to save question which also stores the client ip.
        /// </summary>
        /// <param name="inputQuestion"></param>
        /// <param name="clientInfo"></param>
        /// <returns></returns>
        public bool SaveQuestion(Question inputQuestion, ClientInfo clientInfo)
        {
            bool success = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "savequestion_09072018";


                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_questiontext", DbType.String, inputQuestion.Text));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.UInt32, inputQuestion.AskedBy.Id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customeremailid", DbType.String, inputQuestion.AskedBy.Email));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customermaskingname", DbType.String, inputQuestion.AskedBy.Name));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_sourceid", DbType.UInt16, clientInfo.SourceId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_applicationid", DbType.UInt16, clientInfo.ApplicationId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_platformid", DbType.UInt16, clientInfo.PlatformId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbType.String, clientInfo.ClientIp));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_tags", DbType.String, String.Join(",", inputQuestion.Tags.Select(m => m.Name))));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_questionid", DbType.String, inputQuestion.Id));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                }

                success = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return success;
        }

        /// <summary>
        /// Created By : Deepak Israni on 10 July 2018
        /// Description : Overloaded function to save answer to a question along with the client ip.
        /// </summary>
        /// <param name="answerObj"></param>
        /// <param name="clientInfo"></param>
        /// <returns></returns>
        public bool SaveQuestionAnswer(Answer answerObj, ClientInfo clientInfo)
        {
            bool isSavedSuccessfully = false;
            try
            {
                if (answerObj != null && !String.IsNullOrEmpty(answerObj.Text) && answerObj.AnsweredBy != null)
                {
                    Customer objCustomerBase = answerObj.AnsweredBy;
                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandText = "savequestionanswer_09072018";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_questionid", DbType.String, answerObj.QuestionId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_answertext", DbType.String, answerObj.Text));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.UInt32, objCustomerBase.Id));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customername", DbType.String, objCustomerBase.Name));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customeremail", DbType.String, objCustomerBase.Email));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_sourceid", DbType.UInt16, clientInfo.SourceId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_platformid", DbType.UInt16, clientInfo.PlatformId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbType.String, clientInfo.ClientIp));
                        isSavedSuccessfully = MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase) > 0;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSavedSuccessfully;
        }
    }
}
