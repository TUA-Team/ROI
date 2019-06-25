using System;
using System.Linq;
using DiscordRPC;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using static ROI.ROIUtils;

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
            _client.OnJoin += (sender, args) =>
            {
                ROIMod.Log.Info("Successfully joined discord Rich Presence");
                ROIMod.Log.InfoFormat("Current steam user: {0}", DevManager.Instance.curSteam);
            };
            _client.OnReady += (sender, args) =>
            {
                ROIMod.Log.Info("Rich Presence client is ready");
            };
            _client.OnError += (sender, args) =>
            {
                ROIMod.Log.ErrorFormat("Rich Presence failed. Code {1}\n {0}", args.Message, args.Code);
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
            if (--UpdateCooldown == 0)
            {
                UpdateCooldown = 100;

                if (_client == null)
                    return;

                _presence.Assets.LargeImageKey = "logo";
                if (!Main.gameMenu)
                {
                    _presence.Details = "Playing Terraria";
                    if (NPCAlive(Mod.NPCType<NPCs.Void.VoidPillar.VoidPillar>()))
                    {
                        _client.UpdateDetails("The void hallucination");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the Void Pillar");
                    }
                    else if (NPC.LunarApocalypseIsUp)
                    {
                        _client.UpdateDetails("The moon is dark tonight");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the Celestial Pillars");
                    }
                    else if (NPCAlive(NPCID.MoonLordCore))
                    {
                        _client.UpdateDetails("Upon the final frontier");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the Moon Lord");
                    }
                    else if (NPCAlive(NPCID.CultistBoss))
                    {
                        _client.UpdateDetails("The Psychotic Ritual");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the Lunatic Cultist");
                    }
                    else if (NPCAlive(NPCID.Golem))
                    {
                        _client.UpdateDetails("The Lizhard Divinity");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the Golem");
                    }
                    else if (NPCAlive(NPCID.Plantera))
                    {
                        _client.UpdateDetails( "The Jungle Terror");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting Plantera");
                    }
                    else if (NPCAlive(NPCID.TheDestroyer))
                    {
                        _client.UpdateDetails("The Mechanical Worm");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the destroyer");
                    }
                    else if (NPCAlive(NPCID.Retinazer) || NPCAlive(NPCID.Spazmatism))
                    {
                        _client.UpdateDetails("The Hallowed Eyes");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the Twins");
                    }
                    else if (NPCAlive(NPCID.SkeletronPrime))
                    {
                        _client.UpdateDetails("The Steel Skeleton");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting Skeletron Prime");
                    }
                    else if (NPCAlive(NPCID.WallofFlesh))
                    {
                        _client.UpdateDetails("The Lord of Hell");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the Wall of Flesh");
                    }
                    else if (NPCAlive(NPCID.SkeletronHead))
                    {
                        _client.UpdateDetails("The Cursed Man");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting Skeletron");
                    }
                    else if (NPCAlive(NPCID.QueenBee))
                    {
                        _client.UpdateDetails("NOT THE BEES");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the Queen Bee");
                    }
                    else if (NPCAlive(NPCID.EaterofWorldsHead))
                    {
                        _client.UpdateDetails("The Corrupted Abomination");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the Eater of the World");
                    }
                    else if (NPCAlive(NPCID.BrainofCthulhu))
                    {
                        _client.UpdateDetails("The Bodiless Brain");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the Brain of Cthulhu");
                    }
                    else if (NPCAlive(NPCID.EyeofCthulhu))
                    {
                        _client.UpdateDetails("The Bodiless Brain");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the Brain of Cthulhu");
                    }
                    else if (NPCAlive(NPCID.KingSlime))
                    {
                        _client.UpdateDetails("The Slimy Overlord");
                        _client.UpdateState(Main.LocalPlayer.name + " is fighting the King Slime");
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

        /*
        private string BossString(int type)
        {
            return Language.GetTextValue("DRPBossState", Main.LocalPlayer.name,
                NPCLoader.GetNPC(type).DisplayName ?? );
        }
        */

        protected override void UnloadInternal()
        {
            _client.UpdateEndTime(DateTime.UtcNow);
            _client.Dispose();
        }
    }
}
