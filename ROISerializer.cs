using System;
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
<<<<<<< HEAD
                ["Revision"] = value.Revision,
            };

            public override Version Deserialize(TagCompound tag) => new Version(tag.GetAsInt("Major"), tag.GetAsInt("Minor"), tag.GetAsInt("Build"), tag.GetAsInt("Revision") <= -1 ? 0 : tag.GetAsInt("Revision"));
=======
                ["Revision"] = value.Revision < 0 ? 0 : value.Revision
            };

            public override Version Deserialize(TagCompound tag) => new Version(
                tag.GetAsInt("Major"),
                tag.GetAsInt("Minor"),
                tag.GetAsInt("Build"),
                tag.GetAsInt("Revision") < 0 ? 0 : tag.GetAsInt("Revision"));
>>>>>>> 93055d08c4298f520ee2b67f37961dd6c4805bd5
        }
    }
}
