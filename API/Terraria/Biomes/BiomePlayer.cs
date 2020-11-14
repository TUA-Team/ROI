using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace API.Terraria.Biomes
{
    public sealed class BiomePlayer : ModPlayer
    {
        public BiomeRegistry BiomeRegistry { get; private set; }

        public override void Initialize()
        {
            BiomeRegistry = new BiomeRegistry();
            for (int i = 0; i < IdHookLookup<BiomeBase>.Instances.Count; i++)
            {
                var biome = IdHookLookup<BiomeBase>.Instances[i];
                BiomeRegistry.Register(biome);
            }
            BiomeRegistry.Build();
        }


        public override void UpdateBiomes()
        {
            BiomeRegistry.Update();
        }

        public override void CopyCustomBiomesTo(Player other)
        {
            BiomeRegistry.CopyCustomBiomesTo(other);
        }

        public override bool CustomBiomesMatch(Player other)
        {
            return BiomeRegistry.CustomBiomesMatch(other);
        }

        public override void SendCustomBiomes(BinaryWriter writer)
        {
            BiomeRegistry.SendCustomBiomes(writer);
        }

        public override void ReceiveCustomBiomes(BinaryReader reader)
        {
            BiomeRegistry.ReceiveCustomBiomes(reader);
        }
    }
}
