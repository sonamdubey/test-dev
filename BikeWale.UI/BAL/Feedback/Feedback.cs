using Bikewale.DAL.Feedback;
using Bikewale.Interfaces.Feedback;
using Microsoft.Practices.Unity;

namespace Bikewale.BAL.Feedback
{
    public class Feedback : IFeedback
    {
        private readonly IFeedback _feedbackRepository = null;
        public Feedback()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IFeedback, FeedbackRepository>();
                _feedbackRepository = container.Resolve<IFeedback>();
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 21 Jan 2015
        /// Summary : To Save Customer feedback
        /// </summary>
        /// <param name="feedbackComment"></param>
        /// <param name="feedbackType"></param>
        /// <param name="platformId"></param>
        /// <returns></returns>
        public bool SaveCustomerFeedback(string feedbackComment, ushort feedbackType, ushort platformId,string pageUrl)
        {
            bool isSaved = false;

            isSaved = _feedbackRepository.SaveCustomerFeedback(feedbackComment, feedbackType, platformId, pageUrl);

            return isSaved;
        }   //End of SaveCustomerFeedback
    }   //End of class
}   //End of namespace
