using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static readonly object _lockObj = new object();
    private static T instance;

    public static T Instance
    {
        get
        {
            lock (_lockObj)
            {
                if (instance != null) 
                    return instance;

                instance = (T)FindObjectOfType(typeof(T));
                if (instance != null) 
                    return instance;

                instance = new GameObject($"{typeof(T).Name} (Singleton)").AddComponent<T>();
                return instance;
            }
        }

        private set
        {
            lock (_lockObj)
            {
                instance = value;
            }
        }
    }

    private void Awake()
    {
        if (instance == null) 
            Instance = GetComponent<T>();

        AwakeInner();
    }

    public static void DeleteInstance()
    {
        Instance = null;
    }

    protected virtual void AwakeInner()
    {

    }

    protected void OnDestroy()
    {
        OnDestroyInner();
        if (instance == this) 
            DeleteInstance();
    }

    protected virtual void OnDestroyInner()
    {

    }
}