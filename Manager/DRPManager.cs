using System;
using System.Deployment.Internal;
using System.Linq;
using DiscordRPC;
using ROI.NPCs.Void.VoidPillar;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;

namespace ROI.Manager
{
    internal sealed class DRPManager : AbstractManager<DRPManager>
    {
        private RichPresence _presence;

        private DiscordRpcClient _client;

        // public static DiscordRpcClient_client => _client;

        private string _currentState;

        private int UpdateCooldown = 100;

        public override void Initialize()
        {
            UnifiedRandom rand = new UnifiedRandom();
            _currentState = Main.netMode == 0
                ? rand.Next(new string[] { "Playing Alone", "Lone Samurai", "Singleplayer" })
                : rand.Next(new string[] { "Playing With Friends", "Multiplayer" });

            _presence = new RichPresence()
            {
                Details = Environment.Is64BitProcess ? "In Main Menu (64x)" : "In Main Menu",
                State = (ROIMod.dev && ROIMod.debug) ? "Debugging/Developing" : _currentState,
                Assets = new Assets()
                {
                    LargeImageKey = "logo",
                    LargeImageText = "Loading mods"
                }
            };
            if (Main.netMode != 0)
            {
                _presence.Party = new Party()
                {
                    Size = Main.ActivePlayersCount,
                    Max = Main.maxNetPlayers
                };
            }
            _client = new DiscordRpcClient("528086919670792233", DevManager.Instance.curSteam, true, -1);
            _client.OnError += (sender, args) =>
            {
                ROIMod.instance.Logger.ErrorFormat("Rich Presence failed. Code {1}, {0}", args.Message, args.Code);
            };
            _presence.Timestamps = new Timestamps()
            {
                Start = DateTime.UtcNow,
            };
            _client.Initialize();
            _client.SetPresence(_presence);
            _client.Invoke();
        }

        // We should get some images pertaining to each boss
        //_client.UpdateLargeAsset("EoC logo", Main.rand.NextBool() ? "Playing Realm of Infinity" : "The start of a new day");
        public void Update()
        {
            UpdateCooldown--;
            if (UpdateCooldown == 0)
            {
                UpdateCooldown = 100;

                if (_client == null)
                    return;


                _presence.Assets.LargeImageKey = "logo";
                if (!Main.gameMenu)
                {
                    _presence.Details = "Playing Terraria";
                    if (Main.npc.Any(i => i.type == ROIMod.instance.NPCType<VoidPillar>()))
                    {
                        _client.UpdateDetails("The void hallucination");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the Void Pillar");
                    }
                    if (NPC.LunarApocalypseIsUp)
                    {
                        _client.UpdateDetails( "The moon is dark tonight");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the Celestial Pillars");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.MoonLordCore))
                    {
                        _client.UpdateDetails( "Upon the final frontier");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the Moon Lord");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.CultistBoss))
                    {
                        _client.UpdateDetails( "The Psychotic Ritual");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the Lunatic Cultist");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.Golem))
                    {
                        _client.UpdateDetails( "The Lizhard Divinity");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the Golem");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.Plantera))
                    {
                        _client.UpdateDetails( "The Jungle Terror");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting Plantera");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.TheDestroyer))
                    {
                        _client.UpdateDetails( "The Mechanical Worm");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the destroyer");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.Retinazer) || (Main.npc.Any(i => i.type == NPCID.Spazmatism)))
                    {
                        _client.UpdateDetails( "The Hallowed Eyes");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the Twins");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.SkeletronPrime))
                    {
                        _client.UpdateDetails( "The Steel Skeleton");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting Skeletron Prime");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.WallofFlesh))
                    {
                        _client.UpdateDetails( "The Lord of Hell");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the Wall of Flesh");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.SkeletronHead))
                    {
                        _client.UpdateDetails( "The Cursed Man");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting Skeletron");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.QueenBee))
                    {
                        _client.UpdateDetails( "NOT THE BEES");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the Queen Bee");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.EaterofWorldsHead))
                    {
                        _client.UpdateDetails( "The Corrupted Abomination");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the Eater of the World");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.BrainofCthulhu))
                    {
                        _client.UpdateDetails( "The Bodiless Brain");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the Brain of Cthulhu");
                    } else if (Main.npc.Any(i => i.type == NPCID.EyeofCthulhu))
                    {
                        _client.UpdateDetails( "The Bodiless Brain");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the Brain of Cthulhu");
                    }

                    // presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing Realm of Infinity" : "exploring the wasteland";
                    // presence.Details = Main.LocalPlayer.name + " is currently in the wasteland";
                    //_client.SetPresence(presence);
                }
                else
                {
                    _presence.Details = Environment.Is64BitProcess ? "In Main Menu (64x)" : "In Main Menu";
                    _presence.State = (ROIMod.dev && ROIMod.debug)
                        ? "Debugging/Developing" : "Doing nothing";

                    _presence.Assets.LargeImageText = "Doing nothing";
                }
                _client.SetPresence(_presence);
            }
            
        }

        internal override void Unload()
        {
            _client.UpdateEndTime(DateTime.UtcNow);
            _client.Dispose();
        }
    }
}
