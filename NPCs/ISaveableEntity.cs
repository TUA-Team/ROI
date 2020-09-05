using Terraria.ModLoader.IO;

namespace ROI.NPCs
{
    public interface ISaveableEntity
    {
        /// <summary>Allows you to save extra data on world save. The position, name and mod are already handled in the world save.</summary>
        /// <param name="tag"></param>
        void Save(TagCompound tag);

        void Load(TagCompound tag);

        bool SaveHP { get; }
    }
}