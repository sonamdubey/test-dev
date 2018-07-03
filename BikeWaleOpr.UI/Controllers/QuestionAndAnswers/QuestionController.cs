using System.Web.Mvc;

namespace BikewaleOpr.Controllers.QuestionAndAnswers
{
    /// <summary>
    /// Created by : Snehal Dange on 7th June 2018
    /// Description: Question Controller for QnA mapping
    /// </summary>
    public class QuestionController : Controller
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public QuestionController()
        {

        }



        /// <summary>
        /// Created by : Snehal Dange on 7th June 2018
        /// Desc : ActionMethod to bind questions and filters on opr
        /// </summary>
        /// <returns></returns>
        [Route("questions/")]
        public ActionResult Index()
        {
            return null;
        }
    }
}