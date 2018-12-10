using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.CMS.Articles;

namespace Carwale.Interfaces.CMS.Articles
{
    public interface ICMSContentCache
    {
        /// <summary>
        /// Function to get list of articles by category id.
        /// </summary>
        /// <typeparam name="ArticleSummary"></typeparam>
        /// <param name="application">type of application. e.g. bikewale or carwale</param>
        /// <param name="categotyId">Category of article. e.g. news roadtest, features etc.</param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="recordCount">Returns total no of records</param>
        /// <param name="filters">If any filters are required to filter the article.</param>
        /// <returns></returns>
        IList<ArticleSummary> GetContentListByCategory(ushort application, ushort categotyId, uint startIndex, uint endIndex, out uint recordCount, ContentFilter filters);

        /// <summary>
        /// Function to get the list of articles by subcategory. e.g. tips and advices.
        /// </summary>
        /// <typeparam name="ArticleSummary"></typeparam>
        /// <param name="application">type of application. e.g. bikewale or carwale</param>
        /// <param name="subCategotyId"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="recordCount">Returns total no of records</param>
        /// <param name="filters">If any filters are required to filter the article.</param>
        /// <returns></returns>
        IList<ArticleSummary> GetContentListBySubCategory(ushort application, ushort subCategotyId, uint startIndex, uint endIndex, out uint recordCount, ContentFilter filters);

        /// <summary>
        /// Function to get details of the article having single page by basic id. e.g. news
        /// </summary>
        /// <typeparam name="ArticleDetails"></typeparam>
        /// <param name="basicId"></param>
        /// <returns></returns>
        ArticleDetails GetContentDetails(ulong basicId);

        /// <summary>
        /// Function to get details of the articles having multiple pages by basic id. e.g. roadtest.
        /// </summary>
        /// <typeparam name="ArticlePageDetails"></typeparam>
        /// <param name="basicId"></param>
        /// <returns></returns>
        ArticlePageDetails GetContentPages(ulong basicId);


        /// <summary>
        /// Function to get the list of featured articles. It can be one or multiple type of articles depending on contenttypes passed to the function.
        /// </summary>
        /// <typeparam name="ArticleSummary"></typeparam>
        /// <param name="application"></param>
        /// <param name="contentTypes">list containing type of content. e.g. news roadtest. Data returned will be of given content type.</param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        List<ArticleSummary> GetFeaturedArticles(ushort application, List<ushort> contentTypes, ushort totalRecords);

        /// <summary>
        /// Function to get the list of recent articles. It can be one or multiple type of articles depending on contenttypes passed to the function.
        /// </summary>
        /// <typeparam name="ArticleSummary"></typeparam>
        /// <param name="application"></param>
        /// <param name="contentTypes">list containing type of content. e.g. news roadtest. Data returned will be of given content type.</param>
        /// <param name="totalRecords"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        List<ArticleSummary> GetMostRecentArticles(ushort application, List<ushort> contentTypes, ushort totalRecords, ContentFilter filters);


        /// <summary>
        /// Function to get the list of related articles. It can be one or multiple type of articles depending on contenttypes passed to the function.
        /// </summary>
        /// <typeparam name="ArticleSummary"></typeparam>
        /// <param name="application"></param>
        /// <param name="tag"></param>
        /// <param name="contentTypes">List containing type of content. e.g. news roadtest. Data returned will be of given content type.</param>
        /// <param name="Tags"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        List<ArticleSummary> GetRelatedArticles(ushort application, List<ushort> contentTypes, List<Tag> Tags, ushort totalRecords);

    }   // interface
}   // namespace
