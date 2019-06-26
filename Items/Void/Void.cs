using ROI.Items.Interface;
using ROI.Players;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ROI.Items.Void
{
    // Delete me later or give me the binding of isaac void sprite and some actual use - Dradon
    // I think we should change this to "Sinister Elixir" and use the sprite for something else
    public class Void : VoidItem
    {
        public override bool CloneNewInstances => false;

        private IList<int> integratedBuffs = new List<int>(10);

        public override int VoidTier { get; set; }
        public override int VoidCost { get; set; }

        public override ModItem Clone(Item item)
        {
            if (item.modItem is Void voidItem)
            {
                voidItem.integratedBuffs = integratedBuffs;
            }

            return item.modItem;
        }

        protected override void TrueSetDefaults()
        {
            item.width = 40;
            item.height = 40;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.value = 0;
            item.expert = true;

            integratedBuffs = new List<int>(10);
        }

        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound
            {
                [nameof(integratedBuffs)] = integratedBuffs
            };
            return tag;
        }

        public override void Load(TagCompound tag)
        {
            integratedBuffs = tag.GetList<int>(nameof(integratedBuffs));
        }

        public override bool CanRightClick()
        {
            Item mouseItem = Main.mouseItem;
            if (AddBuffRequirement(mouseItem))
            {
                integratedBuffs.Add(mouseItem.buffType);
                Main.mouseItem.TurnToAir();
            }
            return false;
        }

        public override void VoidTierEffect()
        {
            switch (VoidTier)
            {
                case 0:
                    VoidCost = 50;
                    break;
                case 1:
                    VoidCost = 40;
                    break;
                case 2:
                    VoidCost = 30;
                    break;
                case 3:
                    VoidCost = 20;
                    break;
                case 4:
                    VoidCost = 15;
                    break;
                case 5:
                    VoidCost = 10;
                    break;
                case 6:
                    VoidCost = 5;
                    break;
            }
            /*
            VoidCost = VoidTier switch
            {
                0 => 50,
                1 => 40,
                2 => 30,
                3 => 20,
                4 => 15,
                5 => 10,
                6 => 5
            };
            */
        }

        public override bool UseItem(Player player)
        {
            ROIPlayer modPlayer = player.GetModPlayer<ROIPlayer>();

            if (modPlayer.voidItemCooldown == 0 && modPlayer.VoidAffinityAmount > 50)
            {
                for (int i = 0; i < integratedBuffs.Count; i++)
                {
                    int buffID = integratedBuffs[i];
                    player.AddBuff(buffID, 60 * 60 * 4, false);
                }

                modPlayer.voidItemCooldown = 18000;
                modPlayer.AddVoidAffinity(-50);
                return true;
            }

            return false;
        }

        protected override void TrueModifyTooltips(List<TooltipLine> tooltips, int prevLnIndex)
        {
            if (integratedBuffs.Count != 0)
            {
                for (int i = 0; i < integratedBuffs.Count; i++)
                {
                    TooltipLine line = new TooltipLine(mod, 
                        $"VoidPotion:{ROIUtils.GetBuffName(integratedBuffs[i])}",
                        $"[c/00ff00:- {ROIUtils.GetBuffName(integratedBuffs[i])}]");
                    tooltips.Insert(prevLnIndex - 1, line);
                }
            }
        }

        private bool AddBuffRequirement(Item mouseItem) =>
            mouseItem.stack >= 30 && mouseItem.buffType != 0 && integratedBuffs.Count != 10;
    }
}
