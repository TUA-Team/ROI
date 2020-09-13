using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace API.Networking
{
    public class ShouldSerializeNetworkDataContractResolver : DefaultContractResolver
    {
        private readonly byte packet;

        public ShouldSerializeNetworkDataContractResolver(byte packet)
        {
            this.packet = packet;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            property.ShouldSerialize =
                instance =>
                {
                    return member.DeclaringType.GetCustomAttribute<SyncAttribute>() != null;
                };

            return property;
        }
    }
}
