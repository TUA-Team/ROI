namespace ROI.Items.Lunar.Anchors
{
    internal class SilverCrest : LunarAnchor
    {
        protected override int AnchorValue => 10;
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Silver Crest");
            Tooltip.SetDefault("A traditional coin, one imbued with magic over the ages");
		}
		
		public override void SetDefaults()
		{
            item.width = 22;
            item.height = 32;
            item.value = 100 * 100;
            item.rare = Terraria.ID.ItemRarityID.Blue;
        }
    }
}
