using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    protected static T _instance;
    public virtual void Awake()
    {
        if (!_instance)
        {
            _instance = this as T;
        }
    }
}