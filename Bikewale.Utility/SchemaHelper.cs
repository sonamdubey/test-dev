using Bikewale.Entities.Schema;
using Newtonsoft.Json.Linq;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 31-Aug-2017
    /// Summary: Created Helper class for Schema
    /// 
    /// </summary>
    public static class SchemaHelper
    {
        private static string _WebPage = "WebPage";
        public const string QAPage = "QAPage";
        public static string JsonSerialize(dynamic objSchema)
        {
            try
            {
                JObject jsonObj = JObject.FromObject(objSchema);
                jsonObj.Add("@context", "http://schema.org");
                return jsonObj.ToString(Newtonsoft.Json.Formatting.None);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12th Sep 2017
        /// Description : Overload method to page level schema into webpage schema
        /// </summary>
        /// <param name="objSchema"></param>
        /// <param name="pageSchema"></param>
        /// <returns></returns>
        public static string JsonSerialize(dynamic objSchema, dynamic pageSchema)
        {
            try
            {
                JObject jsonObj = JObject.FromObject(objSchema);
                jsonObj.Add("@context", "http://schema.org");
                jsonObj.Add("mainEntity", JObject.FromObject(pageSchema));
                return jsonObj.ToString(Newtonsoft.Json.Formatting.None);
            }
            catch
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// Created By : Sushil Kumar on 12th Sep 2017
        /// Description : Function to return webpage schema json 
        /// </summary>
        /// <param name="objSchema"></param>
        /// <param name="pageSchema"></param>
        /// <returns></returns>
        public static WebPage GetWebpageSchema(Models.PageMetaTags objPageMeta, BreadcrumbList breadcrumb)
        {
            WebPage webpage = null;
            if (objPageMeta != null && breadcrumb != null)
            {

                try
                {
                    //set webpage schema for the model page
                    webpage = new WebPage();
                    webpage.Type = _WebPage;
                    webpage.Description = objPageMeta.Description;
                    webpage.Keywords = objPageMeta.Keywords;
                    webpage.Title = objPageMeta.Title;
                    webpage.Url = objPageMeta.CanonicalUrl;
                    webpage.Breadcrum = breadcrumb;

                }
                catch
                {
                    webpage = null;
                }
            }

            return webpage;
        }

        /// <summary>
        /// Created By : Sumit Kate on 28 Jun 2018
        /// Description : Function to return webpage schema json with custom pageType
        /// </summary>
        /// <param name="objSchema"></param>
        /// <param name="pageSchema"></param>
        /// <param name="pageType"></param>
        /// <returns></returns>
        public static WebPage GetWebpageSchema(Models.PageMetaTags objPageMeta, BreadcrumbList breadcrumb, string pageType)
        {
            WebPage webpage = null;
            if (objPageMeta != null && breadcrumb != null)
            {

                try
                {
                    webpage = GetWebpageSchema(objPageMeta, breadcrumb);
                    webpage.Type = !string.IsNullOrEmpty(pageType) ? pageType : webpage.Type;
                }
                catch
                {
                    webpage = null;
                }
            }

            return webpage;
        }


        /// <summary>
        /// Created By : Sushil Kumar on 12th Sep 2017
        /// Description : Function to set breadcrumb item
        /// </summary>
        /// <param name="position"></param>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static BreadcrumbListItem SetBreadcrumbItem(ushort position, string url, string name)
        {
            if (position > 0 && !string.IsNullOrEmpty(name))
            {

                try
                {
                    return new BreadcrumbListItem
                    {
                        Position = position,
                        Item = new BreadcrumbItem()
                        {
                            Url = url,
                            Name = name
                        }
                    };

                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

    }
}
