using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROI.Models;
using ROI.UI.Void;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ROI.Loaders
{
    // this is internal because all of the UI states are internal
    // possible choices: use normal UserInterfaces, use public properties, use public setter methods
    internal sealed class InterfaceLoader : BaseLoader
    {
        public VoidAffinity vAffinityState;
        public ROIUserInterface<VoidAffinity> vAffinityInterface;

        public VoidPillarHealthBar vPillarHealthState;
        public ROIUserInterface<VoidPillarHealthBar> vPillarHealthInterface;


        private GameTime lastGameTime;


        public override void Initialize(Mod mod) {
            vAffinityState = new VoidAffinity(mod);
            vAffinityState.Activate();
            vAffinityInterface = new ROIUserInterface<VoidAffinity>();
            vAffinityInterface.SetState(vAffinityState);

            vPillarHealthState = new VoidPillarHealthBar(mod);
            vPillarHealthState.Activate();
            vPillarHealthInterface = new ROIUserInterface<VoidPillarHealthBar>();
            // implicit: vPillarHealthInterface.SetState(null);
        }

        public void UpdateUI(GameTime gameTime) {
            lastGameTime = gameTime;

            if (vPillarHealthInterface.CurrentState != null) {
                vPillarHealthInterface.Update(lastGameTime);
            }
        }

        // list of failedInterfaces is there to debug any possible problems with
        // other mods disabling layers
        public bool ModifyInterfaceLayers(List<GameInterfaceLayer> layers, out ICollection<string> failedInterfaces) {
            var list = new List<string>();

            insertLayerViaVanilla("Resources Bars", "Void Affinity", vAffinityInterface.Draw, out var index);

            void insertLayerViaVanilla(string vanillaLayer, string name, Action<SpriteBatch, GameTime> draw, out int index) {
                index = layers.FindIndex(l => l.Name.Equals($"Vanilla: {vanillaLayer}"));
                insertLayer(index, name, draw);
            }

            void insertLayer(int index, string name, Action<SpriteBatch, GameTime> draw) {
                if (index == -1) {
                    list.Add(name);
                    return;
                }
                layers.Insert(index, new LegacyGameInterfaceLayer(
                    $"ROI: {name}",
                    delegate {
                        draw(Main.spriteBatch, lastGameTime);
                        return true;
                    }, InterfaceScaleType.UI));
            }

            return (failedInterfaces = list.Count != 0 ? list : null) == null;
        }
    }
}
