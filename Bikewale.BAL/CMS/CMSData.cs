using Bikewale.DAL.CMS;
using Bikewale.Entities.CMS;
using Bikewale.Interfaces.CMS;
using Microsoft.Practices.Unity;
using System.Collections.Generic;

namespace Bikewale.BAL.CMS
{
    public class CMSData<T,V> : ICMSContentRepository<T,V> where T : CMSContentListEntity, new()
                                                           where V : CMSPageDetailsEntity, new()
    {
        private ICMSContentRepository<T, V> _contentRepo;

        public CMSData(EnumCMSContentType contentType)
        {            
            using (var container = new UnityContainer())
            {                
                switch (contentType)
                {
                    case EnumCMSContentType.News:
                        container.RegisterType<ICMSContentRepository<T, V>, NewsRepository<T, V>>();
                        break;
                    case EnumCMSContentType.ComparisonTests:
                        break;
                    case EnumCMSContentType.BuyingUsed:
                        break;
                    case EnumCMSContentType.ABIBlog:
                        break;
                    case EnumCMSContentType.TipsAndAdvices:
                        break;
                    case EnumCMSContentType.Features:
                        container.RegisterType<ICMSContentRepository<T, V>, FeaturesRepository<T, V>>();
                        break;
                    case EnumCMSContentType.Products:
                        break;
                    case EnumCMSContentType.RoadTest:
                        container.RegisterType<ICMSContentRepository<T, V>, RoadTestRepository<T, V>>();
                        break;
                    case EnumCMSContentType.AutoExpo2012:
                        break;
                    case EnumCMSContentType.PhotoGalleries:
                        break;
                    case EnumCMSContentType.Videos:
                        break;
                    case EnumCMSContentType.AutoExpo2014:
                        break;
                    case EnumCMSContentType.AutoExpoMedia:
                        break;
                    default:
                        break;
                }

                _contentRepo = container.Resolve<ICMSContentRepository<T, V>>();
            }
        }

        public IList<T> GetContentList(int startIndex, int endIndex, out int recordCount, ContentFilter filters)
        {
            recordCount = 0;

            return _contentRepo.GetContentList(startIndex, endIndex, out recordCount, filters);
        }

        public V GetContentDetails(int contentId, int pageId)
        {
            return _contentRepo.GetContentDetails(contentId, pageId);
        }

        public void UpdateViews(int contentId)
        {
            _contentRepo.UpdateViews(contentId);
        }

        public List<CMSFeaturedArticlesEntity> GetMostRecentArticles(List<EnumCMSContentType> contentTypes, ushort totalRecords)
        {
            return _contentRepo.GetMostRecentArticles(contentTypes, totalRecords);
        }


        public List<CMSFeaturedArticlesEntity> GetFeaturedArticles(List<EnumCMSContentType> contentTypes, ushort totalRecords)
        {
            return _contentRepo.GetFeaturedArticles(contentTypes, totalRecords);
        }
    }
}
