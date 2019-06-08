using System.IO;
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

        private int movementTimer
        {
            set => npc.ai[0] = value;
            get => (int)npc.ai[0];
        }
        private bool _movementUp
        {
            set => npc.ai[1] = value ? 1 : 0;
            get => npc.ai[1] == 1;
        }
        private float _damageReduction
        {
            set => npc.ai[0] = value;
            get => npc.ai[0];
        }

        public PillarShieldColor ShieldColor { get; private set; }
        public int ShieldHealth { get; private set; }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Pillar");
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

            movementTimer = 100;
            _movementUp = false;
            _damageReduction = 0;

            ShieldColor = PillarShieldColor.Red;
            ShieldHealth = npc.lifeMax;
            for (int i = 0; i < 200; i++)
            {
                var possiblePillar = Main.npc[i];
                if (possiblePillar.modNPC is VoidPillar
                    && possiblePillar.active && i != npc.whoAmI)
                {
                    npc.Kill();
                }
            }
        }

        public override void AI()
        {
            PillarMovement();
            DecideAttack();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((byte) ShieldColor);
            writer.Write(movementTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            ShieldColor = (PillarShieldColor) reader.ReadByte();
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
                string genderPronoun = plr.Male ? "his" : "her";
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
            if (ShieldHealth > 0)
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
            ShieldHealth -=  damage - (int)(damage * _damageReduction);
            
            if (ShieldHealth <= 0 && ShieldColor != PillarShieldColor.Rainbow)
            {
                ShieldHealth = 20000;
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
                PlayerDeathReason death = PlayerDeathReason.ByCustomReason(Main.player[i].name + " life was consumed by the void.");
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

                Main.player[i].allDamage *= 0.75f;
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
