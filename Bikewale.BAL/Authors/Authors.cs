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
    /// <summary>
    /// Created by : Ashutosh Sharma on 20-Sep-2017
    /// Description :  Provide BAL methods related to authors.
    /// </summary>
    public class Authors : IAuthors
    {
        static readonly ILog _logger = LogManager.GetLogger(typeof(Authors));
        /// <summary>
        /// Created by : Ashutosh Sharma on 20-Sep-2017
        /// Description :  Method to get author list via GRPC.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AuthorEntityBase> GetAuthorsList()
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
