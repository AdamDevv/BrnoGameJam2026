using System;
using UnityEngine;

namespace APX.Managers.GameObjects
{
    public abstract class ASingleton : MonoBehaviour { }

    public abstract class ASingleton<T> : ASingleton, IDisposable where T : ASingleton<T>
    {
        protected static T _instance;
        private bool _isDuplicateSingleton;

        public static T Instance
        {
            get
            {
                if (_instance is null)
                {
                    Debug.LogWarningFormat("[{0}] Instance called when uninitialized!", typeof(T).Name);
                }

                return _instance;
            }
        }

        protected void Awake()
        {
            if (_instance != null)
            {
                _isDuplicateSingleton = true;
                Destroy(gameObject);
                return;
            }

            _instance = this as T;

            Initialize();
        }

        protected virtual void Initialize() { }

        protected void OnDestroy()
        {
            if (_isDuplicateSingleton)
            {
                return;
            }
            
            Dispose();
        }

        public virtual void Dispose()
        {
            _instance = null;
        }
    }
}
