using System;
using Terraria.ModLoader.IO;

namespace ROI.API.Verlet
{
    public class VerletSegment : TagSerializable
    {
        public int start;
        public int next;
        public float size;

        public VerletSegment(int start, int next, float size)
        {
            this.start = start;
            this.next = next;
            this.size = size;
        }


        public TagCompound SerializeData() => new TagCompound
        {
            [nameof(start)] = start,
            [nameof(next)] = next,
            [nameof(size)] = size
        };

        public static readonly Func<TagCompound, VerletSegment> DESERIALIZER = tag =>
        {
            return new VerletSegment(tag.GetInt(nameof(start)), tag.GetInt(nameof(next)), tag.GetFloat(nameof(size)));
        };
    }
}
