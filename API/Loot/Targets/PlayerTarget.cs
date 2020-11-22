using Terraria;

namespace ROI.API.Loot.Targets
{
    public struct PlayerTarget : ILootTarget
    {
        private readonly Player player;

        public PlayerTarget(int player)
        {
            this.player = Main.player[player];
        }

        public void Spawn(LootEntry item)
        {
            player.QuickSpawnItem(item.Type, item.Min);
        }
    }
}
