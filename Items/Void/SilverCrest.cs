namespace ROI.Items.Void
{
    internal class SilverCrest : LunarAnchor
    {
        protected override int Affinity => 10;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Silver Crest");
            Tooltip.SetDefault("A traditional coin, one imbued with magic over the ages\n" +
                "Most formal magicians use currency such as this\n" +
                "The value of each coin varies wizard to wizard, making them \n" +
                "rather capricious when it comes to debt");
            //TODO: move this extraordinarily long text to lore book
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 32;
            item.value = 3 * 100;
            item.rare = Terraria.ID.ItemRarityID.Blue;
        }
    }
}