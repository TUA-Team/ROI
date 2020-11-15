using Terraria;

namespace API.ModLoader.EntityComponents
{
    public abstract class EntityHook
    {
        protected Entity Entity { get; private set; }

        public void Activate(Entity entity)
        {
            Entity = entity;
            OnActivate();
        }


        protected virtual void OnActivate()
        {
        }

        public virtual void OnDeactivate()
        {
        }

        public virtual void Update()
        {
        }
    }
}
