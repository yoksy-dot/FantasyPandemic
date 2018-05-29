using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singletonクラス
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;

    public new static T Instantiate
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }

            return instance;
        }

    }

    public virtual void Awake()
    {
        if (this != Instantiate)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this);
    }
}
