using System;

namespace ROI.Manager
{
	internal abstract class BaseInstanceManager<T> where T : new()
	{
		private static Lazy<T> _instance;

		/// <summary>
		/// Use this to initialize variable instead of overriding the constructor
		/// </summary>
		public abstract void Initialize();

		public static T Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		protected BaseInstanceManager()
		{
			Initialize();
		}

		public void Unload()
		{
			_instance = null;
		}
	}
}
