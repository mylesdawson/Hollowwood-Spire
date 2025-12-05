using System.Collections.Generic;
using UnityEngine;

public static class Logger
{
    // Stores last logged message per key/tag
    private static readonly Dictionary<string, string> lastMessages = new();

    /// <summary>
    /// Logs only if the message is different from the previous one.
    /// </summary>
    public static void Log(object message, string key = "default")
    {
        string msg = message?.ToString() ?? "null";

        if (ShouldLog(key, msg))
        {
            Debug.Log(msg);
            lastMessages[key] = msg;
        }
    }

    /// <summary>
    /// Logs a warning only if different from previous.
    /// </summary>
    public static void Warning(object message, string key = "default")
    {
        string msg = message?.ToString() ?? "null";

        if (ShouldLog(key, msg))
        {
            Debug.LogWarning(msg);
            lastMessages[key] = msg;
        }
    }

    /// <summary>
    /// Logs an error only if different from previous.
    /// </summary>
    public static void Error(object message, string key = "default")
    {
        string msg = message?.ToString() ?? "null";

        if (ShouldLog(key, msg))
        {
            Debug.LogError(msg);
            lastMessages[key] = msg;
        }
    }

    private static bool ShouldLog(string key, string msg)
    {
        return !lastMessages.TryGetValue(key, out string last) || last != msg;
    }

    /// <summary>
    /// Clears a specific key or all keys.
    /// </summary>
    public static void Clear(string key = null)
    {
        if (key == null)
            lastMessages.Clear();
        else
            lastMessages.Remove(key);
    }
}
