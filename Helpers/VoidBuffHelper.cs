using ROI.Buffs.Void;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ROI.Helpers
{
    internal static class VoidBuffHelper
    {
        //possibly move to generics, but it's a little bit of a headache - Agrair
        private static readonly Dictionary<string, VoidBuff> instances;

        static VoidBuffHelper()
        {
            instances = typeof(VoidBuffHelper).Assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.IsSubclassOf(typeof(VoidBuff)))
                .ToDictionary(x => x.Name, x => (VoidBuff)Activator.CreateInstance(x));
        }

        public static VoidBuff GetBuff(string id)
        {
            return instances[id];
        }
    }
}
