using Carwale.Entity.Enum;
using Carwale.Entity.UserReview;
using Carwale.Interfaces.UesrReview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Carwale.Notifications.Logs;
using NewCarConsumers;
using AEPLCore.Queue;
using AEPLCore.Utils.Serializer;
using System.Configuration;

namespace Carwale.DAL.UserReview 
{
    public class UserReviewRepository :RepositoryBase,IUserReviewRepository
    {
        public string SaveUserReview(UserReviewDetails userReviewDetails)
        {
            try
            {
                    string query=@"UPDATE cwexperience.customerreviews
                                   SET Title = @Title,Comments = @Description,
                                   StyleR=@ExteriorStyle,ComfortR=@Comfort,PerformanceR=@Performance,
                                   ValueR=@ValueForMoney,FuelEconomyR = @FuelEconomy
                                   WHERE Id = @Id;
                                   SELECT C.Name FROM cwexperience.customerreviews AS CR
                                   INNER JOIN cwmasterdb.customers AS C ON CR.CustomerId = C.Id
                                   WHERE CR.Id = @Id;";
                    using (var con = CarDataMySqlMasterConnection)
                    {
                        string customerName = con.Query<string>(query, userReviewDetails).SingleOrDefault();
                        return customerName;
                    }
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
                return string.Empty;
            }
        }

        public void SendUserReviewEmail(int reviewId, UserReviewStatus status)
        {
            if (reviewId > 0)
            {
                PublishManager publishManager = new PublishManager();
                ReviewRequest review = new ReviewRequest();
                review.ReviewId = reviewId;
                review.Status = (ReviewStatus)status;
                publishManager.PublishMessage(ConfigurationManager.AppSettings["UserReviewMailQueue"] ?? "senduserreviewemail",
                new QueueMessage
                {
                    InputParameterName = "ReviewRequest",
                    DeadletterCount = 0,
                    FunctionName = "SendUserReviewEmail",
                    ModuleName = ConfigurationManager.AppSettings["NewCarConsumerModuleName"] ?? "newcarconsumers",
                    Payload = Serializer.ConvertProtobufMsgToBytes(review)
                });
            }
        }
    }
}
