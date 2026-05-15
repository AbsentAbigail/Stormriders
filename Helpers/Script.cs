#region

using System;
using UnityEngine;

#endregion

namespace Stormriders.Helpers;

public class Script<T> where T : ScriptableObject, new()
{
    private readonly Action<T> _modifier;
    private readonly string _name;

    public Script()
    {
    }

    public Script(Action<T> modifier)
    {
        _name = typeof(T).Name;
        _modifier = modifier;
    }

    public Script(string name, Action<T> modifier)
    {
        _name = name;
        _modifier = modifier;
    }

    public static implicit operator T(Script<T> script)
    {
        var result = ScriptableObject.CreateInstance<T>();
        result.name = script._name;
        script._modifier?.Invoke(result);
        return result;
    }
}