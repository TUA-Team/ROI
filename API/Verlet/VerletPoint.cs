using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader.IO;

namespace ROI.API.Verlet
{
    // TODO: This should really be a struct, but that would unfortunately require unsafe
    public class VerletPoint : TagSerializable
    {
        public Vector2 old;
        public Vector2 pos;

        public VerletPoint(Vector2 old, Vector2 pos)
        {
            this.old = old;
            this.pos = pos;
        }


        public TagCompound SerializeData() => new TagCompound
        {
            [nameof(pos)] = pos
        };

        public static readonly Func<TagCompound, VerletPoint> DESERIALIZER = tag =>
        {
            var pos = tag.Get<Vector2>(nameof(VerletPoint.pos));
            return new VerletPoint(pos, pos);
        };
    }
}
