using API.CustomModLoader.EntityComponents;
using System.IO;
using System.Linq;
using Terraria;

namespace API.CustomModLoader.Biomes
{
    public sealed class BiomeRegistry : HookRegistry<BiomeBase>
    {
        public bool IsBiomeActive<T>() where T : BiomeBase =>
            registered.TryGetValue(typeof(T), out var b) && b.Active;


        public void CopyCustomBiomesTo(Player other)
        {
            foreach (BiomeBase comp in registered.Values)
                comp.CopyCustomBiomesTo(other);
        }

        public bool CustomBiomesMatch(Player other)
        {
            var otherRegistry = other.GetModPlayer<BiomePlayer>().BiomeRegistry;
            var enumerator = registered.Values.GetEnumerator();
            var otherEnumerator = otherRegistry.registered.Values.GetEnumerator();

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
            var bits = new BitArrayU8(Enumerable.Select(registered.Values,
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
            foreach (BiomeBase biome in registered.Values)
            {
                biome.ReceiveBiome(bits.Get(index));
            }
        }
    }
}
