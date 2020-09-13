using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace API.Networking
{
    public class ShouldSerializeNetworkDataContractResolver : DefaultContractResolver
    {
        private readonly string kind;

        public ShouldSerializeNetworkDataContractResolver(string kind)
        {
            this.kind = kind;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            property.ShouldSerialize =
                instance => member.DeclaringType.GetCustomAttribute<SyncKindAttribute>() is var attr
                    && attr.Kind.EqualsIC(kind);

            return property;
        }
    }
}
