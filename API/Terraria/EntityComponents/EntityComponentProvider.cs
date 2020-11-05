using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static API.Terraria.EntityComponents.HookLoader;

namespace API.Terraria.EntityComponents
{
    public class EntityComponentProvider : EntityComponent,IEnumerable<EntityComponent>
    {
        protected override void OnActivate()
        {
            AddComponent(GetType(), this);
        }

        public void ActivateComponent<T>(T component) where T : EntityComponent => ActivateComponent(typeof(T), component);

        public void ActivateComponent(Type type, EntityComponent component)
        {
            component.Activate(Entity);
            AddComponent(type, component);
        }

        public virtual void AddComponent(Type type, EntityComponent component)
        {
            dict.Add(type, component);
        }

        public virtual T GetComponent<T>() where T : EntityComponent
        {
            return (T)GetComponent(typeof(T));
        }

        public virtual EntityComponent GetComponent(Type type)
        {
            return dict.TryGetValue(type, out var result) ? result : default;
        }


        public override void UpdateComponent()
        {
            foreach (var comp in updateHooks.Impls)
            {
                comp.UpdateComponent();
            }
        }

        public void Build() => updateHooks.Build(dict.Values);


        protected readonly HookList<EntityComponent> updateHooks = HookList<EntityComponent>.Create<Action>(x => x.UpdateComponent);


        protected readonly Dictionary<Type, EntityComponent> dict = new Dictionary<Type, EntityComponent>();


        public int Count => dict.Count;

        public IEnumerator<EntityComponent> GetEnumerator() => dict.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => dict.Values.GetEnumerator();
    }

    public class EntityComponentProvider<T> : EntityComponentProvider,IEnumerable<T> where T:EntityComponent
    {
        public new void ActivateComponent<S>(S component) where S : T => base.ActivateComponent(component);

        public override void AddComponent(Type type, EntityComponent component) => base.AddComponent(type, (T)component);

        public new S GetComponent<S>() where S : T => (S)base.GetComponent(typeof(S));

        public new T GetComponent(Type type) => (T)base.GetComponent(type);


        public new IEnumerator<T> GetEnumerator() => dict.Values.Cast<T>().GetEnumerator();
    }
}
