using System;
using UnityEngine;

[Serializable]
public class JsonDetails<E>
{

    public virtual void Init()
    {
        // Override if necessary
        // If you need to set data that isn't part of the JSON
        // e.g Calculate derived stats
    }

    public virtual E GetEnumCode()
    {
        Debug.LogError("Override this please!");
        return default(E);
    }
}