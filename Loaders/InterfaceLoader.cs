using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROI.API;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;

namespace ROI.Loaders
{
    // this is public because all of the UI states are public
    // possible choices: use normal UserInterfaces, use public properties, use public setter methods, use public states
    public class InterfaceLoader : Singleton<InterfaceLoader>
    {
        //public VoidPillarHealthBar vPillarHealthState;
        //public ROIUserInterface<VoidPillarHealthBar> vPillarHealthInterface;


        private GameTime lastGameTime;

        public override void Load()
        {
            //VoidAffinity.Load();

            //vPillarHealthState = new VoidPillarHealthBar(mod);
            //vPillarHealthState.Activate();
            //vPillarHealthInterface = new ROIUserInterface<VoidPillarHealthBar>();
            // implicit: vPillarHealthInterface.SetState(null);
        }


        public void UpdateUI(GameTime gameTime)
        {
            lastGameTime = gameTime;

            // TODO: (super low prio) write simple ?. based way to do this
            //if (vPillarHealthInterface.CurrentState != null)
            //{
            //    vPillarHealthInterface.Update(lastGameTime);
            //}
        }

        // list of failedInterfaces is there to debug any possible problems with
        // other mods disabling layers
        public void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            return;

            var failed = new List<string>();

            //insertLayerViaVanilla("Resources Bars", "Void Affinity", (sb, _) => VoidAffinity.Draw(sb), out int index);


            // indexes are named i and j because I was too lazy to figure out
            // how to name it `index` without breaking stuff - Agrair
            void insertLayerViaVanilla(string vanillaLayer, string name, Action<SpriteBatch, GameTime> draw, out int i)
            {
                i = layers.FindIndex(l => l.Name.Equals($"Vanilla: {vanillaLayer}"));
                insertLayer(i, name, draw);
            }

            void insertLayer(int j, string name, Action<SpriteBatch, GameTime> draw)
            {
                if (j == -1)
                {
                    failed.Add(name);
                    return;
                }
                layers.Insert(j, new LegacyGameInterfaceLayer(
                    $"ROI: {name}",
                    delegate
                    {
                        draw(Main.spriteBatch, lastGameTime);
                        return true;
                    }, InterfaceScaleType.UI));
            }
        }

        public override void Unload()
        {
            //VoidAffinity.Unload();
        }

        // list of failedInterfaces is there to debug any possible problems with
        // other mods disabling layers
        /*        public bool ModifyInterfaceLayers(List<GameInterfaceLayer> layers, out ICollection<string> failedInterfaces)
                {
                    var failed = new List<string>();

                    insertLayerViaVanilla("Resources Bars", "Void Affinity", vAffinityInterface.Draw, out int index);


                    // indexes are named i and j because I was too lazy to figure out
                    // how to name it `index` without breaking stuff - Agrair
                    void insertLayerViaVanilla(string vanillaLayer, string name, Action<SpriteBatch, GameTime> draw, out int i)
                    {
                        i = layers.FindIndex(l => l.Name.Equals($"Vanilla: {vanillaLayer}"));
                        insertLayer(i, name, draw);
                    }

                    void insertLayer(int j, string name, Action<SpriteBatch, GameTime> draw)
                    {
                        if (j == -1)
                        {
                            failed.Add(name);
                            return;
                        }
                        layers.Insert(j, new LegacyGameInterfaceLayer(
                            $"ROI: {name}",
                            delegate
                            {
                                draw(Main.spriteBatch, lastGameTime);
                                return true;
                            }, InterfaceScaleType.UI));
                    }

                    return (failedInterfaces = failed.Count != 0 ? failed : null) == null;
                }*/
    }
}
