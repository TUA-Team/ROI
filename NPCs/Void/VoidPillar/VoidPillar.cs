using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROI.ID;
using ROI.NPCs.Interfaces;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ROI.NPCs.Void.VoidPillar
{
    internal sealed partial class VoidPillar : ModNPC, ISavableEntity
    {
        public override string Texture => "Terraria/NPC_507";

        private int movementTimer;
        private bool _movementUp;
        private float _damageReduction;

        public PillarShieldColor ShieldColor { get; internal set; }

        public int ShieldHealth { get; internal set; }

        public override void SetDefaults()
        {
            npc.boss = true;
            npc.width = 174;
            npc.height = 364;
            npc.aiStyle = -1;
            npc.buffImmune.SetAllTrue();
            npc.lifeMax = (Main.expertMode) ? 25000 : 20000;
            music = MusicID.LunarBoss;
            npc.noGravity = true;
            npc.immortal = true;
            npc.knockBackResist = 0f;

            ShieldColor = PillarShieldColor.Red;
            ShieldHealth = (Main.expertMode) ? 25000 : 20000;

            movementTimer = 100;
            _movementUp = false;
            _damageReduction = Main.expertMode ? 0.2f : 0; //Set red shield damage reduction here
            //if (Main.npc.Where(i => i.modNPC is VoidPillar).ToList().Count > 1)
            //{
            //    npc.ForceKill();
            //}
        }

        public override void AI()
        {
            PillarMovement();
            DecideAttack();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(ShieldHealth);
            writer.Write((byte)ShieldColor);
            writer.Write(movementTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            ShieldHealth = reader.ReadInt32();
            ShieldColor = (PillarShieldColor)reader.ReadByte();
            movementTimer = reader.Read();
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            DamageShield(damage);

            if (ShieldColor == PillarShieldColor.Green)
            {
                string gp1 = player.Male ? "himself" : "herself";
                string gp2 = player.Male ? "his" : "her";
                PlayerDeathReason deathReason = PlayerDeathReason.ByCustomReason(
                    Main.rand.Next(new string[] {string.Format("{0} impaled {1}", player.name, gp1),
                    string.Format("{0} swung {1} sword and the sword hit them back", player.name, gp2) }));
                player.Hurt(deathReason, damage / 10, 0, false, false, crit);
            }
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            DamageShield(damage);

            if (ShieldColor == PillarShieldColor.Green)
            {
                var plr = Main.player[projectile.owner];
                PlayerDeathReason deathReason = PlayerDeathReason.ByCustomReason(
                    string.Format("{0} threw something and was unlucky enough that it came back", plr));
                plr.Hurt(deathReason, damage / 10, 0, false, false, crit);
            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
            Vector2 center = npc.Center - Main.screenPosition;
            DrawData drawingData = new DrawData(TextureManager.Load("Images/Misc/Perlin"), center - new Vector2(0, 10), new Rectangle(0, 0, 600, 600), GetShieldColor(), npc.rotation, new Vector2(300, 300), Vector2.One, SpriteEffects.None, 0);
            GameShaders.Misc["ForceField"].UseColor(Main.DiscoColor);
            GameShaders.Misc["ForceField"].Apply(drawingData);
            drawingData.Draw(Main.spriteBatch);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin();
        }

        public override bool CheckActive() => false;

        private void DamageShield(int damage)
        {
            ShieldHealth -= damage - (int)(damage * _damageReduction);
            npc.life -= damage - (int)(damage * _damageReduction);

            if (ShieldHealth <= 0 && ShieldColor != PillarShieldColor.Rainbow)
            {
                ShieldHealth = (Main.expertMode) ? 25000 : 20000;
                npc.life = (Main.expertMode) ? 25000 : 20000;
                SwitchShieldColor();
            }
        }

        private void SwitchShieldColor()
        {
            switch (ShieldColor)
            {
                case PillarShieldColor.Red:
                    _damageReduction = 0.2f;
                    ShieldColor = PillarShieldColor.Purple;
                    break;
                case PillarShieldColor.Purple:
                    ShieldColor = PillarShieldColor.Black;
                    _damageReduction = 0.9f;
                    break;
                case PillarShieldColor.Black:
                    ShieldColor = PillarShieldColor.Green;
                    _damageReduction = 0.2f;
                    break;
                case PillarShieldColor.Green:
                    ShieldColor = PillarShieldColor.Blue;
                    _damageReduction = 0.5f;
                    break;
                case PillarShieldColor.Blue:
                    ShieldColor = PillarShieldColor.Rainbow;
                    _damageReduction = 0.8f;
                    npc.immortal = false;
                    break;
            }
        }

        public Color GetShieldColor()
        {
            switch (ShieldColor)
            {
                case PillarShieldColor.Black:
                    return Color.Black;
                case PillarShieldColor.Blue:
                    return Color.Blue;
                case PillarShieldColor.Green:
                    return Color.Green;
                case PillarShieldColor.Purple:
                    return Color.Purple;
                case PillarShieldColor.Red:
                    return Color.Red;
                default:
                    return Main.DiscoColor;
            }
            /*
            return ShieldColor switch
            {
                PillarShieldColor.Black => Color.Black,
                PillarShieldColor.Blue => Color.Blue,
                PillarShieldColor.Green => Color.Green,
                PillarShieldColor.Purple => Color.Purple,
                PillarShieldColor.Red => Color.Red,
                _ => Color.Red
            };
            */
        }

        private void DecideAttack()
        {
            switch (ShieldColor)
            {
                case PillarShieldColor.Red:
                    DebuffRed();
                    break;
                case PillarShieldColor.Purple:
                    DebuffPurple();
                    break;
                case PillarShieldColor.Black:
                    DebuffBlack();
                    break;
                case PillarShieldColor.Green:
                    break;
                case PillarShieldColor.Blue:
                    break;
                default:
                    break;
            }
        }

        #region Pillar debuff

        /// <summary>
        /// Red color shield, will drain health from the player directly
        /// </summary>
        private void DebuffRed()
        {
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i].name == "")
                {
                    continue;
                }
                PlayerDeathReason death = PlayerDeathReason.ByCustomReason(Main.player[i].name + " life was consumed by his avatar from the void.");
                Main.player[i].Hurt(death, (Main.expertMode) ? 50 : 25, 0, false, true, false, 5);
                Main.player[i].lifeRegen = 0;
            }
        }

        /// <summary>
        /// Purple color shield, weaken the player by 75%
        /// </summary>
        private void DebuffPurple()
        {
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i] == null)
                {
                    continue;
                }

                Main.player[i].allDamage /= 0.75f;
            }
        }

        /// <summary>
        /// Black shield color, the player 
        /// </summary>
        private void DebuffBlack()
        {
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i] == null)
                {
                    continue;
                }

                Main.player[i].headcovered = true;
                
                if (Main.rand.Next(1000) == 0)
                {
                    Main.player[i].AddBuff(BuffID.Suffocation, (Main.expertMode) ? 60 * 5 : 60 * 10, false);
                }

            }
        }

        #endregion

        private void PillarMovement()
        {
            movementTimer--;
            if (movementTimer == 0)
            {
                movementTimer = 100;
                _movementUp = !_movementUp;
            }

            if (_movementUp)
            {
                npc.position.Y -= 0.2f;
            }
            else
            {
                npc.position.Y += 0.2f;
            }
        }

        public TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            tag.Add("shieldPhase", (byte)ShieldColor);
            tag.Add("shieldHealth", ShieldHealth);
            return tag;
        }

        public void Load(TagCompound data)
        {
            ShieldColor = (PillarShieldColor) data.GetByte("shieldPhase");
            ShieldHealth = data.GetAsInt("shieldHealth");
        }

        public bool SaveHP => true;
    }
}
