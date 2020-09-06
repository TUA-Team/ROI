using System;
using System.Collections.Generic;

namespace API
{
	/// <summary>
	/// This class assumes that there will never ever be another instance of the specified IdBasedObject.
	/// This means that in order to stay consistent, a new system will have to be devised that accounts
	/// for IdBasedObject.uniqueType or something, or any type derived from IdBasedObject must be a singleton
	/// </summary>
	public static class IdHolder
	{
		public static Dictionary<Type, IHaveId> objsByType = new Dictionary<Type, IHaveId>();

		internal static void Link(Type t, Action<byte> update) => objsByType[t].Link(update);

		public static void Register(IHaveId obj) => objsByType.Add(obj.GetType(), obj);
	}

	public static class IdHolder<T> where T : IHaveId
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