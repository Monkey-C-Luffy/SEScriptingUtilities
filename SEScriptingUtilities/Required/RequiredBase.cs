namespace SEScriptingUtilities
{
    public abstract class RequiredBase
    {
        protected UtilityManager _utilityManager;
        protected string _name;
        protected string _identifier;
        protected bool _exists;
        protected bool _loaded;
        /// <summary>
        /// The in game display name of the required block or group
        /// </summary>
        public string DisplayName
        {
            get
            {
                return _name;
            }
            protected set
            {
                _name = value;
            }
        }
        /// <summary>
        /// The given identifier for the required block or group
        /// </summary>
        public string Identifier
        {
            get
            {
                return _identifier;
            }
            protected set
            {
                _identifier = value;
            }
        }
        public bool Exists
        {
            get
            {
                return _exists;
            }
            protected set
            {
                _exists = value;
            }
        }
        public bool Loaded
        {
            get
            {
                return _loaded;
            }
            protected set
            {
                _loaded = value;
            }
        }
        public abstract bool Load();
        public abstract bool CheckExists();
    }
}
