using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.QuestionAndAnswers;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.QuestionAndAnswers;
using Bikewale.Models.QuestionAndAnswers;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Bikewale.Models.QuestionsAnswers
{
    public class ThankYouPageModel
    {
        #region Private Variables
        private readonly IQuestions _questions;
        private readonly IBikeInfo _bikeInfo;

        private readonly int questionCount = 10;
        #endregion

        #region Public Properties
        public bool IsMobile { get; set; }
        public Entities.QuestionAndAnswers.Sources Source { get; set; }
        public string ReturnUrl { get; set; }
        #endregion

        #region Constructor
        public ThankYouPageModel(IQuestions questions, IBikeInfo bikeInfo)
        {
            _questions = questions;
            _bikeInfo = bikeInfo;
        }
        #endregion

        #region Functions

        /// <summary>
        /// Created By : Deepak Israni on 13 July 2018
        /// Description : Function to bind data on Thank You page after question is answered.
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public ThankYouPageVM GetData(string queryString)
        {
            ThankYouPageVM objData = new ThankYouPageVM();

            try
            {
                string decodedString = Bikewale.Utility.TripleDES.DecryptTripleDES(queryString);

                NameValueCollection queryCollection = HttpUtility.ParseQueryString(decodedString);
                uint modelId;

                if (!String.IsNullOrEmpty(this.ReturnUrl))
                {
                    objData.ReturnUrl = ReturnUrl;
                }
                if(this.Source != Sources.Invalid)
                {
                    objData.Source = (ushort)this.Source;
                }

                #region Bind Question Data

                if (UInt32.TryParse(queryCollection["modelId"], out modelId))
                {
                    GenericBikeInfo bikeData = _bikeInfo.GetBikeInfo(modelId, 1);

                    if (bikeData != null && bikeData.Make != null && bikeData.Model != null)
                    {
                        objData.BikeName = String.Format("{0} {1}", bikeData.Make.MakeName, bikeData.Model.ModelName);
                        objData.MakeName = bikeData.Make.MakeName;
                        objData.ModelName = bikeData.Model.ModelName;
                    }
                    else
                    {
                        return objData;
                    }

                    IEnumerable<Question> qData = _questions.GetRemainingUnansweredQuestions(modelId, questionCount, queryCollection["userEmail"]);

                    if (qData != null)
                    {
                        ICollection<QuestionUrl> questionData = new Collection<QuestionUrl>();
                        QuestionUrl questionItem = null;
                        foreach (Question question in qData)
                        {
                            questionItem = new QuestionUrl();
                            questionItem.QuestionData = question;
                            string q = Bikewale.Utility.TripleDES.EncryptTripleDES(string.Format(@"userEmail={0}&userName={1}&questionId={2}&modelId={3}", queryCollection["userEmail"], queryCollection["userName"], question.Id, queryCollection["modelId"]));

                            questionItem.AnsweringUrl = IsMobile ?
                                String.Format("{0}/m/questions-and-answers/answer/?q={1}", BWConfiguration.Instance.BwHostUrl, q)
                                : questionItem.AnsweringUrl = String.Format("{0}/questions-and-answers/answer/?q={1}", BWConfiguration.Instance.BwHostUrl, q);

                            if (this.Source != Sources.Invalid)
                            {
                                questionItem.AnsweringUrl = String.Format("{0}&source={1}", questionItem.AnsweringUrl,(ushort)Source);
                            }
                            if (!String.IsNullOrEmpty(this.ReturnUrl))
                            {
                                questionItem.AnsweringUrl = String.Format("{0}&returnUrl={1}", questionItem.AnsweringUrl, ReturnUrl);
                            }
                            questionData.Add(questionItem);
                        }

                        objData.Questions = questionData;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.QuestionsAnswers.ThankYouPageModel.GetData");
            }


            return objData;
        }

        #endregion

    }
}