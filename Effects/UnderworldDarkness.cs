/*
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace ROI.Effects
{
    public static class UnderworldDarkness
    {
        public const string UNDERWORLD_FILTER_NAME = "UnderworldFilter";


        public static void Load() {
            (Filters.Scene[$"{nameof(ROI)}:{UNDERWORLD_FILTER_NAME}"] =
                new Filter(new ScreenShaderData(new Ref<Effect>(ROIMod.Instance.GetEffect($"Effects/{UNDERWORLD_FILTER_NAME}")), UNDERWORLD_FILTER_NAME), EffectPriority.VeryHigh)).Load();

            Main.graphics.PreparingDeviceSettings += Draw;
        }

        public static void Draw(object sender, PreparingDeviceSettingsEventArgs args) =>
            args.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;


        public static Color Hell { get; } = new Color(64, 0, 0);
    }
}
*/