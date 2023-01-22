using UnityEngine;

namespace Talespin.AStar.Utils
{
    /// <summary>
    /// CachedObject. Allows only 1 instance to exist at a time. 
    /// <para>
    /// Unlike a SingletonBehaviour, this script does NOT call DontDestroyOnLoad. CachedBehaviours are thus loaded & unloaded with their respective Scene.
    /// </para>
    /// </summary>
    /// <typeparam name="T">Type for Object</typeparam>
    public abstract class CachedBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Properties
        /// <summary>
        /// Whether an Instance has currently been Cached (instance != null)
        /// </summary>
        public static bool Exists => instance != null;

        /// <summary>
        /// Instance for CachedBehaviour. Calls FindObjectOfType<typeparamref name="T"/> if inner Instance is currently null
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                instance = FindObjectOfType<T>(true); // Try to find an instance (can still potentially be null)
                return instance; // Return found instance (or null if no instance exists)
            }
            protected set { instance = value; }
        }

        /// <summary>
        /// Internal Cache-Reference
        /// </summary>
        protected static T instance;
        #endregion

        #region Methods
        /// <summary>
        /// Instance-Setup. Remember to call base.Awake() if overriding
        /// </summary>
        protected virtual void Awake()
        {
            if (instance != null && !ReferenceEquals(instance, this))
            {
                Debug.LogErrorFormat(instance.gameObject, "CachedBehaviour<{0}> already exists! Existing Object: {1}. Destroying new object {2}", typeof(T).Name, instance.gameObject.name, gameObject.name);
                Destroy(gameObject);
                return;
            }
            instance = this as T;
        }

        /// <summary>
        /// Disposal of Instance
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (instance != null && ReferenceEquals(instance, this))
                instance = null;
        }
        #endregion
    }
}