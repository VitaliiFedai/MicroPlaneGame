using System;

public class GameSettings
{
    public event Action<bool> OnInvertUpDownChange;

    public bool InvertUpDown 
    {
        get => _invertUpDown; 
        set
        {
            bool changed = _invertUpDown != value;
            _invertUpDown = value;

            if (changed) 
            {
                OnInvertUpDownChange?.Invoke(value);
            }
        }
    }
    private bool _invertUpDown;
}