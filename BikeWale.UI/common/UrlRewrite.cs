using System.Text.RegularExpressions;
// Mails Class
//
using System.Web;

namespace Bikewale.Common
{
    public class UrlRewrite
    {
        // will format the a name to make it URL compatible 
        //for accessories only
        public static string FormatURL(string url)
        {
            string reg = @"[^0-9a-zA-Z\s]*"; // everything except a-z and 0-9

            url = Regex.Replace(url, reg, "", RegexOptions.IgnoreCase);

            return url.Replace(" ", "").ToLower();
        }

        public static string FormatSpecial(string url)
        {
            string reg = @"[^/\-0-9a-zA-Z\s]*"; // everything except a-z, 0-9, / and -

            url = Regex.Replace(url, reg, "", RegexOptions.IgnoreCase);

            return url.Replace(" ", "").Replace("-", "").Replace("/", "").ToLower();
        }

        public static string GenerateMakeLink(string make)
        {
            return "<a title=\"" + make + " bikes in India\" href=\"" + GetMakeUrl(make) + "\">" + make + "</a>";
        }

        public static string GetMakeUrl(string make)
        {
            return "/content/bikes/" + UrlRewrite.FormatSpecial(make) + "/";
        }

        public static string GenerateModelLink(string make, string model)
        {
            return "<a title=\"" + make + " " + model + " bikes in India\" href=\"" + GetModelUrl(make, model) + "\">" + make + " " + model + "</a>";
        }

        public static string GetModelUrl(string make, string model)
        {
            return "/content/bikes/" + UrlRewrite.FormatSpecial(make) + "/" + UrlRewrite.FormatSpecial(model) + "/";
        }

        public static string GenerateBodyTypeLink(string bodyType)
        {
            return "<a title=\"" + bodyType + " bikes in India\" href=\"" + GetBodyTypeUrl(bodyType) + "\">" + bodyType + "</a>";
        }

        public static string GetBodyTypeUrl(string bodyType)
        {
            return "/content/" + UrlRewrite.FormatSpecial(bodyType) + "-type-bikes/";
        }

        public static string GenerateBodyTypeMakeLink(string bodyType, string make)
        {
            return "<a title=\"" + bodyType + " bikes in India\" href=\"" + GetBodyTypeMakeUrl(bodyType, make) + "\">" + make + "</a>";
        }

        public static string GetBodyTypeMakeUrl(string bodyType, string make)
        {
            return "/content/" + UrlRewrite.FormatSpecial(bodyType) + "-type-bikes/" + UrlRewrite.FormatSpecial(make) + "/";
        }

        public static string GenerateBodyTypeModelLink(string bodyType, string make, string model)
        {
            return "<a title=\"" + bodyType + " bikes in India\" href=\"" + GetBodyTypeModelUrl(bodyType, make, model) + "\">" + make + " " + model + "</a>";
        }

        public static string GetBodyTypeModelUrl(string bodyType, string make, string model)
        {
            return "/content/" + UrlRewrite.FormatSpecial(bodyType) + "-type-bikes/" + UrlRewrite.FormatSpecial(make) + "/" + UrlRewrite.FormatSpecial(model) + "/";
        }

        public static string GenerateSegmentLink(string segment)
        {
            return "<a title=\"" + segment + " bikes in India\" href=\"" + GetSegmentUrl(segment) + "\">" + segment + "</a>";
        }

        public static string GetSegmentUrl(string segment)
        {
            return "/content/" + UrlRewrite.FormatSpecial(segment) + "-segment-bikes/";
        }

        public static string GenerateSegmentMakeLink(string segment, string make)
        {
            return "<a title=\"" + segment + " bikes in India\" href=\"" + GetSegmentMakeUrl(segment, make) + "\">" + make + "</a>";
        }

        public static string GetSegmentMakeUrl(string segment, string make)
        {
            return "/content/" + UrlRewrite.FormatSpecial(segment) + "-segment-bikes/" + UrlRewrite.FormatSpecial(make) + "/";
        }

        public static string GenerateSegmentModelLink(string segment, string make, string model)
        {
            return "<a title=\"" + segment + " bikes in India\" href=\"" + GetSegmentModelUrl(segment, make, model) + "\">" + make + " " + model + "</a>";
        }

        public static string GetSegmentModelUrl(string segment, string make, string model)
        {
            return "/content/" + UrlRewrite.FormatSpecial(segment) + "-segment-bikes/" + UrlRewrite.FormatSpecial(make) + "/" + UrlRewrite.FormatSpecial(model) + "/";
        }

        public static string GenerateDetailsLink(string make, string model, string version, string versionId)
        {
            return "<a title=\"" + make + " " + model + " " + version + " bikes in India\" href=\"" + GetDetailsUrl(make, model, version, versionId) + "\">" + make + " " + model + " " + version + " - Overview</a>";
        }

        public static string GetDetailsUrl(string make, string model, string version, string versionId)
        {
            return "/content/bikes/" + UrlRewrite.FormatSpecial(make) + "/" + UrlRewrite.FormatSpecial(model) + "/" +
                                                UrlRewrite.FormatSpecial(version) + "_details-" + versionId + ".html";
        }

        public static string GenerateSpecificationsLink(string make, string model, string version, string versionId)
        {
            return "<a title=\"" + make + " " + model + " " + version + " bikes in India\" href=\"" + GetSpecificationsUrl(make, model, version, versionId) + "\">" + make + " " + model + " " + version + " - Specifications</a>";
        }

        public static string GetSpecificationsUrl(string make, string model, string version, string versionId)
        {
            return "/content/bikes/" + UrlRewrite.FormatSpecial(make) + "/" + UrlRewrite.FormatSpecial(model) + "/" +
                                                UrlRewrite.FormatSpecial(version) + "_specifications-" + versionId + ".html";
        }

        public static string GenerateFeaturesLink(string make, string model, string version, string versionId)
        {
            return "<a title=\"" + make + " " + model + " " + version + " bikes in India\" href=\"" + GetFeaturesUrl(make, model, version, versionId) + "\">" + make + " " + model + " " + version + " - Features</a>";
        }

        public static string GetFeaturesUrl(string make, string model, string version, string versionId)
        {
            return "/content/bikes/" + UrlRewrite.FormatSpecial(make) + "/" + UrlRewrite.FormatSpecial(model) + "/" +
                                                UrlRewrite.FormatSpecial(version) + "_features-" + versionId + ".html";
        }

        public static string GenerateAllFeaturesLink(string make, string model, string version, string versionId)
        {
            return "<a title=\"" + make + " " + model + " " + version + " bikes in India\" href=\"" + GetAllFeaturesUrl(make, model, version, versionId) + "\">" + make + " " + model + " " + version + " - All Features</a>";
        }

        public static string GetAllFeaturesUrl(string make, string model, string version, string versionId)
        {
            return "/content/bikes/" + UrlRewrite.FormatSpecial(make) + "/" + UrlRewrite.FormatSpecial(model) + "/" +
                                                UrlRewrite.FormatSpecial(version) + "_allfeatures-" + versionId + ".html";
        }

        public static string GenerateColorsLink(string make, string model, string version, string versionId)
        {
            return "<a title=\"" + make + " " + model + " " + version + " bikes in India\" href=\"" + GetColorsUrl(make, model, version, versionId) + "\">" + make + " " + model + " " + version + " - Colors</a>";
        }

        public static string GetColorsUrl(string make, string model, string version, string versionId)
        {
            return "/content/bikes/" + UrlRewrite.FormatSpecial(make) + "/" + UrlRewrite.FormatSpecial(model) + "/" +
                                                UrlRewrite.FormatSpecial(version) + "_colors-" + versionId + ".html";
        }

        public static string GenerateReviewsLink(string make, string model, string version, string versionId)
        {
            return "<a title=\"" + make + " " + model + " " + version + " bikes in India\" href=\"" + GetReviewsUrl(make, model, version, versionId) + "\">" + make + " " + model + " " + version + " - Reviews & Ratings</a>";
        }

        public static string GetReviewsUrl(string make, string model, string version, string versionId)
        {
            return "/content/bikes/" + UrlRewrite.FormatSpecial(make) + "/" + UrlRewrite.FormatSpecial(model) + "/" +
                                                UrlRewrite.FormatSpecial(version) + "_reviews-" + versionId + ".html";
        }

        public static string GeneratePhotosLink(string make, string model, string version, string versionId)
        {
            return "<a title=\"" + make + " " + model + " " + version + " bikes in India\" href=\"" + GetPhotosUrl(make, model, version, versionId) + "\">" + make + " " + model + " " + version + " - Photos</a>";
        }

        public static string GetPhotosUrl(string make, string model, string version, string versionId)
        {
            return "/content/bikes/" + UrlRewrite.FormatSpecial(make) + "/" + UrlRewrite.FormatSpecial(model) + "/" +
                                                UrlRewrite.FormatSpecial(version) + "_photos-" + versionId + ".html";
        }

        public static void Return404()
        {
            HttpContext.Current.Response.StatusCode = 404;
            HttpContext.Current.Response.End();
        }
    }//class
}//namespace