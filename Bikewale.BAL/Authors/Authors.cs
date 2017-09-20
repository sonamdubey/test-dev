using Bikewale.BAL.GrpcFiles;
using Bikewale.Entities.Authors;
using Bikewale.Interfaces.Authors;
using Grpc.CMS;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Bikewale.BAL.Authors
{
    public class Authors : IAuthors
    {
        static readonly ILog _logger = LogManager.GetLogger(typeof(Authors));
        public IEnumerable<AuthorEntityBase> GetAuthorsListViaGrpc()
        {
            try
            {
                var _objGrpcAuthorsList = GrpcMethods.GetAuthorsList(2);
                if (_objGrpcAuthorsList != null && _objGrpcAuthorsList.CalculateSize() > 0)
                {
                    return GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcAuthorsList);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return Enumerable.Empty<AuthorEntityBase>();
        }
    }
}
