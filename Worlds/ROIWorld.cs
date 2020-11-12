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
using ROI.Projectiles;
using ROI.Projectiles.VoidPillarProjectiles;
using ROI.Worlds.Structures.Wasteland;
using ROI.Worlds.Subworlds.Wasteland;
using SubworldLibrary;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.Graphics.Effects;
using Terraria.ID;
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

        internal static List<Wasteland_Cave> wastelandCaves;


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
            ModLiquid lava = LiquidRegistry.GetLiquid(ModLoader.GetMod("LiquidAPI"), "Lava");
            for (int i = (int)(Main.screenPosition.X) / 16 - 16; i < (Main.screenPosition.X + Main.graphics.PreferredBackBufferWidth) / 16 + 16; i++)
            {
                for (int j = (int)(Main.screenPosition.Y) / 16 - 16; j < (Main.screenPosition.Y + Main.graphics.PreferredBackBufferWidth) / 16 + 16; j++)
                {
                    if (!WorldGen.InWorld(i, j) || !WorldGen.InWorld(i, j - 1))
                        break;

                    LiquidRef reference = LiquidWorld.grid[i, j];
                    LiquidRef referenceUp = LiquidWorld.grid[i, j - 1];
                    LiquidRef referenceUpRight = LiquidWorld.grid[i + 1, j - 1];
                    if (reference.Type.Type == waste.Type && !referenceUp.HasLiquid && reference.HasLiquid)
                    {
                        float preBrightness = Lighting.brightness;
                        Lighting.brightness = 0.2f;
                        Lighting.AddLight(i, j, 0.01f, 0.5f, 0.01f);
                        Lighting.brightness = preBrightness;
                        // TODO: Move this into a proper method for server
                        if (Main.netMode != NetmodeID.Server && Main.rand.Next(5000) == 0 && reference.Amount > 200 && !referenceUpRight.Tile.active() && Main.hasFocus)
                        {
                            int properOffset = (j * 16) + 16 - (16 - reference.Amount / 16); 
                            Projectile.NewProjectile(new Vector2(i * 16, (j - 1) * 16 + 8f), Vector2.Zero, ModContent.ProjectileType<Wasteland_Surface_Bubble>(), 0, 0);
                        }

                        if (Main.netMode != NetmodeID.Server && Subworld.IsActive<TheWastelandDepthSubworld>() && Main.rand.Next(5000) == 0  && Main.hasFocus)
                        {
                            int bubbleType = Main.rand.Next(0, 3);
                            
                            int projectile = Projectile.NewProjectile(new Vector2(i * 16, (j + 2) * 16), new Vector2(Main.rand.NextFloat(-0.36f, 0.36f), -0.5f), ModContent.ProjectileType<Wasteland_Bubble>(), 0, 0, 255, bubbleType);
                            Main.projectile[projectile].ai[0] = bubbleType;
                        }
                    }
                    if (reference.Type.Type == lava.Type)
                    {
                        float preBrightness = Lighting.brightness;
                        Lighting.brightness = 0.7f;
                        Lighting.AddLight(i, j, 1.00f, 0.25f, 0.10f);
                        Lighting.brightness = preBrightness;
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
