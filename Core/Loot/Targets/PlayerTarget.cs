using Terraria;

namespace ROI.Core.Loot.Targets
{
    public class PlayerTarget : LootTarget
    {
        private readonly Player player;

        public PlayerTarget(int player)
        {
            this.player = Main.player[player];
        }

        public override void Spawn(LootEntry item)
        {
            int stack = item.Min > item.Max ? item.Min : Main.rand.Next(item.Min, item.Max);
            player.QuickSpawnItem(item.Type, stack);
        }
    }
}
