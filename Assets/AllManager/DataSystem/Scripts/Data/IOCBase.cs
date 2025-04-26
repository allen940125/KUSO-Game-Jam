using System;
using System.Collections.Generic;
public class IOCContainer
{
    private Dictionary<Type, object> mInstances = new Dictionary<Type, object>();

    public void Register<T>(T instance)
    {
        var key = typeof(T);

        if (mInstances.ContainsKey(key))
        {
            mInstances[key] = instance; //m/�i�� 
        }
        else
        {
            mInstances.Add(key, instance);
        }
    }

    public T Get<T>() where T : class
    {
        var key = typeof(T);

        if (mInstances.TryGetValue(key, out var retInstance))
        {
            return retInstance as T;
        }

        return null;
    }
}

public abstract class Architecture<T> where T : Architecture<T>, new()
{
    private static T mArchitecture;

    static void MakeSureArchitecture()
    {
        if (mArchitecture == null)
        {
            mArchitecture = new T();
            mArchitecture.Init();
        }
    }

    protected abstract void Init();

    private IOCContainer mContainer = new IOCContainer();

    public static T Get<T>() where T : class
    {
        MakeSureArchitecture();

        return mArchitecture.mContainer.Get<T>();
    }

    public void Register<T>(T instance)
    {
        MakeSureArchitecture();

        mArchitecture.mContainer.Register<T>(instance);
    }
}

