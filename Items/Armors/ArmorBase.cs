using Terraria.ModLoader;

namespace ROI.Items.Armors
{
    /// <summary>
    /// @Agrair redo this thing, we'll reuse it
    /// </summary>

    abstract class ArmorBase : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            if (CustomValue(out int value, out byte rare))
            {
                item.value = value;
                item.rare = rare;
            }
            else
            {
                item.value = 10000;
                item.rare = 2;
            }
        }

        protected virtual bool CustomValue(out int value, out byte rare)
        {
            value = 0;
            rare = 0;
            return false;
        }

    //    public override void UpdateEquip(Player player)
    //    {
    //        if (DevSet(out var _) && !SteamID64Checker.Instance.VerifyDevID())
    //        {
    //            DevSetPenalty(player);
    //        }
    //    }

    //    private bool DevSet(out string dev)
    //    {
    //        dev = "";
    //        return false;
    //    }

    //    protected virtual void DevSetPenalty(Player plr) { plr.statLife--; }

    //    public override void ModifyTooltips(List<TooltipLine> tooltips)
    //    {
    //        if (DevSet(out string dev))
    //        {
    //            tooltips.Add(new TooltipLine(mod, "DevSet", $"Thanks for supporting Realm of Infinity! - {dev}"));
    //        }
    //    }
    }
}
