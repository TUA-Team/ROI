using System;

namespace API
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class SaveAttribute : Attribute
    {
        public SaveAttribute()
        {
        }
    }
}
