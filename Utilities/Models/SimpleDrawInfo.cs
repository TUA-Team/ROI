using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader.IO;

namespace ROI.Utilities.Models
{
    public class SimpleDrawSource : TagSerializable
    {
        public Rectangle source;
        public Vector2 origin;

        public TagCompound SerializeData() => new TagCompound
        {
            [nameof(source)] = source,
            [nameof(origin)] = origin,
        };

        public static readonly Func<TagCompound, SimpleDrawSource> DESERIALIZER = tag =>
        {
            var data = new SimpleDrawSource
            {
                source = tag.Get<Rectangle>(nameof(source)),
                origin = tag.Get<Vector2>(nameof(origin))
            };

            return data;
        };
    }
}
