using MySql.CoreDAL;
using QuestionsAnswers.Entities;
using QuestionsAnswers.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionsAnswers.DAL
{
    public class AnswersRepository : IAnswersRepository
    {

        /// <summary>
        /// Created by  : Sanskar Gupta on 19 June 2018
        /// Description : Function to get all the answers for a particular question
        /// </summary>
        /// <param name="questionId">ID of the question for which answers will be retrieved.</param>
        /// <returns></returns>
        public IEnumerable<Answer> GetAnswers(string questionId)
        {
            IList<Answer> answers = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getanswers"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_qid", DbType.String, questionId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            answers = new List<Answer>();
                            while (dr.Read())
                            {
                                Answer answer = new Answer();
                                answer.Id = Convert.ToUInt32(dr["answerid"]);
                                answer.Text = Convert.ToString(dr["answertext"]);
                                answer.AnsweredBy = new Customer()
                                {
                                    Id = SqlReaderConvertor.ParseToUInt32(dr["customerid"]),
                                    Name = Convert.ToString(dr["customername"]),
                                    Email = Convert.ToString(dr["customeremail"])
                                };
                                answer.AnsweredOn = SqlReaderConvertor.ToDateTime(dr["answeredon"]);
                                answer.QuestionId = questionId;
                                
                                answers.Add(answer);
                            }

                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return answers;
        }
    }
}
