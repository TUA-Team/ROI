using System.Reflection;

namespace ROI.Extensions
{
    public static class TypeExtensions
    {
        // TODO: (low prio) cache these
        public static T GetField<T>(this object parent, string name,
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance)
        {
            return (T)parent.GetType().GetField(name, flags).GetValue(parent);
        }
    }
}
