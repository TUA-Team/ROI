using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROI.Helpers;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ROI.NPCs.HeartOfTheWasteland
{
    /// <summary>
    /// ai[0] = searching player timer
    /// ai[1] =
    /// ai[2] =
    /// ai[3] =
    /// </summary>
    [AutoloadBossHead]
    class HeartOfTheWasteland : ModNPC
    {
        public bool IsSleeping { private get; set; }

        private const string BOSS_HEAD_PATH = "ROI/NPCs/HeartOfTheWasteland/HeartOfTheWasteland_head";

        private static Texture2D _tentacle;

        private Vector2 _topBlock;
        private Vector2[] _VinePosition = new Vector2[] { Vector2.Zero, Vector2.Zero, Vector2.Zero };
        private int nextVineToModify = 0;
        

        /// <summary>
        /// [0] = Movement Countdown <br />
        /// [1] = Next movement position
        /// </summary>
        private float[] internalAI = new float[2];

        public Player TargetPlayer
        {
            get
            {
                if (!npc.HasPlayerTarget)
                {
                    npc.TargetClosest();
                }
                return Main.player[npc.target];
            }
        }

        public bool Enrage
        {
            get { return TargetPlayer.DistanceSQ(npc.Center) > 16 * 100; }
        }

        public override string BossHeadTexture => "ROI/NPCs/HeartOfTheWasteland/HeartOfTheWasteland_head0";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heart of the Wasteland");
            Main.npcFrameCount[npc.type] = 2;

            _tentacle = mod.GetTexture("NPCs/HeartOfTheWasteland/Heart_Tentacle");
        }


        public override void SetDefaults()
        {
            npc.width = (int) (158);
            npc.height = (int) (222);
            npc.lifeMax = 9000;
            npc.damage = 60;
            npc.defense = 20;
            npc.knockBackResist = 0f;
            npc.value = Item.buyPrice(0, 20, 50, 25);
            npc.npcSlots = 0f;
            npc.lavaImmune = true;
            npc.noTileCollide = true;
            npc.boss = true;
            npc.immortal = true;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.scale = 2f;
            NPCID.Sets.MustAlwaysDraw[npc.type] = true;
            IsSleeping = true;

            for (int i = 0; i < npc.buffImmune.Length; i++)
            {
                npc.buffImmune[i] = true;
            }
        }

        public override bool PreAI()
        {
            if (_VinePosition[0] == Vector2.Zero && _VinePosition[1] == Vector2.Zero && _VinePosition[2] == Vector2.Zero)
            {
                for (int i = 0; i < 3; i++)
                {
                    _VinePosition[i] = npc.Center;
                }
                return false;
            }
            return base.PreAI();
        }

        public override void AI()
        {
            /*
            npc.boss = !IsSleeping;
            npc.dontTakeDamage = IsSleeping;

            if (IsSleeping)
            {
                return;
            }*/

            npc.ai[0]--;
            internalAI[0]--;

            if (npc.ai[0] <= 0)
            {
                npc.ai[0] = 3600;
                npc.TargetClosest(false);
            }

            if (internalAI[0] <= 0)
            {
                MoveVine();
            }

            if (Enrage)
            {
                ROIAIHelper.MoveToPoint(npc, ROIMathHelper.GetMiddleOfTriangle(_VinePosition[0], _VinePosition[1], _VinePosition[2]), 50f, 1.2f);
            }
            else
            {
                ROIAIHelper.MoveToPoint(npc, ROIMathHelper.GetMiddleOfTriangle(_VinePosition[0], _VinePosition[1], _VinePosition[2]));
            }
            
            return;
            for (int i = 0; i < Main.player.Length; i++)
            {
                Player player = Main.player[i];
                if (player.DistanceSQ(npc.position) < 22500) // 150 tiles
                {
                    if (!Main.dedServ) Main.NewText(Language.GetTextValue($"Mods.{mod.Name}.HotWFarAway{Main.rand.Next(4)}", npc.GivenName),
                        new Color(66, 244, 116));
                    player.position = npc.position +=
                        new Vector2(Main.rand.Next(-15, 15), Main.rand.Next(-15, 15));
                    var point = player.position.ToTileCoordinates();
                    if (!Main.dedServ && Main.tile[point.X, point.Y].nactive())
                        Main.NewText(Language.GetTextValue($"Mods.{mod.Name}.HotWFarAway{Main.rand.Next(4)}", npc.GivenName),
                            new Color(66, 244, 116));
                    // Item6 is magic mirror
                    if (!Main.dedServ) Main.PlaySound(SoundID.Item6, npc.position);
                }
            }
        }

        private void MoveVine()
        {
            internalAI[0] = (Enrage) ? Main.rand.Next(20, 40) : Main.rand.Next(50, 70);
            internalAI[1] = (internalAI[1] == 0) ? 1 : 0;

            float searchVectorX = (Enrage) ? Main.rand.Next(10, 30) : Main.rand.Next(5, 15);
            float searchVectorY = (Enrage) ? Main.rand.Next(10, 20) : Main.rand.Next(5, 15);
            if (npc.Center.X - TargetPlayer.Center.X > 0)
            {
                searchVectorX = -searchVectorX;
            }

            if (internalAI[1] > 0)
            {
                searchVectorY = -searchVectorY;
            }
            _VinePosition[nextVineToModify] = SearchBlockForVine(new Vector2(searchVectorX, searchVectorY)) * 16;
            nextVineToModify++;
            if (nextVineToModify > 2)
            {
                nextVineToModify = 0;
            }
        }

        private Vector2 SearchTopBlock()
        {
            Tile tile = Main.tile[(int)(npc.Center.X / 16), (int)(npc.Center.Y / 16)];
            int y = (int) (npc.Center.Y / 16);
            int x = (int) (npc.Center.X / 16);
            do
            {
                y--;
                tile = Main.tile[x, y];
            } while (!tile.active());

            return new Vector2(x * 16, y * 16);
        }

        private Vector2 SearchBlockForVine(Vector2 rotationSearch)
        {
            Point16 npcPosition = (npc.Center / 16).ToPoint16();
            Vector2 newVineLocation = npc.Center / 16;

            do
            {
                Vector2 collisionResult = Collision.TileCollision(newVineLocation, rotationSearch, 16, 16);
                newVineLocation += rotationSearch;
                if (!WorldGen.InWorld((int) newVineLocation.X, (int) newVineLocation.Y))
                {
                    return npc.Center / 16;
                }
            } while (!Main.tile[(int) newVineLocation.X, (int) newVineLocation.Y].active());

            return newVineLocation;
            
        } 

        public override bool CheckActive()
        {
            return false;
        }

        public override void BossHeadSlot(ref int index)
        {
            index = NPCHeadLoader.GetBossHeadSlot(BOSS_HEAD_PATH + (IsSleeping ? 0 : 1));
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = IsSleeping ? frameHeight : 0;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                writer.WriteVector2(_VinePosition[0]);
                writer.WriteVector2(_VinePosition[1]);
                writer.WriteVector2(_VinePosition[2]);

                writer.Write(internalAI[0]);
                writer.Write(internalAI[1]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                _VinePosition[0] = reader.ReadVector2();
                _VinePosition[1] = reader.ReadVector2();
                _VinePosition[2] = reader.ReadVector2();

                internalAI[0] = reader.ReadSingle();
                internalAI[1] = reader.ReadSingle();
            }
            base.ReceiveExtraAI(reader);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {

            for(int i = 0; i < 3; i++)
            {
                Vector2 startingPoint = new Vector2(_VinePosition[i].X + (float) (_tentacle.Width / 2), _VinePosition[i].Y + (float) (_tentacle.Height / 2));
                float num7 = npc.Center.X - startingPoint.X;
                float num8 = npc.Center.Y - startingPoint.Y;
                float rotation2 = (float) Math.Atan2(num8, num7) - 1.57f;
                bool flag3 = true;
                while (flag3)
                {
                    int num9 = 102;
                    int num10 = 222;
                    float num11 = (float) Math.Sqrt(num7 * num7 + num8 * num8);
                    if (num11 < (float) num10)
                    {
                        num9 = (int) num11 - num10 + num9;
                        flag3 = false;
                    }

                    num11 = (float) num9 / num11;
                    num7 *= num11;
                    num8 *= num11;
                    startingPoint.X += num7;
                    startingPoint.Y += num8;
                    num7 = npc.Center.X - startingPoint.X;
                    num8 = npc.Center.Y - startingPoint.Y;
                    Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int) startingPoint.X / 16, (int) (startingPoint.Y / 16f));
                    spriteBatch.Draw(_tentacle, new Vector2(startingPoint.X - Main.screenPosition.X, startingPoint.Y - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle(0, 0, _tentacle.Width, num9), color2, rotation2, new Vector2((float) _tentacle.Width * 0.5f, (float) _tentacle.Height * 0.5f), 1f, SpriteEffects.None, 0f);
                }
            }

            return true;
        }
    }
}
