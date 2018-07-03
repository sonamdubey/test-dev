using BikewaleOpr.Entity.QnA;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.QuestionsAnswers;
using BikewaleOpr.Models.QuestionAndAnswer;
using BikewaleOpr.Models.QuestionsAndAnswers;
using QuestionsAnswers.BAL;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    [Authorize]
    public class QuestionsAndAnswersController : Controller
    {

        private readonly IBikeMakesRepository _makesRepo = null;
        private readonly IQuestions _questions = null;
        private readonly IQuestionsAndAnswersCache _questionAnswerCache = null;
        /// <summary>
        /// Constructor
        /// </summary>
        public QuestionsAndAnswersController(IBikeMakesRepository makesRepo, IQuestions questions, IQuestionsAndAnswersCache questionAnswerCache)
        {
            _makesRepo = makesRepo;
            _questions = questions;
            _questionAnswerCache = questionAnswerCache;

        }

        public ActionResult ManageQuestions(QnAFilters filtersObj)
        {
            QuestionAnswerModel qnaModel = new QuestionAnswerModel(_makesRepo, _questions, _questionAnswerCache);
            ManageQuestionsVM objManageQuestions = qnaModel.GetData(filtersObj);
            return View("~/Views/QuestionsAndAnswers/QnABase.cshtml", objManageQuestions);
        }
    }
}