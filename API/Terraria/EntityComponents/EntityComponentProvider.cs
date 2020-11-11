using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace API.Terraria.EntityComponents
{
    public class EntityComponentProvider : EntityComponent, IEnumerable<EntityComponent>
    {
        public void ActivateComponent<T>(T component) where T : EntityComponent => ActivateComponent(typeof(T), component);

        public void ActivateComponent(Type type, EntityComponent component)
        {
            component.Activate(Entity);
            AddComponent(type, component);
        }

        public virtual void AddComponent(Type type, EntityComponent component)
        {
            dict.Add(type, component);
            update += component.UpdateComponent;
        }

        public virtual T GetComponent<T>() where T : EntityComponent
        {
            return (T)GetComponent(typeof(T));
        }

        public virtual EntityComponent GetComponent(Type type)
        {
            return dict.TryGetValue(type, out var result) ? result : default;
        }

        public virtual void DeactivateComponent<T>() where T : EntityComponent
        {
            if (dict.TryGetValue(typeof(T), out var component))
            {
                component.OnDeactivate();
                dict.Remove(typeof(T));
                update -= component.UpdateComponent;
            }
        }


        private Action update;
        public override void UpdateComponent() => update?.Invoke();


        protected readonly Dictionary<Type, EntityComponent> dict = new Dictionary<Type, EntityComponent>();


        public int Count => dict.Count;

        public IEnumerator<EntityComponent> GetEnumerator() => dict.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => dict.Values.GetEnumerator();
    }

    public class EntityComponentProvider<T> : EntityComponentProvider, IEnumerable<T> where T : EntityComponent
    {
        public new void ActivateComponent<S>(S component) where S : T => base.ActivateComponent(component);

        public override void AddComponent(Type type, EntityComponent component) => base.AddComponent(type, (T)component);

        public new S GetComponent<S>() where S : T => (S)base.GetComponent(typeof(S));

        public new T GetComponent(Type type) => (T)base.GetComponent(type);


        public new IEnumerator<T> GetEnumerator() => dict.Values.Cast<T>().GetEnumerator();
    }
}
