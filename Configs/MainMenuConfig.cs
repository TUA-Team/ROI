using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace ROI.Configs
{
    [Label("Main menu config")]
    class MainMenuConfig : ModConfig
    {
        [DefaultValue(false)]
        [Label("Custom Main Menu")]
        [Tooltip("Allow you to enable TUA custom main menu")]
        public bool CustomMenu;

        [OptionStrings(new string[] { "Vanilla", "Stardust", "Solar", "Nebula", "Vortex"})]
        [DefaultValue("Vanilla" )]
        [Label("Main menu background")]
        public string NewMainMenuTheme;

        public override ConfigScope Mode => ConfigScope.ClientSide;

        public override void OnLoaded()
        {
            ROIMod.menu = this;
        }
    }
}
