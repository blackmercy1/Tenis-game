using System;

using UnityEngine;

public static class Level
{
    public static event Action<int> Changed;
    public static int Value
    {
        get => _value;
        set
        {
            if (_value.Equals(value))
                return;

            _value = value;
            Changed?.Invoke(value);
        }
    }
    private static int _value;

    public static void Load() => Value = PlayerPrefs.GetInt(nameof(Level), 0);
    public static void Save() => PlayerPrefs.SetInt(nameof(Level), Value);
}