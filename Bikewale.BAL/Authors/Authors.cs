using Bikewale.BAL.GrpcFiles;
using Bikewale.Entities.Authors;
using Bikewale.Entities.CMS.Articles;
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
        public IEnumerable<AuthorEntityBase> GetAuthorsList(int applicationId)
        {
            try
            {
                var _objGrpcAuthorsList = GrpcMethods.GetAuthorsList(applicationId);
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

        /// <summary>
        /// Created by : Vivek Singh Tomar on 20th Sep 2017
        /// Summary : Get Author Details 
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        public AuthorEntity GetAuthorDetailsViaGrpc(int authorId)
        {
            AuthorEntity objAuthor = null;
            try
            {
                var author = GrpcMethods.GetAuthorDetails(authorId);
                if(author != null)
                {
                    objAuthor = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(author);
                }
            }catch(Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return objAuthor;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 20th Sep 2017
        /// Summary : Get Articles by author
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="applicationId"></param>
        /// <param name="categoryList"></param>
        /// <returns></returns>
        public IEnumerable<ArticleSummary> GetArticlesByAuthorViaGrpc(int authorId, int applicationId, string categoryList)
        {
            IEnumerable<ArticleSummary> objArticlesList = null;
            try
            {
                var articles = GrpcMethods.GetContentByAuthor(authorId, applicationId, categoryList);
                if(articles != null)
                {
                    objArticlesList = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(articles);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return objArticlesList;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 20th Sep 2017
        /// Summary : Get List of all other authors
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public IEnumerable<AuthorEntityBase> GetAllOtherAuthors(int authorId, int applicationId)
        {
            IEnumerable<AuthorEntityBase> objAuthorList = null;
            try
            {
                var _objGrpcAuthorsList = GrpcMethods.GetAllOtherAuthors(authorId, applicationId);
                if (_objGrpcAuthorsList != null && _objGrpcAuthorsList.CalculateSize() > 0)
                {
                    objAuthorList = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcAuthorsList);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return objAuthorList;
        }
    }
}
