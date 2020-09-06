using Terraria;
using Terraria.ModLoader;

namespace ROI.Extensions
{
    public static class BuffCheckExtensions
    {
        public static bool HasBuff<T>(this Player player) where T : ModBuff => player.HasBuff(ModContent.BuffType<T>());

        public static void AddBuff<T>(this Player player, int time, bool quiet = true) where T : ModBuff => player.AddBuff(ModContent.BuffType<T>(), time, quiet);

        public static void ClearBuff<T>(this Player player) where T : ModBuff => player.ClearBuff(ModContent.BuffType<T>());
    }
}