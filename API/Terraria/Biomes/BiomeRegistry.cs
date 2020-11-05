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
            var enumerator = RegisteredComponents.GetEnumerator();
            var otherEnumerator = otherRegistry.RegisteredComponents.GetEnumerator();
            
            while (enumerator.MoveNext() && otherEnumerator.MoveNext())
            {
                var biome = enumerator.Current;
                var otherBiome = otherEnumerator.Current;

                if (biome.Active ^ otherBiome.Active)
                    return false;
            }

            return true;
        }
    }
}
