using System;
using System.Collections.Generic;

namespace API
{    
	public static class IdHolder
	{
		public static Dictionary<Type, IdBasedObject> objsByType = new Dictionary<Type, IdBasedObject>();

		internal static void Link(Type t, Action<byte> update) => objsByType[t].Link(update);

		public static void Register(IdBasedObject obj) => objsByType.Add(obj.GetType(), obj);
	}

	public static class IdHolder<T> where T : IdBasedObject
	{
		public static byte Id { get; private set; }

		static IdHolder() {
			IdHolder.Link(typeof(T), Link);
		}

		private static void Link(byte instance) {
			Id = instance;
		}
	}
}