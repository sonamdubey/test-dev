using AutoMapper;
using Carwale.BL.GrpcFiles;
using Carwale.Entity.Author;
using Carwale.Entity.CMS;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Author;
using Carwale.Notifications;
using Grpc.CMS;
using System;
using System.Collections.Generic;

namespace Carwale.BL.Authors
{
    public class AuthorsBL : IAuthorRepository
    {
        public List<AuthorList> GetAllOtherAuthors(int authorId)
        {
            try
            {
                return GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetAllOtherAuthors(authorId, (int)Application.CarWale));
            }
            catch (Exception ex)
            {
                ErrorClass obj = new ErrorClass(ex, "Carwale.BL.Authors.AuthorsBL GetAllOtherAuthors()");
                obj.SendMail();
                return new List<AuthorList>();
            }
        }

        public AuthorEntity GetAuthorDetails(int authorId)
        {
            try
            {
                return GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetAuthorDetails(authorId));
            }
            catch (Exception ex)
            {
                ErrorClass obj = new ErrorClass(ex, "Carwale.BL.Authors.AuthorsBL GetAuthorDetails()");
                obj.SendMail();
                return new AuthorEntity();
            }
        }

        public List<AuthorList> GetAuthorsList()
        {
            try
            {
                return GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetAuthorsList((int)Application.CarWale));
            }
            catch (Exception ex)
            {
                ErrorClass obj = new ErrorClass(ex, "Carwale.BL.Authors.AuthorsBL GetAuthorsList()");
                obj.SendMail();
                return new List<AuthorList>();
            }           
        }
                
        public List<ExpertReviews> GetExpertReviewsByAuthor(int authorId, CMSAppId applicationId)
        {
            try
            {
                return GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetContentByAuthor(authorId, Convert.ToInt16(applicationId), string.Format("{0},{1}", (int)CMSContentType.ComparisonTests, (int)CMSContentType.RoadTest)));
            }
            catch (Exception ex)
            {
                ErrorClass obj = new ErrorClass(ex, "Carwale.BL.Authors.AuthorsBL GetExpertReviewsByAuthor()");
                obj.SendMail();
                return new List<ExpertReviews>();
            }
        }

        public List<NewsEntity> GetNewsByAuthor(int authorId, CMSAppId applicationId)
        {
            try
            {
                return Mapper.Map<List<ExpertReviews>, List<NewsEntity>>(GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetContentByAuthor(authorId,Convert.ToInt16(applicationId), string.Format("{0}",(int)CMSContentType.News))));
            }
            catch (Exception ex)
            {
                ErrorClass obj = new ErrorClass(ex, "Carwale.BL.Authors.AuthorsBL GetNewsByAuthor()");
                obj.SendMail();
                return new List<NewsEntity>();
            }
        }
    }
}
