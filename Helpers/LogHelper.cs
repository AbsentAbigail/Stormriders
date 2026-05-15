#region

using JetBrains.Annotations;
using UnityEngine;

#endregion

namespace Stormriders.Helpers;

public static class LogHelper
{
    public static void Log(object message)
    {
        Debug.Log($"[Stormriders] {message}");
    }

    public static void Warn(object message)
    {
        Debug.LogWarning($"[Stormriders Warning] {message}");
    }

    public static void Error(object message)
    {
        Debug.LogError($"[Stormriders Error] {message}");
    }
}