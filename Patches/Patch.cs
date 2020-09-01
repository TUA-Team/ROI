namespace ROI.Patches
{
    static partial class Patch
    {
        internal static void Load()
        {
            On.Terraria.Main.DrawWoF += DrawBossTongues;
            IL.Terraria.Main.DrawMenu += AddThemeToMainMenu;
            IL.Terraria.Lighting.PreRenderPhase += ILLightingPreRenderPhase;
            IL.Terraria.Main.DrawBackground += WastelandBackgroundLightingDraw;
        }

        internal static void Unload()
        {
            On.Terraria.Main.DrawWoF -= DrawBossTongues;
        }
    }
}
