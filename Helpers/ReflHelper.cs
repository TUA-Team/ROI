using System.Reflection;

namespace ROI.Helpers
{
    public static class ReflHelper
    {
        //TODO: use cache or something
        public static T GetField<T>(this object parent, string name, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance)
        {
            return (T)parent.GetType().GetField(name, flags).GetValue(parent);
        }
    }
}
