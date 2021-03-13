using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader.IO;

namespace ROI.Core.Verlet.Contexts.Chains
{
    public class ChainDrawData : TagSerializable
    {
        public Rectangle source;
        public Vector2 origin;


        public TagCompound SerializeData() => new TagCompound
        {
            [nameof(source)] = source,
            [nameof(origin)] = origin,
        };

        public static readonly Func<TagCompound, ChainDrawData> DESERIALIZER = tag =>
        {
            var data = new ChainDrawData
            {
                source = tag.Get<Rectangle>(nameof(source)),
                origin = tag.Get<Vector2>(nameof(origin))
            };

            return data;
        };
    }
}
