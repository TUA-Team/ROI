using API.Terraria.EntityComponents;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;

namespace API.Terraria.Biomes
{
    public sealed class BiomeRegistry : ComponentRegistry<BiomeBase>
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

        public void SendCustomBiomes(BinaryWriter writer)
        {
            var bits = new BitArrayU8(Enumerable.Select<BiomeBase, bool>(RegisteredComponents,
                x => x.Active).ToArray());

            //writer.Write((byte)arr.Length);
            for (int i = 0; i < bits.InternalArray.Length; i++)
            {
                writer.Write(bits.InternalArray[i]);
            }
        }

        public void ReceiveCustomBiomes(BinaryReader reader)
        {
            var len = reader.ReadByte();
            var arr = new byte[len];
            for (int i = 0; i < len; i++)
            {
                arr[i] = reader.ReadByte();
            }

            var bits = new BitArrayU8(arr);
            int index = 0;
            foreach (BiomeBase biome in RegisteredComponents)
            {
                biome.ReceiveBiome(bits.Get(index));
            }
        }
    }
}
