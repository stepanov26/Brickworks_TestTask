using System;
using System.Collections.Generic;
using Brickworks.Utils;
using Newtonsoft.Json;
using UnityEngine;

public class ResourcesModel : IDisposable
{
    private const string DICTIONARY_KEY = nameof(ResourcesModel) + "_resources";
    
    private readonly Dictionary<ResourceType, int> _resources;

    public event Action<ResourceType> ResourceAmountChanged;

    public ResourcesModel()
    {
        _resources = SaveManager.LoadData(DICTIONARY_KEY, new Dictionary<ResourceType, int>());
    }

    public void AddResource(ResourceData data)
    {
        AddResource(data.Type, data.Amount);
    }
    
    public void AddResource(ResourceType resourceType, int value)
    {
        if (!_resources.TryAdd(resourceType, value))
        {
            _resources[resourceType] += value;
        }

        ResourceAmountChanged?.Invoke(resourceType);
    }

    public void UseResource(ResourceData data)
    {
        UseResource(data.Type, data.Amount);
    }

    public void UseResource(ResourceType resourceType, int value)
    {
        if (!_resources.TryGetValue(resourceType, out var amount) || amount < value)
        {
            return;
        }
        
        _resources[resourceType] -= value;
        ResourceAmountChanged?.Invoke(resourceType);
    }

    public int GetResourceAmount(ResourceType resourceType)
    {
        return _resources.GetValueOrDefault(resourceType, 0);
    }

    public void Dispose()
    {
        SaveManager.SaveData(DICTIONARY_KEY, _resources);
    }
}
