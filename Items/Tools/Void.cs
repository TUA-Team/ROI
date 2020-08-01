using ROI.Items.Interface;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using ROIPlayer = ROI.Players.ROIPlayer;

namespace ROI.Items.Tools
{
    //Delete me later or give me the binding of isaac void sprite and some actual use
    public class Void : ROIItem, IVoidItem
    {
        public override bool CloneNewInstances => false;

        private IList<int> integratedBuff = new List<int>(10);


        public int VoidTier { get; set; }
        public int VoidCost { get; set; }


        public override ModItem Clone(Item item)
        {
            if (item.modItem is Void voidItem)
            {
                voidItem.integratedBuff = this.integratedBuff;
            }

            return item.modItem;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The void");
            Tooltip.SetDefault("\"The wielder of this legendary item will be in true communion with the Void\"\n" +
                               "Gives a random buff from the Void at a 5 minute cooldown\n" +
                               "Can also grant up to 10 guaranteed potion effects that will last for 4 minutes.");

        }

        public override void TrueSetDefaults()
        {
            item.width = 40;
            item.height = 40;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.value = 0;
            item.expert = true;
        }

        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            tag.Add("potionEffect", integratedBuff);
            return tag;
        }

        public override void Load(TagCompound tag)
        {
            integratedBuff = tag.GetList<int>("potionEffect");
        }

        public override bool CanRightClick()
        {
            Item mouseItem = Main.mouseItem;
            if (AddBuffRequirement(mouseItem))
            {
                integratedBuff.Add(mouseItem.buffType);
                Main.mouseItem.TurnToAir();
            }
            return false;
        }

        public void VoidTierEffect()
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
        }



        public override bool UseItem(Player player)
        {
            ROIPlayer roi_player = player.GetModPlayer<ROIPlayer>();

            if (roi_player.voidItemCooldown == 0 && roi_player.VoidAffinityAmount > 50)
            {
                foreach (var buffID in integratedBuff)
                {
                    player.AddBuff(buffID, 60 * 60 * 4, false);
                }

                roi_player.voidItemCooldown = 60 * 60 * 5;
                roi_player.AddVoidAffinity(-50);
                return true;
            }

            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int expertToolTipIndex = tooltips.FindIndex(i => i.mod == "Terraria" && i.Name == "Expert");
            if (expertToolTipIndex != -1)
            {
                TooltipLine voidCostLine = new TooltipLine(mod, $"VoidSpecification", $"Cost {VoidCost} [c/800080:void affinity point]");
                tooltips.Insert(expertToolTipIndex - 1, voidCostLine);
            }
            if (integratedBuff.Count != 0)
            {
                int previousLineIndex = expertToolTipIndex;

                for (int i = 0; i < integratedBuff.Count; i++)
                {
                    TooltipLine line = new TooltipLine(mod, $"VoidPotion:{ROIUtils.GetBuffName(integratedBuff[i])}", $"[c/00ff00:- {ROIUtils.GetBuffName(integratedBuff[i])}]");
                    tooltips.Insert(previousLineIndex - 1, line);
                    previousLineIndex++;
                }
            }
        }

        private bool AddBuffRequirement(Item mouseItem)
        {
            return mouseItem.stack >= 30 && mouseItem.buffType != 0 && integratedBuff.Count != 10;
        }
    }
}
