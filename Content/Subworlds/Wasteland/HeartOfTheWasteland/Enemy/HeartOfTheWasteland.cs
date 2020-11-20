using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROI.Content.NPCs;
using ROI.Content.Worlds;
using ROI.Helpers;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Subworlds.Wasteland.HeartOfTheWasteland.Enemy
{
    /// <summary>
    /// ai[0] = AI mode
    /// ai[1] = searching player timer
    /// ai[2] = If Moving, Laser charge left, if Spawning, projectile ID
    /// ai[3] =
    /// localAI[0] = Opacity while spawning
    /// </summary>
    [AutoloadBossHead]
    public sealed class HeartOfTheWasteland : ModNPC
    {
        public bool IsSleeping { get; private set; }

        private const string BOSS_HEAD_PATH = "ROI/Content/Subworlds/Wasteland/HeartOfTheWasteland/Enemy/HeartOfTheWasteland_head";

        private static Texture2D _tentacle;

        private Vector2 _topBlock;
        private Vector2[] _VinePosition = new Vector2[] { Vector2.Zero, Vector2.Zero, Vector2.Zero };
        private int nextVineToModify = 0;

        private int spawningCountdown;
        private float opacity = 0f;

        private int _nextSmallLaserCooldown = 100;
        private int _nextLaserAmount = 5;
        private int _previouslySpawnedLaserAmount = 0;

        /// <summary>
        /// [0] = Movement Countdown <br />
        /// [1] = Next movement position <br />
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
            get { return TargetPlayer.Distance(npc.Center) > 16 * 200; }
        }

        public override string BossHeadTexture => "ROI/Content/Subworlds/Wasteland/HeartOfTheWasteland/Enemy/HeartOfTheWasteland_head0";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heart of the Wasteland");
            Main.npcFrameCount[npc.type] = 2;

            _tentacle = mod.GetTexture("Content/Subworlds/Wasteland/HeartOfTheWasteland/Enemy/Heart_Tentacle");
        }


        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            bossLifeScale *= 1.3f * numPlayers;
            base.ScaleExpertStats(numPlayers, bossLifeScale);
        }

        public override void SetDefaults()
        {
            npc.width = 158;
            npc.height = 222;
            npc.lifeMax = 9000;
            npc.damage = 60;
            npc.defense = 20;
            npc.knockBackResist = 0f;
            npc.value = Item.buyPrice(0, 20, 50, 25);
            npc.npcSlots = 0f;
            npc.lavaImmune = true;
            npc.noTileCollide = true;
            npc.boss = true;
            //npc.immortal = true;
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
            if (ROIWorld.activeHotWID == -1)
            {
                ROIWorld.activeHotWID = npc.whoAmI;
            }
            if (ROIWorld.activeHotWID != npc.whoAmI)
            {
                npc.active = false;
                return false;
            }
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
            //npc.velocity = Vector2.Zero;
            switch ((HotWAiPhase)npc.ai[0])
            {
                case HotWAiPhase.Spawning:
                    if (!Main.projectile[(int)npc.ai[2]].active ||
                        Main.projectile[(int)npc.ai[2]].type != ModContent.ProjectileType<Projectiles.ClumpOfRadioactiveMeat>())
                    {
                        npc.ai[0] = (float)HotWAiPhase.Moving;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                    }

                    opacity += 0.002f;

                    for (int i = 0; i < 2; i++)
                    {
                        if (Main.rand.Next(5) == 0)
                            Dust.NewDust(Main.projectile[(int)npc.ai[2]].Center, 2, 2, DustID.GreenBlood, Main.rand.Next(-2, 2), Main.rand.Next(1, 3), 0, Color.GreenYellow, 1f);
                    }
                    break;
                case HotWAiPhase.Moving:
                    AIPhaseMoving();
                    break;
                case HotWAiPhase.Stalling:
                    //Insert laser and mob spawning stuff here
                    npc.ai[0] = (float)HotWAiPhase.Moving;
                    break;
            }

        }

        #region Moving phase
        private void AIPhaseMoving()
        {
            npc.ai[1]--;
            internalAI[0]--;

            if (npc.ai[1] <= 0)
            {
                npc.ai[1] = 3600;
                npc.TargetClosest(false);
            }

            if (internalAI[0] <= 0)
            {
                MoveVine();
            }

            if (Enrage)
            {
                _nextSmallLaserCooldown -= 3;
                GeneralNPCHelper.MoveToPoint(npc, NumberHelper.GetMiddleOfTriangle(_VinePosition[0], _VinePosition[1], _VinePosition[2]), 50f, 1.2f);
            }
            else
            {
                GeneralNPCHelper.MoveToPoint(npc, NumberHelper.GetMiddleOfTriangle(_VinePosition[0], _VinePosition[1], _VinePosition[2]), MovingSpeed());
            }

            _nextSmallLaserCooldown--;
            if (_nextSmallLaserCooldown < 0)
            {
                _nextSmallLaserCooldown = 15;

                Vector2 spawn = new Vector2(npc.position.X + npc.width / 2, npc.position.Y + npc.height / 2);
                float rotation = (float)Math.Atan2(TargetPlayer.Center.Y - spawn.Y, TargetPlayer.Center.X - spawn.X);

                Projectile.NewProjectile(spawn, rotation.ToRotationVector2() * 6, ProjectileID.EyeLaser, 100, 0.2f);

                _previouslySpawnedLaserAmount++;
                if (_previouslySpawnedLaserAmount >= _nextLaserAmount)
                {
                    MovingLaser();
                    _previouslySpawnedLaserAmount = 0;
                }
            }
        }



        private void MovingLaser()
        {
            if (npc.life > npc.lifeMax * 0.9)
            {
                _nextLaserAmount = 1;
                _nextSmallLaserCooldown = 200;
            }
            else if (npc.life > npc.lifeMax * 0.7)
            {
                _nextLaserAmount = Main.rand.Next(1, 2);
                _nextSmallLaserCooldown = 180;
            }
            else if (npc.life > npc.lifeMax * 0.5)
            {
                _nextLaserAmount = Main.rand.Next(2, 3);
                _nextSmallLaserCooldown = 150;
            }
            else if (!Main.expertMode)
            {
                if (npc.life > npc.lifeMax * 0.3)
                {
                    _nextLaserAmount = Main.rand.Next(2, 4);
                    _nextSmallLaserCooldown = 125;
                }
                else if (npc.life > npc.lifeMax * 0.1)
                {
                    _nextLaserAmount = Main.rand.Next(3, 5);
                    _nextSmallLaserCooldown = 125;
                }
                else
                {
                    _nextLaserAmount = 5;
                    _nextSmallLaserCooldown = 100;
                }
            }
        }

        private float MovingSpeed()
        {
            if (npc.life > npc.lifeMax * 0.9)
            {
                return 1f;
            }
            else if (npc.life > npc.lifeMax * 0.7)
            {
                return 2f;
            }
            else if (npc.life > npc.lifeMax * 0.5)
            {
                return 3f;
            }
            else if (npc.life > npc.lifeMax * 0.3)
            {
                return 5f;
            }
            else if (npc.life > npc.lifeMax * 0.1)
            {
                return 10f;
            }
            else
            {
                return 30f;
            }

        }

        private void MoveVine()
        {
            internalAI[0] = Enrage ? Main.rand.Next(20, 40) : Main.rand.Next(50, 70);
            internalAI[1] = internalAI[1] == 0 ? 1 : 0;

            float searchVectorX = Enrage ? Main.rand.Next(10, 30) : Main.rand.Next(5, 15);
            float searchVectorY = Enrage ? Main.rand.Next(10, 20) : Main.rand.Next(5, 15);
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
        #endregion


        private Vector2 SearchBlockForVine(Vector2 rotationSearch)
        {
            //Point16 npcPosition = (npc.Center / 16).ToPoint16();
            Vector2 newVineLocation = npc.Center / 16;

            do
            {
                //Vector2 collisionResult = Collision.TileCollision(newVineLocation, rotationSearch, 16, 16);
                newVineLocation += rotationSearch;
                if (!WorldGen.InWorld((int)newVineLocation.X, (int)newVineLocation.Y))
                {
                    return npc.Center / 16;
                }
            } while (!Main.tile[(int)newVineLocation.X, (int)newVineLocation.Y].active());

            return newVineLocation;

        }

        public override bool CheckActive()
        {
            if (Main.player.Any(i => !i.dead || i.position.Y / 16 > Main.maxTilesY - 200))
            {
                return false;
            }
            return true;
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

        public override void NPCLoot()
        {
            ROIWorld.activeHotWID = -1;
            if (!Main.hardMode)
            {
                Main.NewText("The spirits of the ancient gods have been unleashed.", Color.LightSeaGreen);
                WorldGen.StartHardmode();
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Vector2 npcPosition = new Vector2(npc.position.X - Main.screenPosition.X, npc.position.Y - Main.screenPosition.Y);

            switch ((HotWAiPhase)npc.ai[0])
            {
                case HotWAiPhase.Spawning:
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    PostSpawningDraw(spriteBatch);
                    spriteBatch.Draw(ModContent.GetTexture(Texture), npcPosition, new Rectangle(0, 0, 152, 222), Color.White * opacity, 0f, Vector2.Zero, new Vector2(2, 2), SpriteEffects.None, 1f);
                    spriteBatch.End();
                    spriteBatch.Begin();
                    break;
                case HotWAiPhase.Moving:
                case HotWAiPhase.Stalling:
                    PostSpawningDraw(spriteBatch);
                    spriteBatch.Draw(ModContent.GetTexture(Texture), npcPosition, new Rectangle(0, 0, 152, 222), Color.White, 0f, Vector2.Zero, new Vector2(2, 2), SpriteEffects.None, 1f);
                    break;
                case HotWAiPhase.dead:
                    break;
            }
            PostSpawningDraw(spriteBatch);

            return false;
        }

        private void PostSpawningDraw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 startingPoint = new Vector2(_VinePosition[i].X, _VinePosition[i].Y);
                DrawRepeatingTexture(spriteBatch, startingPoint, npc.Center, _tentacle, Color.White);
            }
        }

        private void DrawRepeatingTexture(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Texture2D texture, Color color)
        {
            Vector2 direction = end - start;
            float dist = direction.Length();
            direction.Normalize();
            float rotation = MathHelper.PiOver2 + (float)Math.Atan2(direction.Y, direction.X);
            int height = texture.Height;

            void DrawSection(int drawHeight)
            {
                spriteBatch.Draw(texture, start - direction * (height - drawHeight), new Rectangle(0, height - drawHeight, texture.Width, drawHeight), color, rotation, new Vector2(texture.Width * 0.5f, height), 1f, SpriteEffects.None, 0f);
            }

            while (dist > height)
            {
                DrawSection(height);
                dist -= height;
                start += direction * height;
            }

            DrawSection((int)dist);
        }

        private enum HotWAiPhase : byte
        {
            Spawning = 0,
            Stalling = 1,
            Moving = 2,
            dead = 3,
            subtitle = 4 //To be implemented

        }

        private enum HotWAiStallingPhase : byte
        {
            FollowingDeathRay = 0,
            SpinningDeathRay = 1
        }

        private enum HotWAiMovingPhase : byte
        {
            LaserSpam = 0,
            DeathRay = 1
        }
    }
}
