using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingletonBase<T> : MonoBehaviour where T : MonoSingletonBase<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    protected virtual void Awake()
    {
        Instance=this as T;
    }
}
