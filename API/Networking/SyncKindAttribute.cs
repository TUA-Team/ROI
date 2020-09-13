using System;

namespace API.Networking
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class SyncKindAttribute : Attribute
    {
        public SyncKindAttribute(string kind)
        {
            Kind = kind;
        }


        public string Kind { get; }
    }
}
