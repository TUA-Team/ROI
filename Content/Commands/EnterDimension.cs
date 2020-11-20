using ROI.Content.Subworlds.Wasteland;
using SubworldLibrary;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Content.Commands
{
    internal sealed class EnterDimension : ModCommand
    {
        public override bool Autoload(ref string name) => ROIMod.DEBUG;

        public override void Action(CommandCaller caller, string input, string[] args)
        {

            switch (args[0])
            {
                /*                case "rift":
                                    List<Subworld> loaded = (List<Subworld>)typeof(Subworld).GetField("subworlds", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
                                    //typeof(SLWorld).GetField("loading", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, true);
                                    if (!Subworld.Enter<VoidRiftSubworld>())
                                    {
                                        throw new Exception("Cannot enter dimension");
                                    }
                                    break;*/
                case "wasteland":
                    if (!Subworld.Enter<WastelandDepthSubworld>())
                    {
                        Main.NewText("Cannot enter dimension");
                    }
                    break;
                case "overworld":
                    Subworld.Exit();
                    break;
            }
        }

        public override string Command => "LoadDim";
        public override CommandType Type => CommandType.World;
    }
}