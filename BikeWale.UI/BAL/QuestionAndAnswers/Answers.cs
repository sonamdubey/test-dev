using Bikewale.Automappers.QuestionAndAnswers;
using Bikewale.Entities.Customer;
using Bikewale.Entities.QuestionAndAnswers;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.QuestionAndAnswers;
using Bikewale.Notifications;
using System;
using System.Collections.Specialized;
using System.Web;

namespace Bikewale.BAL.QuestionAndAnswers
{
    /// <summary>
    /// Created by : Snehal Dange on 11th July 2018
    /// Desc : Class created to handle answer flow through bikewale site
    /// </summary>
    public class Answers : IAnswers
    {

        private readonly ICustomerAuthentication<CustomerEntity, UInt32> _objAuthCustomer = null;
        private readonly ICustomer<CustomerEntity, UInt32> _objCustomer = null;
        private readonly QuestionsAnswers.BAL.IQuestions _objQNAQuestions = null;
        private readonly IAnswerRepository _objAnswerRepo = null;
        private readonly uint _answerMaxLength = 10000;

        /// <summary>
        /// Constuctor
        /// </summary>
        public Answers(ICustomerAuthentication<CustomerEntity, UInt32> objAuthCustomer, ICustomer<CustomerEntity, UInt32> objCustomer, QuestionsAnswers.BAL.IQuestions objQNAQuestions, IAnswerRepository objAnswerRepo)
        {
            _objAuthCustomer = objAuthCustomer;
            _objCustomer = objCustomer;
            _objQNAQuestions = objQNAQuestions;
            _objAnswerRepo = objAnswerRepo;
        }

        /// <summary>
        /// Created by : Snehal Dange on 11th July 2018
        /// Desc :  Function created to save the answer 
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="answer"></param>
        /// <param name="clientInfo"></param>
        /// <returns></returns>
        private bool SaveAnswer(Bikewale.Entities.QuestionAndAnswers.Answer answer, BWClientInfo clientInfo, string questionId)
        {
            bool status = false;
            try
            {
                answer.Text = StringHtmlHelpers.RemoveHtmlWithSpaces(answer.Text);
                if (!string.IsNullOrEmpty(answer.Text))
                {
                    QuestionsAnswers.Entities.ClientInfo qnaClientInfo = null;
                    QuestionsAnswers.Entities.Answer qnaAnswer = null;
                    if (clientInfo != null)
                    {
                        qnaClientInfo = QuestionAndAnswersMappper.Convert(clientInfo);
                    }
                    CustomerEntityBase objAnsweredBy = answer.AnsweredBy;
                    if (objAnsweredBy != null)
                    {
                        objAnsweredBy = GetCustomerEntity(objAnsweredBy);
                        if (objAnsweredBy.CustomerId > 0 && answer.Text.Length <= _answerMaxLength)
                        {
                            qnaAnswer = ConvertBikeWaleAnswerToQNAAnswer(answer, questionId);
                            if (qnaAnswer.AnsweredBy != null && qnaClientInfo != null)
                            {
                                status = _objQNAQuestions.SaveQuestionAnswer(qnaAnswer, qnaClientInfo);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.QuestionAndAnswers.Answers.SaveAnswer");
            }
            return status;
        }

        /// <summary>
        /// Created By : Snehal Dange on 11 July 2018
        /// Description: Checks if customer exists and if not creates a new customer entity.
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="mobile"></param>
        /// <param name="email"></param>
        /// <returns></returns>

        private CustomerEntityBase GetCustomerEntity(CustomerEntityBase objCustomer)
        {
            CustomerEntity objCust = null;
            if (!_objAuthCustomer.IsRegisteredUser(objCustomer.CustomerEmail, objCustomer.CustomerMobile))
            {
                objCust = new CustomerEntity() { CustomerName = objCustomer.CustomerName, CustomerEmail = objCustomer.CustomerEmail, CustomerMobile = objCustomer.CustomerMobile, ClientIP = Bikewale.Utility.CurrentUser.GetClientIP() };
                objCustomer.CustomerId = objCust.CustomerId = _objCustomer.Add(objCust);
            }
            else
            {
                objCust = _objCustomer.GetByEmailMobile(objCustomer.CustomerEmail, objCustomer.CustomerMobile);

                objCust.CustomerName = objCustomer.CustomerName;
                objCust.CustomerEmail = !String.IsNullOrEmpty(objCustomer.CustomerEmail) ? objCustomer.CustomerEmail : objCust.CustomerEmail;
                objCust.CustomerMobile = objCustomer.CustomerMobile;
                objCustomer.CustomerId = objCust.CustomerId;
                _objCustomer.Update(objCust);
            }
            return objCust;
        }

        /// <summary>
        /// CReated by : Snehal Dange on 17th July 2018
        /// Desc : Mapper to convert bikewale answer entity to qna answer entity
        /// </summary>
        /// <returns></returns>
        private QuestionsAnswers.Entities.Answer ConvertBikeWaleAnswerToQNAAnswer(Bikewale.Entities.QuestionAndAnswers.Answer answer, string questionId)
        {
            QuestionsAnswers.Entities.Answer qnaAnswer = null;
            try
            {

                qnaAnswer = new QuestionsAnswers.Entities.Answer();
                qnaAnswer.Text = answer.Text;
                qnaAnswer.QuestionId = questionId;
                CustomerEntityBase bwCust = answer.AnsweredBy;
                if (bwCust != null)
                {
                    QuestionsAnswers.Entities.Customer qnaCustomer = new QuestionsAnswers.Entities.Customer();
                    qnaCustomer.Id = Convert.ToUInt32(bwCust.CustomerId);
                    qnaCustomer.Name = bwCust.CustomerName;
                    qnaCustomer.Email = bwCust.CustomerEmail;

                    qnaAnswer.AnsweredBy = qnaCustomer;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.QuestionAndAnswers.Answers.ConvertBikeWaleAnswersToQNAAnswers()");

            }
            return qnaAnswer;
        }



        /// <summary>
        /// Created by : Snehal Dange on 19th July 2018
        /// Desc :  Check if the user has already submitted the answer.
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool CheckDuplicateAnswerByUser(string questionId, uint customerId)
        {
            bool isDuplicate = false;
            try
            {
                isDuplicate = _objAnswerRepo.CheckDuplicateAnswerByUser(questionId, customerId);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Models.QuestionsAnswers.Answers.CheckDulpicateAnswerByUser() Question Id: {0}, Customer  Id: {1}.", questionId, customerId));
            }
            return isDuplicate;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 03 Sep 2018
        /// Description :   Submits the User answers for a question
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="answerText"></param>
        /// <param name="userName"></param>
        /// <param name="userEmail"></param>
        /// <param name="clientInfo"></param>
        /// <returns></returns>
        public bool SubmitUserAnswer(string questionId, string answerText, string userName, string userEmail, BWClientInfo clientInfo)
        {
            bool saveStatus = false;
            try
            {
                CustomerEntityBase objCust = new CustomerEntityBase();
                objCust.CustomerName = userName;
                objCust.CustomerEmail = userEmail;
                Bikewale.Entities.QuestionAndAnswers.Answer answerObj = new Bikewale.Entities.QuestionAndAnswers.Answer();
                answerObj.AnsweredBy = objCust;
                answerObj.Text = answerText;
                saveStatus = SaveAnswer(answerObj, clientInfo, questionId);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.QuestionAndAnswers.Answers.SubmitUserAnswer()");

            }
            return saveStatus;
        }
    }
}
