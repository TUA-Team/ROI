using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROI.Enums;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.NPCs.Void.VoidPillar
{
    /// <summary>
    /// AI slot usage
    /// <list type="bullet">
    /// 
    /// </list>>
    /// 0 -
    /// 1 -
    /// 2 -
    /// 3 -
    /// 
    /// </summary>
    internal sealed class VoidPillar : ModNPC
    {
        public override string Texture => "Terraria/NPC_507";

        private int movementTimer = 100;
        private PillarShieldColor _shieldColor = PillarShieldColor.Red;
        private bool _movementUp = false;
        private int _shieldHealth = 20000;
        private float _damageReduction = 0.0f;

        public PillarShieldColor ShieldColor => _shieldColor;
        public int ShieldHealth => _shieldHealth;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Pillar - Manifestation of the void");
        }

        public override void SetDefaults()
        {
            npc.boss = true;
            npc.width = 174;
            npc.height = 364;
            npc.aiStyle = -1;
            npc.buffImmune.SetAllTrue();
            npc.lifeMax = 20000;
            music = MusicID.LunarBoss;
            npc.noGravity = true;
            npc.immortal = true;
            npc.knockBackResist = 0f;
        }

        public override void AI()
        {
            PillarMovement();
            DecideAttack();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((byte) _shieldColor);
            writer.Write(movementTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            _shieldColor = (PillarShieldColor) reader.ReadByte();
            movementTimer = reader.Read();
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            DamageShield(damage);
            if (_shieldColor == PillarShieldColor.Green)
            {
                string genderPronoun = (player.Male) ? "his" : "her";
                PlayerDeathReason deathReason = PlayerDeathReason.ByCustomReason($"Got slain by {genderPronoun} own sword.");
                player.Hurt(deathReason, damage / 10, 0, false, false, crit);
            }
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            DamageShield(damage);

            if (_shieldColor == PillarShieldColor.Green)
            {
                string genderPronoun = (Main.player[projectile.owner].Male) ? "his" : "her";
                PlayerDeathReason deathReason = PlayerDeathReason.ByCustomReason($"Got shredded by {genderPronoun} own projectile.");
                Main.player[projectile.owner].Hurt(deathReason, damage / 10, 0, false, false, crit);
            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (_shieldHealth > 0)
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
            
        }

        private void DamageShield(int damage)
        {
            _shieldHealth -=  damage - (int)(damage * _damageReduction);
            
            if (_shieldHealth <= 0 && ShieldColor != PillarShieldColor.Rainbow)
            {
                _shieldHealth = 20000;
                SwitchShieldColor();
            }
        }

        private void SwitchShieldColor()
        {
            switch (ShieldColor)
            {
                case PillarShieldColor.Red:
                    _damageReduction = 0.2f;
                    _shieldColor = PillarShieldColor.Purple;
                    break;
                case PillarShieldColor.Purple:
                    _shieldColor = PillarShieldColor.Black;
                    _damageReduction = 0.9f;
                    break;
                case PillarShieldColor.Black:
                    _shieldColor = PillarShieldColor.Green;
                    _damageReduction = 0.2f;
                    break;
                case PillarShieldColor.Green:
                    _shieldColor = PillarShieldColor.Blue;
                    _damageReduction = 0.5f;
                    break;
                case PillarShieldColor.Blue:
                    _shieldColor = PillarShieldColor.Rainbow;
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
        }

        private void DecideAttack()
        {
            switch (ShieldColor)
            {
                case PillarShieldColor.Red:
                    break;
                case PillarShieldColor.Purple:
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
                if (Main.player[i] == null)
                {
                    continue;
                }
                PlayerDeathReason death = PlayerDeathReason.ByCustomReason(Main.player[i].name + " life was consumed by a manifestation of the void.");
                Main.player[i].Hurt(death, 5, 0, false, true, false, 5);
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
                    Main.player[i].AddBuff(BuffID.Suffocation, 60 * 5, false);
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
    }
}
