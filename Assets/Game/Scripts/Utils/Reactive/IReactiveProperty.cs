using System;

namespace Brickworks.Utils
{
    public interface IReactiveProperty<T>
    {
        T Value { get; }
        event Action<T> ValueChanged;
    }   
}