using System;
using System.Diagnostics;
using Terraria;

namespace ROI.Models
{
    public struct Snowflake
    {
        private static ushort currentIncrement = 0;


        public Snowflake(ulong epoch = Constants.TERRARIA_EPOCH)
        {
            Timestamp = DateTime.UtcNow;
            Raw = (ulong)Timestamp.Subtract(new DateTime((long)epoch)).TotalMilliseconds << 22;

            WorkerId = (byte)Main.rand.Next(byte.MaxValue);
            Raw |= (ulong)WorkerId << 17;

            // divide by 255 because process ids are way too high
            // even with unchecked, processid always end up being 255
            ProcessId = (byte)(Process.GetCurrentProcess().Id / 255);
            Raw |= (ulong)ProcessId << 12;

            if (currentIncrement == ushort.MaxValue) currentIncrement = 0;
            Increment = currentIncrement++;
            Raw |= Increment;
        }


        public ulong Raw { get; }

        public DateTime Timestamp { get; }
        public byte WorkerId { get; }
        public byte ProcessId { get; }
        public ushort Increment { get; }
    }
}
