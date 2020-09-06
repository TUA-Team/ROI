using ROI.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ROI
{
    // We call this ROIMod instead of ROI since we don't want to have a class with the same name as the namespace its in.
    public sealed partial class ROIMod : Mod
    {
        public override void Load() {
            InitializeLoaders();

            GenerateLocalization();

            if (!Main.dedServ) {
                LoadClient();
            }
        }

        public override void Unload() {
            UnloadLoaders();

            if (!Main.dedServ) {
                UnloadClient();
            }
        }

        private void GenerateLocalization() {
            var dictionary = (Dictionary<string, ModTranslation>)
                this.GetField<IDictionary<string, ModTranslation>>("translations");

            var list = new List<string>();

            list.AddRange(this.GetField<IDictionary<string, ModItem>>("items")
                .Select(x => $"{x.Value.DisplayName.Key}={x.Value.DisplayName.GetTranslation(Language.ActiveCulture)}"));

            list.AddRange(this.GetField<IDictionary<string, ModItem>>("items")
                .Select(x => $"{x.Value.Tooltip.Key}={x.Value.Tooltip.GetTranslation(Language.ActiveCulture)}"));

            list.AddRange(this.GetField<IDictionary<string, ModNPC>>("npcs")
                .Select(x => $"{x.Value.DisplayName.Key}={x.Value.DisplayName.GetTranslation(Language.ActiveCulture)}"));

            list.AddRange(this.GetField<IDictionary<string, ModBuff>>("buffs")
                .Select(x => $"{x.Value.DisplayName.Key}={x.Value.DisplayName.GetTranslation(Language.ActiveCulture)}"));

            list.AddRange(this.GetField<IDictionary<string, ModBuff>>("buffs")
                .Select(x => $"{x.Value.Description.Key}={x.Value.Description.GetTranslation(Language.ActiveCulture)}"));

            list.AddRange(this.GetField<IDictionary<string, ModProjectile>>("projectiles")
                .Select(x => $"{x.Value.DisplayName.Key}={x.Value.DisplayName.GetTranslation(Language.ActiveCulture)}"));

            int index = $"Mods.ROI.".Length;
            ReLogic.OS.Platform.Current.Clipboard = string.Join("\n", list.Select(x => x.Remove(0, index)));
        }
    }
}