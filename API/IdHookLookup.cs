using System;
using System.Collections.Generic;

namespace API
{
    public static class IdHookLookup
    {
        public static event Action OnClear;

        public static void Clear() => OnClear?.Invoke();
    }

    public static class IdHookLookup<T> where T : IHaveId
    {
        public static IReadOnlyList<T> Instances => dict;
        private static readonly List<T> dict;

        static IdHookLookup()
        {
            dict = new List<T>();
            IdHookLookup.OnClear += dict.Clear;
        }

        public static void Register<S>(S hook) where S : T
        {
            dict.Add(hook);
            //IdByType.Register(typeof(S), hook.MyId);
        }

        public static T Get(int id) => dict[id];
    }
}
