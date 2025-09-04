using System;
using Newtonsoft.Json;
using UnityEngine;

public class SaveManager
{
    private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings()
    {
        NullValueHandling = NullValueHandling.Ignore,
        TypeNameHandling = TypeNameHandling.All
    };
    
    public static bool SaveData<T>(string key, T value)
    {
        if (value == null)
        {
            Debug.LogError("Save manager can't save null value");
            return false;
        }

        if (string.IsNullOrEmpty(key))
        {
            Debug.LogError("Save manager can't save by empty key");
            return false;
        }

        var json = JsonConvert.SerializeObject(value, JsonSettings);
        PlayerPrefs.SetString(key, json);
        return true;
    }
    
    public static T LoadData<T>(string key, T defaultValue = default)
    {
        return (T)LoadData(key, typeof(T), () => defaultValue);
    }
    
    private static object LoadData(string key, Type type, Func<object> defaultValue)
    {
        if (string.IsNullOrEmpty(key))
        {
            Debug.LogError($"{nameof(SaveManager)} can't load by empty key");
            return defaultValue.Invoke();
        }
        
        var json = PlayerPrefs.GetString(key);
        if (string.IsNullOrEmpty(json))
        {
            return defaultValue.Invoke();
        }

        try
        {
            return JsonConvert.DeserializeObject(json, type, JsonSettings);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            return defaultValue.Invoke();
        }
    }
}
