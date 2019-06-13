using System.Collections.Generic;
using System.Text;
using MonoMod.RuntimeDetour.HookGen;
using ROI.Players;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ROI.Items
{
	//Delete me later or give me the binding of isaac void sprite and some actual use
	public class Void : ROIItem
	{
	    public override bool CloneNewInstances => false;

	    private IList<int> integratedBuff = new List<int>(10);

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
            Tooltip.SetDefault("\"The wielder of this legendary item will be in true communion with the void\"\n" +
                               "5 minutes cooldown, gives a random buff from the void!\n" +
                               "Can also add up to 10 guaranteed potion effect that will last for 4 minutes!\n" +
                               "Consume 50 void affinity points");
        
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

	    private bool AddBuffRequirement(Item mouseItem)
	    {
	        return mouseItem.stack >= 30 && mouseItem.buffType != 0 && integratedBuff.Count != 10;
	    }

	    public override bool UseItem(Player player)
	    {
	        ROIPlayer roi_player = player.GetModPlayer<ROIPlayer>();

            if (roi_player.voidItemCooldown == 0 && roi_player.VoidAffinityAmount > 50)
	        {
	            foreach (var buffID in integratedBuff)
	            {
	                player.AddBuff(buffID, 60*60*4, false);
	            }

	            roi_player.voidItemCooldown = 60 * 60 * 5;
	            roi_player.AddVoidAffinity(-50);
	            return true;
	        }

	        return false;
	    }

	    public override void ModifyTooltips(List<TooltipLine> tooltips)
	    {
	        if (integratedBuff.Count != 0)
	        {
	            int expertToolTipIndex = tooltips.FindIndex(i => i.mod == "Terraria" && i.Name == "Expert");
	            int previousLineIndex = expertToolTipIndex;
                
	            for (int i = 0; i < integratedBuff.Count; i++)
	            {
                    TooltipLine line = new TooltipLine(mod, $"VoidPotion:{ROIStaticHelper.GetBuffName(integratedBuff[i])}", $"[c/00ff00:- {ROIStaticHelper.GetBuffName(integratedBuff[i])}]");
	                tooltips.Insert(previousLineIndex - 1, line);
	                previousLineIndex++;
	            }
            }
	    }
    }
}
