using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace XTS.WSApi.Lib
{
    public class XTSJsonSerializerSettings : JsonSerializerSettings
    {
        private static XTSJsonSerializerSettings _instance = new XTSJsonSerializerSettings();
        public static XTSJsonSerializerSettings Instance { get { return _instance; } }

        public XTSJsonSerializerSettings()
        {
            base.ContractResolver = XTSContractResolver.Instance;
        }
    }

    public class XTSContractResolver : DefaultContractResolver
    {
        private static XTSContractResolver _instance = new XTSContractResolver();
        private ConcurrentDictionary<Type, IList<JsonProperty>> _propertyList = new ConcurrentDictionary<Type, IList<JsonProperty>>();

        public static XTSContractResolver Instance { get { return _instance; } }

        protected override JsonProperty CreateProperty
            (MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);
            if (member is PropertyInfo)
            {
                PropertyInfo pi = (PropertyInfo)member;
                prop.Readable = (pi.GetMethod != null);
                prop.Writable = (pi.SetMethod != null);
            }
            return prop;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties;
            if (!_propertyList.TryGetValue(type, out properties))
            {
                properties = base.CreateProperties(type, memberSerialization);
                _propertyList.TryAdd(type, properties);
            }
            return properties;
        }
    }
}
