using log4net;
using Microsoft.Xna.Framework;
using ROI.Crafting;
using ROI.NPCs.HeartOfTheWasteland;
using ROI.NPCs.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiquidAPI;
using LiquidAPI.LiquidMod;
using ROI.NPCs.Void;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;

namespace ROI.Worlds
{
    partial class ROIWorld : ModWorld
    {
        private int _pillarSpawningTimer;

        /// <summary>
        /// This version string gonna be really important as we'll use it to distinguish an old world with a new one
        /// </summary>
        internal Version version = new Version(0, 0, 1, 1);

        public static int activeHotWID;

        internal static List<Tentacle> tenctacleList = new List<Tentacle>();

        

        public override TagCompound Save()
        {
            return new TagCompound()
            {
                ["modNPCData"] = SaveModNPCData(),
                [nameof(version)] = version
            };
        }

        private static List<TagCompound> SaveModNPCData()
        {
            List<TagCompound> npcList = new List<TagCompound>();
            foreach (NPC i in Main.npc)
            {
                if (i.modNPC is ISavableEntity entity)
                {

                    TagCompound currentEntityTag = entity.Save();
                    currentEntityTag["position"] = i.position;
                    currentEntityTag["name"] = i.modNPC.Name;
                    currentEntityTag["mod"] = i.modNPC.mod.Name;
                    if (entity.SaveHP)
                    {
                        currentEntityTag["Health"] = i.life;
                    }
                    npcList.Add(currentEntityTag);
                }
            }

            return npcList;
        }

        public override void Load(TagCompound tag)
        {
            LoadModNPCData(tag);
            try
            {
                if (tag.ContainsKey(nameof(version)))
                {
                    version = tag.Get<Version>(nameof(version));
                }
                else
                {
                    version = new Version(0, 0, 0, 0);
                }
            }
            catch (Exception e)
            {
                version = new Version(0, 0, 0, 0);
            }
            
        }

        private static void LoadModNPCData(TagCompound tag)
        {
            List<TagCompound> modNPCData = tag.Get<List<TagCompound>>("modNPCData");
            foreach (TagCompound tagCompound in modNPCData)
            {
                Vector2 position = tagCompound.Get<Vector2>("position");
                int instanceNPCID = NPC.NewNPC((int)position.X, (int)position.Y, ModLoader.GetMod(tagCompound.GetString("mod")).NPCType(tagCompound.GetString("name")));
                if (Main.npc[instanceNPCID].modNPC is ISavableEntity entity)
                {
                    entity.Load(tagCompound);
                }
                if (tag.ContainsKey("Health"))
                {
                    Main.npc[instanceNPCID].life = tag.GetAsInt("Health");
                }
            }
        }

        public override void PreUpdate()
        {
            foreach (Tentacle tentacle in tenctacleList)
            {
                tentacle.Update(ROIMod.gameTime);
            }


            Main.bottomWorld = (Main.maxTilesY * 16) + 400;
            Main.topWorld = 0;
            Main.leftWorld = 0;
            Main.rightWorld = Main.maxTilesX * 16;
            if (!NPC.AnyNPCs(ModContent.NPCType<HeartOfTheWasteland>()))
            {
                activeHotWID = -1;
            }

            LiquidCrafting.RecipeMatch(Main.item.Where(i => i.type != 0).ToArray());
        }

        public override void NetSend(BinaryWriter writer)
        {
        }

        public override void NetReceive(BinaryReader reader)
        {
        }

        public override void PostDrawTiles()
        {
            Main.spriteBatch.Begin();
            foreach (Tentacle tentacle in tenctacleList)
            {
                tentacle.Draw();
            }
            Main.spriteBatch.End();
            ModLiquid waste = LiquidRegistry.GetLiquid(ModLoader.GetMod("LiquidAPI"), "PlutonicWaste");
            for (int i = (int) (Main.screenPosition.X - 3)  / 16; i < (Main.screenPosition.X - 3 + Main.graphics.PreferredBackBufferWidth + 3)  / 16; i++)
            {
                for (int j = (int) (Main.screenPosition.Y - 3)  / 16; j < (Main.screenPosition.Y - 3 + Main.graphics.PreferredBackBufferWidth + 3)  / 16; j++)
                {
                    if (!WorldGen.InWorld(i, j) || !WorldGen.InWorld(i, j - 1))
                        break;

                    LiquidRef reference = LiquidWorld.grid[i, j];
                    LiquidRef referenceUp = LiquidWorld.grid[i, j - 1];
                    if (reference.Type.Type == waste.Type && !referenceUp.Tile.active() && !referenceUp.HasLiquid)
                    {
                     
                        Lighting.AddLight(i, j , 0.01f, 1f * 0.8f, 0.01f);
                        
                    }
                }
            }
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int hellGen = tasks.FindIndex(i => i.Name == "Underworld");
            int hellForgeGen = tasks.FindIndex(i => i.Name == "Hellforge");
            mod.Logger.Info(Main.maxTilesX);
            mod.Logger.Info(Main.maxTilesY);
            if (hellGen != -1)
            {
                tasks[hellGen] = new PassLegacy("Underworld", (progress) =>
                {
                    OriginalUnderworldGeneration(progress);
                });
            }

            if (hellForgeGen != -1)
            {
                tasks[hellForgeGen] = new PassLegacy("Hellforge", progress =>
                {
                    //Why the fuck is this it's own phase in the first place
                    return;
                });
            }

        }

        public override void PostWorldGen()
        {
            version = ROIMod.instance.Version;
        }

        
    }
}
