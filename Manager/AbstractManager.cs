namespace ROI.Manager
{
    internal abstract class AbstractManager<T> where T : new()
    {
        private static T _instance;

        /// <summary>
        /// Use this to initialize variables instead of overriding the constructor
        /// </summary>
        public virtual void Initialize() { }

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

        ~AbstractManager()
        {
            Unload();
            _instance = default;
        }

        public virtual void Unload() { }
    }
}
