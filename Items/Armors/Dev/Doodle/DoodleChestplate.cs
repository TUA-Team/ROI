using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Items.Armors.Dev.Doodle
{
    [AutoloadEquip(EquipType.Body)]
    class DoodleChestplate : ArmorBase
    {
        public override bool Autoload(ref string name)
        {
            return false;
        }

        public override void ArmorArmGlowMask(Player drawPlayer, float shadow, ref int glowMask, ref Color color)
        {
            
            base.ArmorArmGlowMask(drawPlayer, shadow, ref glowMask, ref color);
        }
    }
}
