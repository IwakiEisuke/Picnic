﻿using UnityEngine;

public static class ScriptableObjectUtilityRuntime
{
    // https://discussions.unity.com/t/create-copy-of-scriptableobject-during-runtime/598134/7
    public static T Clone<T>(this T scriptableObject) where T : ScriptableObject
    {
        if (scriptableObject == null)
        {
            Debug.LogError($"ScriptableObject was null. Returning default {typeof(T)} object.");
            return (T)ScriptableObject.CreateInstance(typeof(T));
        }

        T instance = Object.Instantiate(scriptableObject);
        return instance;
    }
}