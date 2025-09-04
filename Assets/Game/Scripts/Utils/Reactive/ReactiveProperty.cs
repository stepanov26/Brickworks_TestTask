using System;

namespace Brickworks.Utils
{
    public class ReactiveProperty<T> : IReactiveProperty<T>
    {
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if (!Equals(_value, value))
                {
                    _value = value;
                    ValueChanged?.Invoke(_value);
                }
            }
        }

        public event Action<T> ValueChanged;

        public ReactiveProperty(T initialValue = default)
        {
            _value = initialValue;
        }
    }
}