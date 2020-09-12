using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROI.Content.UI.Elements;
using ROI.Players;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace ROI.Content.UI.Void
{
    internal class VoidAffinity : ROIState
    {
        private readonly Vector2 DrawingOffset = new Vector2(20f, 170f);

        private readonly Texture2D voidMeterFilled;
        private readonly Texture2D voidMeterEmpty;

        public VoidAffinity(Mod mod) : base(mod)
        {
            GameShaders.Misc["ROI:RadialProgress"] = new MiscShaderData(
                new Ref<Effect>((Effect)mod.GetEffect("Assets/Effects/RadialProgress")), "progress");

            voidMeterFilled = (Texture2D)mod.GetTexture("Assets/Textures/Elements/VoidMeterFull");
            voidMeterEmpty = (Texture2D)mod.GetTexture("Assets/Textures/Elements/VoidMeterEmpty");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            ROIPlayer player = ROIPlayer.Get(Main.LocalPlayer);
            float percent = player.VoidAffinity / player.MaxVoidAffinity / 100f;

            spriteBatch.Draw(voidMeterEmpty,
                DrawingOffset,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                new Vector2(1f, 1f),
                SpriteEffects.None,
                1f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            var radialShader = GameShaders.Misc["ROI:RadialProgress"];
            radialShader.Shader.Parameters["progress"].SetValue(percent);
            radialShader.Shader.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Draw(voidMeterFilled,
                DrawingOffset,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                new Vector2(1f, 1f),
                SpriteEffects.None,
                1f);

            spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                Main.DefaultSamplerState,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                Main.GameViewMatrix.TransformationMatrix);

            Rectangle textureBound = new Rectangle((int)DrawingOffset.X,
                (int)DrawingOffset.Y,
                voidMeterEmpty.Width,
                voidMeterFilled.Height);

            if (textureBound.Contains((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y))
            {
                Main.hoverItemName = $"Void meter : {player.VoidAffinity}/{player.MaxVoidAffinity}\n" +
                    $"Percent : {percent * 100}%\n" +
                    $"Tier : {player.VoidTier}";
            }
        }


        // TODO: (low prio) Move this to DrawUtils
        public Texture2D DrawCircle(int diameter, int diameterInterior, float percent)
        {
            Texture2D texture = new Texture2D(Main.graphics.GraphicsDevice, diameter, diameter);
            Color[] colorData = new Color[diameter * diameter];

            float radius = diameter / 2f;
            float radiusInterior = diameterInterior / 2f;
            float radiusSquared = radius * radius;
            float radiusSquaredInterior = radiusInterior * radiusInterior;

            for (int x = 0; x < diameter; x++)
            {
                for (int y = 0; y < diameter; y++)
                {

                    int index = x * diameter + y;
                    Vector2 pos = new Vector2(x - radius, y - radius);
                    float anglePercent = percent * MathHelper.TwoPi - MathHelper.Pi;
                    float angle = (float)Math.Atan2(pos.Y, pos.X);

                    if (anglePercent > angle && pos.LengthSquared() < radiusSquared && pos.LengthSquared() > radiusSquaredInterior)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }
    }
}
