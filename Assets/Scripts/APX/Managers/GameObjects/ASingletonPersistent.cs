namespace APX.Managers.GameObjects
{
    public abstract class ASingletonPersistent<T> : ASingleton<T> where T : ASingletonPersistent<T>
    {
        public new static T Instance => _instance;

        protected override void Initialize()
        {
            base.Initialize();

            _instance.transform.parent = null;
            DontDestroyOnLoad(_instance.gameObject);
        }
    }
}
