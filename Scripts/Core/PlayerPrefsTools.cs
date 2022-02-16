#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public static class PlayerPrefsTools
{
    [MenuItem("Tools/DevLink/Clear save")]
    public static void Clear()
    {
        PlayerPrefs.DeleteAll();
    }
}
#endif