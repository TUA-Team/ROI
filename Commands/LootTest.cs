using ROI.API.Loot.Targets;
using ROI.Content.Subworlds.Wasteland.WorldBuilding;
using Terraria.ModLoader;

namespace ROI.Commands
{
    internal sealed class LootTest : ModCommand
    {
        public override bool Autoload(ref string name) => ROIMod.DEBUG;

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            switch (args[0])
            {
                case "wasteland":
                    new WastelandChestLoot(new PlayerTarget(caller.Player.whoAmI)).Populate();
                    break;
            }
        }

        public override string Command => "loottest";
        public override CommandType Type => CommandType.World;
    }
}
