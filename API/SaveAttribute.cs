using System;

namespace API
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class SaveAttribute : Attribute
    {
        /// <summary>
        /// Unfortunately, because of NBT, this only works on <c>byte</c>, <c>short</c>,
        /// <c>int</c>, <c>long</c>, <c>float</c>, <c>float</c>, <c>double</c>, 
        /// <c>byte[]</c>, <c>string</c>, <c>IList</c>, <c>byte</c>, <c>TagCompound</c>, <c>int[]</c>
        /// </summary>
        public SaveAttribute()
        {
        }
    }
}
