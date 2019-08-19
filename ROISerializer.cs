using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace ROI
{
    /// <summary>
    /// Keep this in case we ever add custom serializer, which is probable with electricity system
    /// </summary>
    internal class ROISerializer
    {
        internal class VersionSerializer : TagSerializer<Version, TagCompound>
        {
            public override TagCompound Serialize(Version value) => new TagCompound()
            {
                ["Major"] = value.Major,
                ["Minor"] = value.Minor,
                ["Build"] = value.Build,
                ["Revision"] = value.Revision
            };

            public override Version Deserialize(TagCompound tag) => new Version(tag.GetAsInt("Major"), tag.GetAsInt("Minor"), tag.GetAsInt("Build"), tag.GetAsInt("Revision"));
        }
    }
}
