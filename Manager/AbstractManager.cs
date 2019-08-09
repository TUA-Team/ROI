namespace ROI.Manager
{
    internal abstract class AbstractManager<T> where T : new()
    {
        private static T _instance;

        /// <summary>
        /// Use this to initialize variable instead of overriding the constructor
        /// </summary>
        public abstract void Initialize();

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }

                return _instance;
            }
        }

        protected AbstractManager()
        {
            Initialize();
        }

        internal void UnloadInstance()
        {
            _instance = default(T);
            Unload();
        }

        internal virtual void Unload()
        {

        }
    }
}
