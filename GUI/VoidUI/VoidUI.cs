using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROI.Manager;
using ROI.Players;
using Terraria;
using Terraria.Graphics.Shaders;

namespace ROI.GUI.VoidUI
{
    internal class VoidUI
	{
		private static readonly Vector2 DrawingOffset = new Vector2(20f, 170f);
	    private static Texture2D voidMeterFilled;
	    private static Texture2D voidMeterEmpty;

	    public static void Load()
	    {
	        GameShaders.Misc["ROI:RadialProgress"] = new MiscShaderData(new Ref<Effect>(ROIMod.instance.GetEffect("Effects/RadialProgress")), "progress");

            voidMeterFilled = ROIMod.instance.GetTexture("Textures/UIElements/VoidMeterFull");
	        voidMeterEmpty = ROIMod.instance.GetTexture("Textures/UIElements/VoidMeterEmpty");
        }

	    public static void Unload()
	    {
	        voidMeterFilled.Dispose();
	        voidMeterEmpty.Dispose();
        }

	    public static void Draw(SpriteBatch spriteBatch)
		{
			ROIPlayer player = Main.LocalPlayer.GetModPlayer<ROIPlayer>();
			float percent = VoidManager.Instance.Percent(player) / 100f;

            spriteBatch.Draw(voidMeterEmpty, DrawingOffset, null, Color.White, 0f, Vector2.Zero, new Vector2(1f, 1f), SpriteEffects.None, 1f);
            spriteBatch.End();
		    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
		    var radialShader = GameShaders.Misc["ROI:RadialProgress"];
		    radialShader.Shader.Parameters["progress"].SetValue(percent);
            radialShader.Shader.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(voidMeterFilled, DrawingOffset, null, Color.White, 0f, Vector2.Zero, new Vector2(1f, 1f), SpriteEffects.None, 1f);
            spriteBatch.End();
		    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            
            Rectangle textureBound = new Rectangle((int)DrawingOffset.X, (int)DrawingOffset.Y, voidMeterEmpty.Width, voidMeterFilled.Height);

		    if (textureBound.Contains((int) Main.MouseScreen.X, (int) Main.MouseScreen.Y))
		    {
		        Main.hoverItemName = $"Void meter : {player.VoidAffinityAmount}/{player.MaxVoidAffinity}\n" +
		                             $"Percent : {percent * 100}%\n" +
		                             $"Tier : {player.VoidTier}";
		    }
		}


        //TODO: Move this to DrawUtils
		public Texture2D DrawCircle(int diameter, int diameterInterior, float percent)
		{
			Texture2D texture = new Texture2D(Main.graphics.GraphicsDevice, diameter, diameter);
			Color[] colorData = new Color[diameter * diameter];

			float radius = diameter * .5f;
			float radiusInterior = diameterInterior * .5f;
			float radiusSquared = radius * radius;
			float radiusSquaredInterior = radiusInterior * radiusInterior;

			for (int x = 0; x < diameter; x++)
			{
				for (int y = 0; y < diameter; y++)
				{
					int index = x * diameter + y;
					Vector2 pos = new Vector2(x - radius, y - radius);
					float anglePercent = (percent * MathHelper.TwoPi) - MathHelper.Pi;
					float angle = (float)Math.Atan2(pos.Y, pos.X);

                    colorData[index] = (anglePercent > angle 
                        && pos.LengthSquared() < radiusSquared 
                        && pos.LengthSquared() > radiusSquaredInterior) ? Color.White : Color.Transparent;
                }
			}

			texture.SetData(colorData);
			return texture;
		}
	}
}
