using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace ROI.Effects
{
    internal static class UnderworldDarkness
    {
        public static readonly Color hell = new Color(64, 0, 0);

        public static void Load()
        {
            //GameShaders.["ROI:UnderworldFilter"] = new MiscShaderData(new Ref<Effect>(ROIMod.instance.GetEffect("Effects/UnderworldFilter")), "underworld");

            Filters.Scene["ROI:UnderworldFilter"] = new Filter(new ScreenShaderData(new Ref<Effect>(ROIMod.instance.GetEffect("Effects/UnderworldFilter")), "UnderworldFilter"), EffectPriority.VeryHigh);
            Filters.Scene["ROI:UnderworldFilter"].Load();


            Main.graphics.PreparingDeviceSettings += Draw;
        }


        public static void Draw(object oof, PreparingDeviceSettingsEventArgs args)
        {
            if (args.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage != RenderTargetUsage.PreserveContents)
            {
                args.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            }
        }

    }
}
