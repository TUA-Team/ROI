using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static API.CustomModLoader.EntityComponents.HookLoader;

namespace API.CustomModLoader.EntityComponents
{
    public abstract class HookRegistry<T> where T : EntityHook
    {
        protected readonly Dictionary<Type, T> registered = new Dictionary<Type, T>();


        public void Update()
        {
            foreach (var hook in registered.Values)
            {
                hook.Update();
            }
        }


        public void Register<S>(S component) where S : T
        {
            registered.Add(typeof(S), component);
        }

        public void Register(T component)
        {
            registered.Add(component.GetType(), component);
        }


        public T GetHook(Type type) => registered[type];


        private readonly List<HookList<T>> hooks = new List<HookList<T>>();

        public void Build()
        {
            for (int i = 0; i < hooks.Count; i++)
                hooks[i].Build(registered.Values);
        }

        protected HookList<T> AddHook<F>(Expression<Func<T, F>> expr)
        {
            var hook = HookList<T>.Create(expr);
            hooks.Add(hook);
            return hook;
        }
    }
}
