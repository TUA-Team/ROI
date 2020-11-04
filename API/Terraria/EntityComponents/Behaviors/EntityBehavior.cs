namespace API.Terraria.EntityComponents.Behaviors
{
    public abstract class EntityBehavior : EntityComponent
    {
        public EntityComponentProvider Components { get; internal set; }
    }
}
