﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria.Localization;
using Terraria.ModLoader;

namespace API
{
    public static class Utils
    {
        // TODO: (low prio) cache these
        public static T GetField<T>(this object parent, string name,
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance)
        {
            return (T)parent.GetType().GetField(name, flags).GetValue(parent);
        }

        // TODO: remove on 1.4 port
        public static void GenerateLocalization(Mod mod)
        {
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

            int index = $"Mods.{mod.Name}.".Length;
            File.WriteAllText(Path.Combine(ModLoader.ModPath, "localized.txt"), string.Join("\n", list.Select(x => x.Remove(0, index))));
        }

        public static T GetOrAdd<T>(this ICollection<T> col, Func<T, bool> predicate, Func<T> factory)
        {
            var val = col.FirstOrDefault(predicate);
            if (val != null)
                return val;
            val = factory();
            col.Add(val);
            return val;
        }

        public static bool EqualsIC(this string str, string str2) => str.Equals(str2, StringComparison.OrdinalIgnoreCase);
    }
}
