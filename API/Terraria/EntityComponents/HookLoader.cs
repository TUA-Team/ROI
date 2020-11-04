using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace API.Terraria.EntityComponents
{
    public static class HookLoader
    {
        // Stolen from tML
        public class HookList<T>
        {
            public T[] Impls { get; private set; }
            public readonly MethodInfo method;

            private HookList(MethodInfo method)
            {
                this.method = method;
            }

            public static HookList<T> Create<F>(Expression<Func<T, F>> expr)
            {
                MethodInfo method;

                try
                {
                    var convert = expr.Body as UnaryExpression;
                    var makeDelegate = convert.Operand as MethodCallExpression;
                    var methodArg = makeDelegate.Object as ConstantExpression;
                    method = methodArg.Value as MethodInfo;
                    if (method == null)
                        throw new NullReferenceException();
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Invalid hook expression " + expr, e);
                }

                return new HookList<T>(method);
            }

            public void Build(IEnumerable<T> providers)
            {
                if (!method.IsVirtual)
                    throw new ArgumentException("Cannot build hook for non-virtual method " + method);
                var argTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();
                Impls = providers.Where(p => p.GetType().GetMethod(method.Name, argTypes).DeclaringType != typeof(T)).ToArray();
            }
        }
    }
}
