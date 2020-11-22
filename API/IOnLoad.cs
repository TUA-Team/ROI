using System.Runtime.Serialization;
using Terraria.ModLoader;

namespace ROI.API
{
    public interface IOnLoad
    {
        void Load(Mod mod);
    }

    public static class SimpleLoadables
    {
        public static void Load(Mod mod)
        {
            foreach (var type in mod.Code.DefinedTypes)
            {
                if (type.IsAbstract)
                    continue;
                if (type.ContainsGenericParameters)
                    continue;

                if (typeof(IOnLoad).IsAssignableFrom(type))
                {
                    var instance = (IOnLoad)FormatterServices.GetUninitializedObject(type);

                    instance.Load(mod);
                }
            }
        }
    }
}
