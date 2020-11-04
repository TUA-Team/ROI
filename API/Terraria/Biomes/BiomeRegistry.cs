using API.Terraria.EntityComponents;
using Terraria;

namespace API.Terraria.Biomes
{
    public sealed class BiomeRegistry:ComponentRegistry<BiomeBase>
    {
        public bool IsBiomeActive<T>() where T : BiomeBase => RegisteredComponents.GetComponent<T>().Active;


        public void CopyCustomBiomesTo(Player other)
        {
            foreach (BiomeBase comp in RegisteredComponents)
                comp.CopyCustomBiomesTo(other);
        }

        public bool CustomBiomesMatch(Player other)
        {
            var otherRegistry = other.GetModPlayer<StandardPlayer>().BiomeRegistry;
            for (int i = 0; i < RegisteredComponents.Count; i++)
            {
                BiomeBase biome = RegisteredComponents[i];
                BiomeBase otherBiome = otherRegistry.RegisteredComponents[i];
                if (biome.Active ^ otherBiome.Active)
                    return false;
            }

            return true;
        }
    }
}
