using Terraria;
using Terraria.ModLoader;

namespace ROI.Extensions
{
    public static class BuffCheckExtensions
    {
        public static bool HasBuff<T>(this Player player) where T : ModBuff => player.HasBuff(typeof(T).GetModFromType().BuffType<T>());

        public static void AddBuff<T>(this Player player, int time, bool quiet = true) where T : ModBuff => player.AddBuff(typeof(T).GetModFromType().BuffType<T>(), time, quiet);

        public static void ClearBuff<T>(this Player player) where T : ModBuff => player.ClearBuff(typeof(T).GetModFromType().BuffType<T>());
    }
}