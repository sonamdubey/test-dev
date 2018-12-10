using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.CMS;
using System.Collections;
using Carwale.Entity;

namespace Carwale.Interfaces
{
    public interface ICMSContentRepository
    {
        // Interfaces for News , Tips and Advice , PitStop, AutoExpo.
        IList<CMSContentList> GetContentList(int startIndex, int endIndex, out int recordCount, OrderBy sortBy, ContentFilters filters, CMSAppId applicationId);
        CMSContentDetails GetContentDetails(int contentId,CMSAppId applicationId);
        void UpdateViews(int contentId);
        IList<VideosEntity> GetVideoList(uint basicId,CMSAppId applicationId);
        IList<PhotoGallery> GetPhotoGallery(string tag, CMSAppId applicationId);
        IList<CMSContentList> GetRelatedNews(string tag, int basicId,CMSAppId applicationId);
        // Interfaces for AutoExpo site (Get Filtered Content)
        //IList<CMSContentList> GetFilteredContent<ContentFilters>(int startIndex, int endIndex, out int recordCount, ContentFilters filters);
        IList<CMSContentList> GetTopFeaturedContent(int topRecords, OrderBy orderBy);

        // Interfaces for Expert Reviews,Features
        //Hashtable GetPages<TOut>(int contentId, ContentFilters filters) where TOut : Hashtable;
        Hashtable GetPages(int contentId);
       //PageContent GetPageContent(int contentId);
    }
}
