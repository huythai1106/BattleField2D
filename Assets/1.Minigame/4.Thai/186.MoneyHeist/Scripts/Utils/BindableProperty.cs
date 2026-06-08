using System;

[Serializable]
public class BindableProperty<T>
{
    private T _value;
    public event Action<T> OnValueChanged;

    public T Value
    {
        get => _value;
        set
        {
            if (!Equals(_value, value))
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }
    }

    public BindableProperty(T initialValue = default)
    {
        _value = initialValue;
    }

    public void Clear()
    {
        OnValueChanged = null;
    }
}
