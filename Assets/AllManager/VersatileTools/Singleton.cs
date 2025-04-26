using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance;
    private static readonly object _lock = new object();
    private static bool _isQuitting = false;

    public static T Instance
    {
        get
        {
            if (_isQuitting)
            {
                Debug.LogWarning($"[Singleton<{typeof(T)}>] 應用正在退出中，返回 null。");
                return null;
            }

            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null && Application.isPlaying)
                    {
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        _instance = singletonObject.AddComponent<T>();
                        DontDestroyOnLoad(singletonObject);
                        Debug.Log($"[Singleton<{typeof(T)}>] 動態創建 Singleton：{singletonObject.name}");
                    }
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            // 立即銷毀並停止後續代碼執行
            DestroyImmediate(gameObject);
            return;
        }

        _instance = this as T;
        DontDestroyOnLoad(gameObject);

        // 其他初始化代碼...
    }
    protected virtual void OnApplicationQuit()
    {
        _isQuitting = true;
    }
}