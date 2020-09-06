using API;
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

            // TODO: ask someone if it's intentional that inline out is broken
#pragma warning disable IDE0018 // Inline variable declaration
            int index;
#pragma warning restore IDE0018 // Inline variable declaration
            insertLayerViaVanilla("Resources Bars", "Void Affinity", vAffinityInterface.Draw, out index);

            void insertLayerViaVanilla(string vanillaLayer, string name, Action<SpriteBatch, GameTime> draw, out int i) {
                i = layers.FindIndex(l => l.Name.Equals($"Vanilla: {vanillaLayer}"));
                insertLayer(i, name, draw);
            }

            void insertLayer(int j, string name, Action<SpriteBatch, GameTime> draw) {
                if (j == -1) {
                    list.Add(name);
                    return;
                }
                layers.Insert(j, new LegacyGameInterfaceLayer(
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
