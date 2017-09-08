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

        public static string JsonSerialize(dynamic objSchema,dynamic pageSchema)
        {
            try
            {
                JObject jsonObj = JObject.FromObject(objSchema);
                jsonObj.Add("@context", "http://schema.org");
                jsonObj.Add("mainPageEntity", pageSchema);
                return jsonObj.ToString(Newtonsoft.Json.Formatting.None);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
