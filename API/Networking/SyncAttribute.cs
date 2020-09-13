using System;

namespace ROI.API.Networking
{
    // TODO: (med prio) currently only works on primitives
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class SyncAttribute : Attribute
    {
        public SyncAttribute(byte packetType)
        {
            PacketType = packetType;
        }


        public byte PacketType { get; }
    }
}
