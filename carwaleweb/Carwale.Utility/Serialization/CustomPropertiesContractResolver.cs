using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Carwale.Utility.Serialization
{
    public class CustomPropertiesContractResolver : DefaultContractResolver
    {
        private HashSet<string> _propertySet;
        private static CamelCasePropertyNamesContractResolver _camelCasePropertyResolver = new CamelCasePropertyNamesContractResolver();

        public CustomPropertiesContractResolver(IEnumerable<string> propertyNames)
        {
            if (propertyNames != null)
            {
                _propertySet = new HashSet<string>(propertyNames, StringComparer.OrdinalIgnoreCase);
            }
        }

        public CustomPropertiesContractResolver(string propertyNames)
        {
            if (!String.IsNullOrWhiteSpace(propertyNames))
            {
                var names = propertyNames.Split(',').Where(n => !String.IsNullOrWhiteSpace(n));
                _propertySet = new HashSet<string>(names, StringComparer.OrdinalIgnoreCase);
            }
        }

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            List<MemberInfo> serializableMembers = null;
            var allMembers = base.GetSerializableMembers(objectType);

            if (_propertySet != null && _propertySet.Count > 0)
            {
                serializableMembers = allMembers.Where(m => _propertySet.Contains(m.Name)).ToList();
            }
            return serializableMembers != null && serializableMembers.Count > 0 ? serializableMembers : allMembers;
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            return _camelCasePropertyResolver.GetResolvedPropertyName(propertyName);
        }
    }
}
