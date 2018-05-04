using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.Notifications;
using Bikewale.Utility;
using SpamFilter.Service.ProtoClass;

namespace Bikewale.BAL.ApiGateway.Adapters.SpamFilter
{
    public class GetScoreAdapter : AbstractApiGatewayAdapter<Bikewale.Entities.Customer.CustomerEntityBase, Bikewale.BAL.ApiGateway.Entities.SpamFilter.SpamScore, SpamScore>
    {
        public GetScoreAdapter()
        {
            ModuleName = BWConfiguration.Instance.SpamFilterServiceModuleName;
            MethodName = "GetScore";

        }
        protected override Google.Protobuf.IMessage BuildRequest(Bikewale.Entities.Customer.CustomerEntityBase input)
        {
            if (input == null)
            {
                return null;
            }
            try
            {
                UserInfo request = new UserInfo();
                request.Name = input.CustomerName;
                request.Number = input.CustomerMobile;
                request.Email = input.CustomerEmail;
                return request;
            }
            catch (System.Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.ApiGateway.Adapters.SpamFilter.BuildRequest");
                return null;
            }
        }

        protected override Entities.SpamFilter.SpamScore BuildResponse(Google.Protobuf.IMessage responseMessage)
        {
            Entities.SpamFilter.SpamScore spamScore = null;
            try
            {
                SpamScore grpcResponse = responseMessage as SpamScore;
                if (responseMessage == null)
                {
                    return null;
                }
                else
                {
                    spamScore = new Entities.SpamFilter.SpamScore();
                    if (grpcResponse.Email != null)
                    {
                        spamScore.Email = new Entities.SpamFilter.ItemScore()
                {
                    Description = grpcResponse.Email.Description,
                    Score = grpcResponse.Email.Score
                };
                    }
                    if (grpcResponse.Name != null)
                    {
                        spamScore.Name = new Entities.SpamFilter.ItemScore()
                                    {
                                        Description = grpcResponse.Name.Description,
                                        Score = grpcResponse.Name.Score
                                    };
                    }
                    if (grpcResponse.Number != null)
                    {
                        spamScore.Number = new Entities.SpamFilter.ItemScore()
                                    {
                                        Description = grpcResponse.Number.Description,
                                        Score = grpcResponse.Number.Score
                                    };
                    }
                    spamScore.Score = grpcResponse.Score;
                }
            }
            catch (System.Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.ApiGateway.Adapters.SpamFilter.BuildResponse");
            }
            return spamScore;
        }
    }
}
