using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public override string BossHeadTexture => "ROI/NPCs/HeartOfTheWasteland/HeartOfTheWasteland_head0";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heart of the Wasteland");
            Main.npcFrameCount[npc.type] = 2;

            _tentacle = mod.GetTexture("NPCs/HeartOfTheWasteland/Heart_Tentacle");
        }


        public override void SetDefaults()
        {
            npc.width = (int) (158 * 1.7f);
            npc.height = (int) (222 * 1.7f);
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
            MoveToPoint(GetMiddleOfTriangle(_VinePosition[0], _VinePosition[1], _VinePosition[2]));
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
            internalAI[0] = Main.rand.Next(20, 40);
            internalAI[1] = (internalAI[1] == 0) ? 1 : 0;

            float searchVectorX = Main.rand.Next(5, 15);
            float searchVectorY = Main.rand.Next(5, 15);
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
            Vector2 npcPosition = npc.Center;
            int distanceFromTop = (int) (npc.Center.Y - _topBlock.Y);
            int textureHeight = _tentacle.Height;

            foreach (Vector2 vinePosition in _VinePosition)
            {
                float rotation = GetRadiantAngle(vinePosition, npcPosition);
                Vector2 distanceBetweenEntityAndWall = vinePosition - npcPosition;
                float temp2 = distanceBetweenEntityAndWall.Length();
                //do
                //{
                    int distanceToDraw = textureHeight;
                    
                    if (distanceFromTop < textureHeight)
                    {
                        distanceToDraw = distanceFromTop;
                    }

                    temp2 = distanceBetweenEntityAndWall.Length();
                    
                    if(temp2 > 0)
                        distanceBetweenEntityAndWall -= new Vector2(102, 222);
                    else
                        distanceBetweenEntityAndWall += new Vector2(102, 222);
                    

                    Vector2 temp = new Vector2(npcPosition.X - distanceBetweenEntityAndWall.X - (_tentacle.Width / 2) - Main.screenPosition.X, npcPosition.Y - distanceBetweenEntityAndWall.Y - Main.screenPosition.Y);
                    spriteBatch.Draw(_tentacle, vinePosition - Main.screenPosition, new Rectangle(0, 0, _tentacle.Width, distanceToDraw), drawColor, rotation, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                //} while (temp2 >=  0);
            }
            return true;
        }

        // TO DO: Move to Math helper
        public float GetRadiantAngle(Vector2 point1, Vector2 point2)
        {
            return (float) Math.Atan((point2.X - point1.X) / (point2.Y - point1.Y));
        }

        // TO DO: Move to Math helper
        public Vector2 GetMiddleOfTriangle(Vector2 point1, Vector2 point2, Vector2 point3)
        {
            return new Vector2((point1.X + point2.X + point3.X) / 3, (point1.Y + point2.Y + point3.Y) / 3);
        }

        // TO DO: Move to AI Helper
        public void MoveToPoint(Vector2 point, float moveSpeed = 1f, float velMultiplier = 1f)
        {
            Vector2 dist = point - npc.Center;
            float length = dist == Vector2.Zero ? 0f : dist.Length();
            if (length < moveSpeed)
            {
                velMultiplier = MathHelper.Lerp(0f, 1f, length / moveSpeed);
            }
            if (length < 200f)
            {
                moveSpeed *= 0.5f;
            }
            if (length < 100f)
            {
                moveSpeed *= 0.5f;
            }
            if (length < 50f)
            {
                moveSpeed *= 0.5f;
            }
            if (length < 10f)
            {
                moveSpeed *= 0.01f;
            }
            npc.velocity = length == 0f ? Vector2.Zero : Vector2.Normalize(dist);
            npc.velocity *= moveSpeed;
            npc.velocity *= velMultiplier;
        }
    }
}
