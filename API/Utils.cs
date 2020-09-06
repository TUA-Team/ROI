using ROI.Extensions;
using System.Collections.Generic;
using System.Linq;
using Terraria.Localization;
using Terraria.ModLoader;

namespace API
{
    public static class Utils
    {
        public static void GenerateLocalization(Mod mod) {
            var dictionary = (Dictionary<string, ModTranslation>)
                mod.GetField<IDictionary<string, ModTranslation>>("translations");

            var list = new List<string>();

            list.AddRange(mod.GetField<IDictionary<string, ModItem>>("items")
                .Select(x => $"{x.Value.DisplayName.Key}={x.Value.DisplayName.GetTranslation(Language.ActiveCulture)}"));

            list.AddRange(mod.GetField<IDictionary<string, ModItem>>("items")
                .Select(x => $"{x.Value.Tooltip.Key}={x.Value.Tooltip.GetTranslation(Language.ActiveCulture)}"));

            list.AddRange(mod.GetField<IDictionary<string, ModNPC>>("npcs")
                .Select(x => $"{x.Value.DisplayName.Key}={x.Value.DisplayName.GetTranslation(Language.ActiveCulture)}"));

            list.AddRange(mod.GetField<IDictionary<string, ModBuff>>("buffs")
                .Select(x => $"{x.Value.DisplayName.Key}={x.Value.DisplayName.GetTranslation(Language.ActiveCulture)}"));

            list.AddRange(mod.GetField<IDictionary<string, ModBuff>>("buffs")
                .Select(x => $"{x.Value.Description.Key}={x.Value.Description.GetTranslation(Language.ActiveCulture)}"));

            list.AddRange(mod.GetField<IDictionary<string, ModProjectile>>("projectiles")
                .Select(x => $"{x.Value.DisplayName.Key}={x.Value.DisplayName.GetTranslation(Language.ActiveCulture)}"));

            int index = $"Mods.ROI.".Length;
            ReLogic.OS.Platform.Current.Clipboard = string.Join("\n", list.Select(x => x.Remove(0, index)));
        }
    }
}
