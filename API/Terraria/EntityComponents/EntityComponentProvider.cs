using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static API.Terraria.EntityComponents.HookLoader;

namespace API.Terraria.EntityComponents
{
    public class EntityComponentProvider : EntityComponent,IReadOnlyList<EntityComponent>
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
            dict.Add(type, components.Count);
            components[components.Count] = component;
        }


        public virtual T GetComponent<T>() where T : EntityComponent
        {
            return (T)GetComponent(typeof(T));
        }

        public virtual EntityComponent GetComponent(Type type)
        {
            return dict.TryGetValue(type, out var result) ? components[result] : default;
        }

        public virtual void DeactivateComponent<T>() where T:EntityComponent
        {
            if (!dict.TryGetValue(typeof(T), out var value))
                return;

            var comp = components[value];
            comp.OnDeactivate();
            dict.Remove(typeof(T));
            components.Remove(comp);
        }


        public override void UpdateComponent()
        {
            foreach (var comp in updateHooks.Impls)
            {
                comp.UpdateComponent();
            }
        }

        public void Build() => updateHooks.Build(components);


        protected readonly HookList<EntityComponent> updateHooks = HookList<EntityComponent>.Create<Action>(x => x.UpdateComponent);


        protected readonly Dictionary<Type, int> dict = new Dictionary<Type, int>();
        protected readonly List<EntityComponent> components = new List<EntityComponent>();


        public int Count => components.Count;

        public EntityComponent this[int index] => components[index];

        public IEnumerator<EntityComponent> GetEnumerator() => components.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => components.GetEnumerator();
    }

    public class EntityComponentProvider<T> : EntityComponentProvider,IReadOnlyList<T> where T:EntityComponent
    {
        public new T this[int index] => (T)components[index];

        public new void ActivateComponent<S>(S component) where S : T => base.ActivateComponent(component);

        public override void AddComponent(Type type, EntityComponent component)
        {
            base.AddComponent(type, (T)component);
        }

        public new S GetComponent<S>() where S : T
        {
            return (S)base.GetComponent(typeof(S));
        }

        public new T GetComponent(Type type)
        {
            return (T)base.GetComponent(type);
        }


        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return dict.Values.Cast<T>().GetEnumerator();
        }
    }
}
