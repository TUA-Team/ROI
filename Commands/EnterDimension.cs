using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ROI.Worlds.Subworlds;
using ROI.Worlds.Subworlds.Wasteland;
using SubworldLibrary;
using Terraria.ModLoader;

namespace ROI.Commands
{
    class EnterDimension : ModCommand
    {
        public override bool Autoload(ref string name) => ROIMod.debug;

        public override void Action(CommandCaller caller, string input, string[] args)
        {

            switch (args[0])
            {
                case "rift" :
                    List<Subworld> loaded = (List<Subworld>) typeof(Subworld).GetField("subworlds", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
                    //typeof(SLWorld).GetField("loading", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, true);
                    if (!Subworld.Enter<VoidRiftSubworld>())
                    {
                        throw new Exception("Cannot enter dimension");
                    }
                    break;
                case "wasteland":
                    if (!Subworld.Enter<TheWastelandDepthSubworld>())
                    {
                        throw new Exception("Cannot enter dimension");
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
