using API.Terraria.Biomes;
using API.Terraria.EntityComponents;
using API.Terraria.EntityComponents.Behaviors;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace API.Terraria
{
    public sealed class StandardPlayer : ModPlayer
    {
        public static StandardPlayer Get(Player plr) => plr.GetModPlayer<StandardPlayer>();


        private EntityComponentProvider components;

        public BiomeRegistry BiomeRegistry { get; private set; }

        public override void Initialize()
        {
            components = new EntityComponentProvider();
            components.Activate(player);

            BiomeRegistry = new BiomeRegistry();
            for (int i = 0; i < IdHookLookup<BiomeBase>.Instances.Count; i++)
            {
                var biome = IdHookLookup<BiomeBase>.Instances[i];
                BiomeRegistry.Register(biome);
            }
            BiomeRegistry.Build();

            components.ActivateComponent(BiomeRegistry);
            components.Build();
        }


        public override void UpdateBiomes()
        {
            BiomeRegistry.UpdateComponent();
        }

        public override void CopyCustomBiomesTo(Player other)
        {
            BiomeRegistry.CopyCustomBiomesTo(other);
        }

        public override bool CustomBiomesMatch(Player other)
        {
            return BiomeRegistry.CustomBiomesMatch(other);
        }

        public override void SendCustomBiomes(BinaryWriter writer)
        {
            BiomeRegistry.SendCustomBiomes(writer);
        }

        public override void ReceiveCustomBiomes(BinaryReader reader)
        {
            BiomeRegistry.ReceiveCustomBiomes(reader);
        }


        public void AttachBehavior<T>(T behavior) where T : EntityBehavior
        {
            behavior.Components = components;
            components.ActivateComponent(behavior);
        }

        public void AttachComponent<T>(T component) where T : EntityComponent
        {
            components.ActivateComponent(component);
        }

        public void DeactivateComponent<T>() where T : EntityComponent => components.DeactivateComponent<T>();
    }
}
