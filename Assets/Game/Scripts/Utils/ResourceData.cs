using System;
using UnityEngine;

namespace Brickworks.Utils
{
    [Serializable]

    public struct ResourceData
    {
        [SerializeField] 
        private ResourceType _type;

        [SerializeField] 
        private int _amount;
        
        public ResourceType Type => _type;
        public int Amount => _amount;

        public ResourceData(ResourceType type, int amount)
        {
            _type = type;
            _amount = amount;
        }
    }
}
