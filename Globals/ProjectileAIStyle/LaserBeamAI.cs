using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ROI.Projectiles.HeartOfTheWasteland;
using Terraria;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Globals.ProjectileAIStyle
{
    /// <summary>
    /// LaserBeamAI:<br />
    /// <b>ai[0]</b> = Read note about it<br />
    /// <b>ai[1]</b> = Projectile owner<br />
    /// <b>LocalAI[0]</b> = to be found<br />
    /// <b>LocalAI[1]</b> = to be found<br />
    /// <br />
    /// ai[0] use on different projectile<br />
    /// <b>Charged cannon blaster laser (ID: 461)</b> = Laser lifetime, if above 300 (5 sec) kill it<br />
    /// <b>Moon lord death ray (small and big, ID: 455)</b> = Speed of the death ray rotation?<br />
    /// </summary>
    public partial class ProjectileAIStyle
    {
        
        public static void DeathRayAIStyle(Projectile projectile)
        {
            Vector2? centerPoint = null;
            if (projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
                projectile.velocity = -Vector2.UnitY;

            if (CenteringLaserProjectileOnAPoint(projectile, ref centerPoint)) return;

            if (projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
                projectile.velocity = -Vector2.UnitY;

            if (LaserAIUpdate(projectile)) return;

            float rotation = projectile.velocity.ToRotation();
            if (projectile.type == 455)
                rotation += projectile.ai[0];
            else if (projectile.type == ModContent.ProjectileType<HotWTracingLaser>())
            {
                rotation += projectile.localAI[0];
            }


            projectile.rotation = rotation - (float)Math.PI / 2f;
            projectile.velocity = rotation.ToRotationVector2();
            float samplesSize = 0f;
            float samplingWidth = 0f;
            Vector2 samplingPoint = projectile.Center;
            if (centerPoint.HasValue)
            {
                samplingPoint = centerPoint.Value;
            }

            SamplesSize(projectile, ref samplesSize, ref samplingWidth);

            float[] samples = new float[(int)samplesSize];
            Collision.LaserScan(samplingPoint, projectile.velocity, samplingWidth * projectile.scale, 2400f, samples);
            float distanceBetweenBeamAndTargetPoint = 0f;
            for (int i = 0; i < samples.Length; i++)
            {
                distanceBetweenBeamAndTargetPoint += samples[i];
            }

            distanceBetweenBeamAndTargetPoint /= samplesSize;
            float amount = 0.5f;
            if (projectile.type == ProjectileID.PhantasmalDeathray)
            {
                distanceBetweenBeamAndTargetPoint = PhantasmalDeathRayPlayerHit(projectile, distanceBetweenBeamAndTargetPoint, ref amount);
            }

            if (projectile.type == 632)
                amount = 0.75f;

            projectile.localAI[1] = MathHelper.Lerp(projectile.localAI[1], distanceBetweenBeamAndTargetPoint, amount);
            LaserBeamDustSpawning(projectile);

            if (projectile.type != ProjectileID.LastPrismLaser || !(Math.Abs(projectile.localAI[1] - distanceBetweenBeamAndTargetPoint) < 100f) || !(projectile.scale > 0.15f))
                return;
            LastPrismDust(projectile);
        }

        private static void LastPrismDust(Projectile projectile)
        {
            float laserLuminance = 0.5f;
            float laserAlphaMultiplier = 0f;
            float lastPrismHue = projectile.GetPrismHue(projectile.ai[0]);
            Color lastPrismColorWithoutAlphaSet = Main.hslToRgb(lastPrismHue, 1f, laserLuminance);
            lastPrismColorWithoutAlphaSet.A = (byte) ((float) (int) lastPrismColorWithoutAlphaSet.A * laserAlphaMultiplier);
            Color lastPrismColor = lastPrismColorWithoutAlphaSet;
            Vector2 dustPosition = projectile.Center + projectile.velocity * (projectile.localAI[1] - 14.5f * projectile.scale);
            float x3 = Main.rgbToHsl(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB)).X;
            for (int i = 0; i < 2; i++)
            {
                float rawDustSpeed = projectile.velocity.ToRotation() + ((Main.rand.Next(2) == 1) ? (-1f) : 1f) * ((float) Math.PI / 2f);
                float rawDustSpeedModifier = (float) Main.rand.NextDouble() * 0.8f + 1f;
                Vector2 dustSpeed = new Vector2((float) Math.Cos(rawDustSpeed) * rawDustSpeedModifier, (float) Math.Sin(rawDustSpeed) * rawDustSpeedModifier);

                //This might ber for all dust actually
                Dust initialLastPrismDust = Dust.NewDustDirect(dustPosition, 0, 0, 267, dustSpeed.X, dustSpeed.Y);
                //int initialLastPrismDust = Dust.NewDust(vector48, 0, 0, 267, vector49.X, vector49.Y);
                initialLastPrismDust.color = lastPrismColorWithoutAlphaSet;
                initialLastPrismDust.scale = 1.2f;
                if (projectile.scale > 1f)
                {
                    initialLastPrismDust.velocity *= projectile.scale;
                    initialLastPrismDust.scale *= projectile.scale;
                }

                initialLastPrismDust.noGravity = true;
                if (projectile.scale != 1.4f && initialLastPrismDust.dustIndex != 6000)
                {
                    Dust lastPrismCloneDust = Dust.CloneDust(initialLastPrismDust);
                    lastPrismCloneDust.color = Color.White;
                    lastPrismCloneDust.scale /= 2f;
                }

                float hue = (x3 + Main.rand.NextFloat() * 0.4f) % 1f;
                initialLastPrismDust.color = Color.Lerp(lastPrismColorWithoutAlphaSet, Main.hslToRgb(hue, 1f, 0.75f), projectile.scale / 1.4f);
            }

            if (Main.rand.Next(5) == 0)
            {
                Vector2 value34 = projectile.velocity.RotatedBy(MathHelper.PiOver2) * ((float) Main.rand.NextDouble() - 0.5f) * projectile.width;
                Dust secondaryDust = Dust.NewDustDirect(dustPosition + value34 - Vector2.One * 4f, 8, 8, 31, 0f, 0f, 100, default(Color), 1.5f);
                secondaryDust.velocity *= 0.5f;
                secondaryDust.velocity.Y = 0f - Math.Abs(secondaryDust.velocity.Y);
            }

            DelegateMethods.v3_1 = lastPrismColorWithoutAlphaSet.ToVector3() * 0.3f;
            float drippleColorModifier = 0.1f * (float) Math.Sin(ROIMod.gameTime.TotalGameTime.TotalSeconds % 3600.0 * 20f);
            Vector2 size = new Vector2(projectile.velocity.Length() * projectile.localAI[1], (float) projectile.width * projectile.scale);
            float prjectileVelocityToRotation = projectile.velocity.ToRotation();
            if (Main.netMode != 2)
                ((WaterShaderData) Filters.Scene["WaterDistortion"].GetShader()).QueueRipple(projectile.position + new Vector2(size.X * 0.5f, 0f).RotatedBy(prjectileVelocityToRotation), new Color(0.5f, 0.1f * (float) Math.Sign(drippleColorModifier) + 0.5f, 0f, 1f) * Math.Abs(drippleColorModifier), size, RippleShape.Square, prjectileVelocityToRotation);

            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], (float) projectile.width * projectile.scale, DelegateMethods.CastLight);
        }

        private static void SamplesSize(Projectile projectile, ref float samplesSize, ref float samplingWidth)
        {
            switch (projectile.type)
            {
                case 455:
                    samplesSize = 3f;
                    samplingWidth = projectile.width;
                    break;
                case 461:
                    samplesSize = 2f;
                    samplingWidth = 0f;
                    break;
                case 642:
                    samplesSize = 2f;
                    samplingWidth = 0f;
                    break;
                case 632:
                    samplesSize = 2f;
                    samplingWidth = 0f;
                    break;
                case 537:
                    samplesSize = 2f;
                    samplingWidth = 0f;
                    break;
            }
        }

        private static float PhantasmalDeathRayPlayerHit(Projectile projectile, float distanceBetweenBeamAndTargetPoint, ref float amount)
        {
            NPC moonLordHeadEye = Main.npc[(int)projectile.ai[1]];
            if (moonLordHeadEye.type == 396)
            {
                Player player8 = Main.player[moonLordHeadEye.target];
                if (!Collision.CanHitLine(moonLordHeadEye.position, moonLordHeadEye.width, moonLordHeadEye.height, player8.position, player8.width, player8.height))
                {
                    distanceBetweenBeamAndTargetPoint = Math.Min(2400f, Vector2.Distance(moonLordHeadEye.Center, player8.Center) + 150f);
                    amount = 0.75f;
                }
            }

            return distanceBetweenBeamAndTargetPoint;
        }

        private static void LaserBeamDustSpawning(Projectile projectile)
        {
            switch (projectile.type)
            {
                case 455:
                    {
                        Vector2 vector40 = projectile.Center + projectile.velocity * (projectile.localAI[1] - 14f);
                        for (int num695 = 0; num695 < 2; num695++)
                        {
                            float num696 = projectile.velocity.ToRotation() + ((Main.rand.Next(2) == 1) ? (-1f) : 1f) * ((float)Math.PI / 2f);
                            float num697 = (float)Main.rand.NextDouble() * 2f + 2f;
                            Vector2 vector41 = new Vector2((float)Math.Cos(num696) * num697, (float)Math.Sin(num696) * num697);
                            int num698 = Dust.NewDust(vector40, 0, 0, 229, vector41.X, vector41.Y);
                            Main.dust[num698].noGravity = true;
                            Main.dust[num698].scale = 1.7f;
                        }

                        if (Main.rand.Next(5) == 0)
                        {
                            Vector2 value30 = projectile.velocity.RotatedBy(MathHelper.PiOver2) * ((float)Main.rand.NextDouble() - 0.5f) * projectile.width;
                            int num699 = Dust.NewDust(vector40 + value30 - Vector2.One * 4f, 8, 8, 31, 0f, 0f, 100, default(Color), 1.5f);
                            Dust dust = Main.dust[num699];
                            dust.velocity *= 0.5f;
                            Main.dust[num699].velocity.Y = 0f - Math.Abs(Main.dust[num699].velocity.Y);
                        }

                        DelegateMethods.v3_1 = new Vector3(0.3f, 0.65f, 0.7f);
                        Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], (float)projectile.width * projectile.scale, DelegateMethods.CastLight);
                        break;
                    }
                case 642:
                    {
                        Vector2 vector42 = projectile.Center + projectile.velocity * (projectile.localAI[1] - 14f);
                        for (int num700 = 0; num700 < 2; num700++)
                        {
                            float num701 = projectile.velocity.ToRotation() + ((Main.rand.Next(2) == 1) ? (-1f) : 1f) * ((float)Math.PI / 2f);
                            float num702 = (float)Main.rand.NextDouble() * 2f + 2f;
                            Vector2 vector43 = new Vector2((float)Math.Cos(num701) * num702, (float)Math.Sin(num701) * num702);
                            int num703 = Dust.NewDust(vector42, 0, 0, 229, vector43.X, vector43.Y);
                            Main.dust[num703].noGravity = true;
                            Main.dust[num703].scale = 1.7f;
                        }

                        if (Main.rand.Next(5) == 0)
                        {
                            Vector2 value31 = projectile.velocity.RotatedBy(MathHelper.PiOver2) * ((float)Main.rand.NextDouble() - 0.5f) * projectile.width;
                            int num704 = Dust.NewDust(vector42 + value31 - Vector2.One * 4f, 8, 8, 31, 0f, 0f, 100, default(Color), 1.5f);
                            Dust dust = Main.dust[num704];
                            dust.velocity *= 0.5f;
                            Main.dust[num704].velocity.Y = 0f - Math.Abs(Main.dust[num704].velocity.Y);
                        }

                        DelegateMethods.v3_1 = new Vector3(0.3f, 0.65f, 0.7f);
                        Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], (float)projectile.width * projectile.scale, DelegateMethods.CastLight);
                        break;
                    }
                case 461:
                    {
                        Vector2 vector44 = projectile.Center + projectile.velocity * (projectile.localAI[1] - 8f);
                        for (int num705 = 0; num705 < 2; num705++)
                        {
                            float num706 = projectile.velocity.ToRotation() + ((Main.rand.Next(2) == 1) ? (-1f) : 1f) * ((float)Math.PI / 2f);
                            float num707 = (float)Main.rand.NextDouble() * 0.8f + 1f;
                            Vector2 vector45 = new Vector2((float)Math.Cos(num706) * num707, (float)Math.Sin(num706) * num707);
                            int num708 = Dust.NewDust(vector44, 0, 0, 226, vector45.X, vector45.Y);
                            Main.dust[num708].noGravity = true;
                            Main.dust[num708].scale = 1.2f;
                        }

                        if (Main.rand.Next(5) == 0)
                        {
                            Vector2 value32 = projectile.velocity.RotatedBy(MathHelper.PiOver2) * ((float)Main.rand.NextDouble() - 0.5f) * projectile.width;
                            int num709 = Dust.NewDust(vector44 + value32 - Vector2.One * 4f, 8, 8, 31, 0f, 0f, 100, default(Color), 1.5f);
                            Dust dust = Main.dust[num709];
                            dust.velocity *= 0.5f;
                            Main.dust[num709].velocity.Y = 0f - Math.Abs(Main.dust[num709].velocity.Y);
                        }

                        DelegateMethods.v3_1 = new Vector3(0.4f, 0.85f, 0.9f);
                        Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], (float)projectile.width * projectile.scale, DelegateMethods.CastLight);
                        break;
                    }
                case 537:
                    {
                        Vector2 vector46 = projectile.Center + projectile.velocity * (projectile.localAI[1] - 8f);
                        for (int num710 = 0; num710 < 2; num710++)
                        {
                            float num711 = projectile.velocity.ToRotation() + ((Main.rand.Next(2) == 1) ? (-1f) : 1f) * ((float)Math.PI / 2f);
                            float num712 = (float)Main.rand.NextDouble() * 0.8f + 1f;
                            Vector2 vector47 = new Vector2((float)Math.Cos(num711) * num712, (float)Math.Sin(num711) * num712);
                            int num713 = Dust.NewDust(vector46, 0, 0, 226, vector47.X, vector47.Y);
                            Main.dust[num713].noGravity = true;
                            Main.dust[num713].scale = 1.2f;
                        }

                        if (Main.rand.Next(5) == 0)
                        {
                            Vector2 value33 = projectile.velocity.RotatedBy(MathHelper.PiOver2) * ((float)Main.rand.NextDouble() - 0.5f) * projectile.width;
                            int num714 = Dust.NewDust(vector46 + value33 - Vector2.One * 4f, 8, 8, 31, 0f, 0f, 100, default(Color), 1.5f);
                            Dust dust = Main.dust[num714];
                            dust.velocity *= 0.5f;
                            Main.dust[num714].velocity.Y = 0f - Math.Abs(Main.dust[num714].velocity.Y);
                        }

                        DelegateMethods.v3_1 = new Vector3(0.4f, 0.85f, 0.9f);
                        Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], (float)projectile.width * projectile.scale, DelegateMethods.CastLight);
                        break;
                    }
            }
        }

        private static bool LaserAIUpdate(Projectile projectile)
        {
            switch (projectile.type)
            {
                case ProjectileID.ChargedBlasterLaser:
                    {
                        projectile.ai[0]++;
                        if (projectile.ai[0] >= 300f)
                        {
                            projectile.Kill();
                            return true;
                        }

                        projectile.scale = (float)Math.Sin(projectile.ai[0] * (float)Math.PI / 300f) * 10f;
                        if (projectile.scale > 1f)
                            projectile.scale = 1f;
                        break;
                    }
                case ProjectileID.PhantasmalDeathray:
                    {
                        if (projectile.localAI[0] == 0f)
                            Main.PlaySound(SoundID.Zombie, (int)projectile.position.X, (int)projectile.position.Y, 104);

                        float num687 = 1f;
                        if (Main.npc[(int)projectile.ai[1]].type == NPCID.MoonLordFreeEye)
                            num687 = 0.4f;

                        projectile.localAI[0]++;
                        if (projectile.localAI[0] >= 180f)
                        {
                            projectile.Kill();
                            return true;
                        }

                        projectile.scale = (float)Math.Sin(projectile.localAI[0] * (float)Math.PI / 180f) * 10f * num687;
                        if (projectile.scale > num687)
                            projectile.scale = num687;
                        break;
                    }
                case 642:
                    {
                        float num688 = 1f;
                        projectile.localAI[0]++;
                        if (projectile.localAI[0] >= 50f)
                        {
                            projectile.Kill();
                            return true;
                        }

                        projectile.scale = (float)Math.Sin(projectile.localAI[0] * (float)Math.PI / 50f) * 10f * num688;
                        if (projectile.scale > num688)
                            projectile.scale = num688;
                        break;
                    }
                case 537:
                    {
                        float num689 = 0.8f;
                        projectile.localAI[0]++;
                        if (projectile.localAI[0] >= 60f)
                        {
                            projectile.Kill();
                            return true;
                        }

                        projectile.scale = (float)Math.Sin(projectile.localAI[0] * (float)Math.PI / 60f) * 10f * num689;
                        if (projectile.scale > num689)
                            projectile.scale = num689;
                        break;
                    }
            }

            return false;
        }

        private static bool CenteringLaserProjectileOnAPoint(Projectile projectile, ref Vector2? vector39)
        {
            switch (projectile.type)
            {
                //DeathRay for the head centering
                case ProjectileID.PhantasmalDeathray when Main.npc[(int)projectile.ai[1]].active && Main.npc[(int)projectile.ai[1]].type == NPCID.MoonLordHead:
                    {
                        if (Main.npc[(int)projectile.ai[1]].ai[0] == -2f)
                        {
                            projectile.Kill();
                            return true;
                        }

                        Vector2 value25 = Utils.Vector2FromElipse(elipseSizes: new Vector2(27f, 59f) * Main.npc[(int)projectile.ai[1]].localAI[1], angleVector: Main.npc[(int)projectile.ai[1]].localAI[0].ToRotationVector2());
                        projectile.position = Main.npc[(int)projectile.ai[1]].Center + value25 - new Vector2(projectile.width, projectile.height) / 2f;
                        break;
                    }
                //DeathRay for the true eye centering
                case ProjectileID.PhantasmalDeathray when Main.npc[(int)projectile.ai[1]].active && Main.npc[(int)projectile.ai[1]].type == 400:
                    {
                        Vector2 value26 = Utils.Vector2FromElipse(elipseSizes: new Vector2(30f, 30f) * Main.npc[(int)projectile.ai[1]].localAI[1], angleVector: Main.npc[(int)projectile.ai[1]].localAI[0].ToRotationVector2());
                        projectile.position = Main.npc[(int)projectile.ai[1]].Center + value26 - new Vector2(projectile.width, projectile.height) / 2f;
                        break;
                    }
                case ProjectileID.StardustSoldierLaser when Main.npc[(int)projectile.ai[1]].active && Main.npc[(int)projectile.ai[1]].type == 411:
                    {
                        Vector2 value27 = new Vector2(Main.npc[(int)projectile.ai[1]].direction * 6, -4f);
                        projectile.position = Main.npc[(int)projectile.ai[1]].Center + value27 - projectile.Size / 2f + new Vector2(0f, 0f - Main.npc[(int)projectile.ai[1]].gfxOffY);
                        break;
                    }
                case ProjectileID.ChargedBlasterLaser when Main.projectile[(int)projectile.ai[1]].active && Main.projectile[(int)projectile.ai[1]].type == 460:
                    {
                        Vector2 value28 = Vector2.Normalize(Main.projectile[(int)projectile.ai[1]].velocity);
                        projectile.position = Main.projectile[(int)projectile.ai[1]].Center + value28 * 16f - new Vector2(projectile.width, projectile.height) / 2f + new Vector2(0f, 0f - Main.projectile[(int)projectile.ai[1]].gfxOffY);
                        projectile.velocity = Vector2.Normalize(Main.projectile[(int)projectile.ai[1]].velocity);
                        break;
                    }
                //Lunar portal laser, not moon lord turret laser :P
                case ProjectileID.MoonlordTurretLaser when Main.projectile[(int)projectile.ai[1]].active && Main.projectile[(int)projectile.ai[1]].type == 641:
                    projectile.Center = Main.projectile[(int)projectile.ai[1]].Center;
                    projectile.velocity = Vector2.Normalize(Main.projectile[(int)projectile.ai[1]].ai[1].ToRotationVector2());
                    break;
                default:
                    {
                        if (projectile.type != ProjectileID.LastPrismLaser || !Main.projectile[(int)projectile.ai[1]].active || Main.projectile[(int)projectile.ai[1]].type != ProjectileID.LastPrism)
                        {
                            projectile.Kill();
                            return true;
                        }

                        float num680 = (float)(int)projectile.ai[0] - 2.5f;
                        Vector2 value29 = Vector2.Normalize(Main.projectile[(int)projectile.ai[1]].velocity);
                        Projectile tempName = Main.projectile[(int)projectile.ai[1]];
                        float num681 = num680 * ((float)Math.PI / 6f);
                        float num682 = 20f;
                        Vector2 zero = Vector2.Zero;
                        float num683 = 1f;
                        float num684 = 15f;
                        float num685 = -2f;
                        if (tempName.ai[0] < 180f)
                        {
                            num683 = 1f - tempName.ai[0] / 180f;
                            num684 = 20f - tempName.ai[0] / 180f * 14f;
                            if (tempName.ai[0] < 120f)
                            {
                                num682 = 20f - 4f * (tempName.ai[0] / 120f);
                                projectile.Opacity = tempName.ai[0] / 120f * 0.4f;
                            }
                            else
                            {
                                num682 = 16f - 10f * ((tempName.ai[0] - 120f) / 60f);
                                projectile.Opacity = 0.4f + (tempName.ai[0] - 120f) / 60f * 0.6f;
                            }

                            num685 = -22f + tempName.ai[0] / 180f * 20f;
                        }
                        else
                        {
                            num683 = 0f;
                            num682 = 1.75f;
                            num684 = 6f;
                            tempName.Opacity = 1f;
                            num685 = -2f;
                        }

                        float num686 = (tempName.ai[0] + num680 * num682) / (num682 * 6f) * ((float)Math.PI * 2f);
                        num681 = Vector2.UnitY.RotatedBy(num686).Y * ((float)Math.PI / 6f) * num683;
                        zero = (Vector2.UnitY.RotatedBy(num686) * new Vector2(4f, num684)).RotatedBy(tempName.velocity.ToRotation());
                        projectile.position = tempName.Center + value29 * 16f - tempName.Size / 2f + new Vector2(0f, 0f - Main.projectile[(int)tempName.ai[1]].gfxOffY);
                        projectile.position += tempName.velocity.ToRotation().ToRotationVector2() * num685;
                        projectile.position += zero;
                        projectile.velocity = Vector2.Normalize(tempName.velocity).RotatedBy(num681);
                        projectile.scale = 1.4f * (1f - num683);
                        projectile.damage = tempName.damage;
                        if (tempName.ai[0] >= 180f)
                        {
                            projectile.damage *= 3;
                            vector39 = tempName.Center;
                        }

                        if (!Collision.CanHitLine(Main.player[tempName.owner].Center, 0, 0, tempName.Center, 0, 0))
                            vector39 = Main.player[tempName.owner].Center;

                        projectile.friendly = (tempName.ai[0] > 30f);
                        break;
                    }
            }

            return false;
        }
    }
}
