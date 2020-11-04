using API.Terraria.EntityComponents.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static API.Terraria.EntityComponents.HookLoader;

namespace API.Terraria.EntityComponents
{
    public abstract class ComponentRegistry<T>:EntityBehavior where T:EntityComponent
    {
        public EntityComponentProvider<T> RegisteredComponents { get; private set; }

        protected override void OnActivate()
        {
            RegisteredComponents = new EntityComponentProvider<T>();
        }

        public override void UpdateComponent() => RegisteredComponents.UpdateComponent();


        public void Register(T component)
        {
            Components.ActivateComponent(component.GetType(), component);
            RegisteredComponents.AddComponent(component.GetType(), component);
        }

        private readonly List<HookList<T>> hooks = new List<HookList<T>>();

        public void Build()
        {
            RegisteredComponents.Build();
            for (int i = 0; i < hooks.Count; i++)
                hooks[i].Build(RegisteredComponents);
        }

        protected HookList<T> AddHook<F>(Expression<Func<T, F>> expr)
        {
            var hook = HookList<T>.Create(expr);
            hooks.Add(hook);
            return hook;
        }
    }
}
