using System.Text;
using System.Web.Http.Description;

namespace Bikewale.Service.Areas.HelpPage
{
    public static class ApiDescriptionExtensions
    {
        /// <summary>
        /// Generates an URI-friendly ID for the <see cref="ApiDescription"/>. E.g. "Get-Values-id_name" instead of "GetValues/{id}?name={name}"
        /// </summary>
        /// <param name="description">The <see cref="ApiDescription"/>.</param>
        /// <returns>The ID as a string.</returns>
        public static string GetFriendlyId(this ApiDescription description)
        {
            //string path = description.RelativePath;
            //string[] urlParts = path.Split('?');
            //string localPath = urlParts[0];
            //string queryKeyString = null;
            //if (urlParts.Length > 1)
            //{
            //    string query = urlParts[1];
            //    string[] queryKeys = HttpUtility.ParseQueryString(query).AllKeys;
            //    queryKeyString = String.Join("_", queryKeys);
            //}

            //StringBuilder friendlyPath = new StringBuilder();
            //friendlyPath.AppendFormat("{0}-{1}",
            //    description.HttpMethod.PopulateWhere,
            //    localPath.Replace("/", "-").Replace("{", String.Empty).Replace("}", String.Empty));
            //if (queryKeyString != null)
            //{
            //    friendlyPath.AppendFormat("_{0}", queryKeyString.Replace('.', '-'));
            //}
            //return friendlyPath.ToString();
            string path = description.RelativePath;
            path = path.Replace("&", "%26");
            StringBuilder friendlyPath = new StringBuilder();
            friendlyPath.AppendFormat("{0}-{1}", description.HttpMethod.Method, path);
            return friendlyPath.ToString();
        }
    }
}